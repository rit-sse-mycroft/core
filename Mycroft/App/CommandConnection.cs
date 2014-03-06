using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.App
{
    public class CommandConnection
    {
        /// <summary>
        /// Source of all commands
        /// </summary>
        public Stream Input {get; private set;}

        /// <summary>
        /// Manages the TCP connection under the stream
        /// </summary>
        public TcpClient Client { get; private set; }

        /// <summary>
        /// Wrap a command connection around a generic input stream
        /// </summary>
        /// <param name="input">The source of all commands</param>
        public CommandConnection(TcpClient client, Stream input)
        {
            this.Input = input;
            this.Client = client;
        }

        public string GetCommand()
        {
            int msgLen = GetMsgLen();

            byte[] buff = new byte[msgLen];
            Input.Read(buff, 0, buff.Length);
            string msg = Encoding.UTF8.GetString(buff, 0, buff.Length);
            System.Diagnostics.Debug.WriteLine("Got message: " + msg);
            return msg;
        }

        /// <summary>
        /// Writes a message back to the host
        /// </summary>
        /// <param name="message">The message to write, including the message type tag and JSON body</param>
        /// <returns>Returns a Task for async operation</returns>
        public async Task SendMessageAsync(string message)
        {
            var size = Encoding.UTF8.GetByteCount(message);
            string fullMessage = size.ToString() + "\n" + message;
            byte[] data = Encoding.UTF8.GetBytes(fullMessage);
            await Input.WriteAsync(data, 0, data.Length);
        }

        private int GetMsgLen()
        {
            byte[] smallBuf = new byte[100];
            string soFar = "";
            for (int i = 0; i < smallBuf.Length; i++ ) // read until we find a newline
            {
                smallBuf[i] = (byte)Input.ReadByte();
                if (smallBuf[i] == 255)
                {
                    throw new IOException("Stream closed (255 was read)");
                }
                try
                {
                    soFar = Encoding.UTF8.GetString(smallBuf, 0, i+1);
                    if (soFar.EndsWith("\n"))
                    {
                        break;
                    }
                }
                catch (ArgumentException) { } // do nothing, it's just not valid yet
            }
            try
            {
                return int.Parse(soFar.Trim());
            }
            catch (FormatException)
            {
                throw new IOException("Application sent non-parsable message length");
            }
        }

        /// <summary>
        /// Closes the underlying connection
        /// </summary>
        public void Close()
        {
            try
            {
                Input.Dispose();
                Client.Close();
            }
            catch (IOException) { }
        }
    }
}
