using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft
{
    class CommandConnection
    {
        /// <summary>
        /// Source of all commands
        /// </summary>
        private Stream input;
        /// <summary>
        /// Wrap a command connection around a generic input stream
        /// </summary>
        /// <param name="input">The source of all commands</param>
        public CommandConnection(Stream input)
        {
            this.input = input;
        }

        public async Task<string> getCommandAsync()
        {
            int msgLen = await Task.Run<int>((Func<int>)(getMsgLen));

            byte[] buff = new byte[msgLen];
            input.Read(buff, 0, buff.Length);
            string msg = Encoding.UTF8.GetString(buff, 0, buff.Length);
            System.Diagnostics.Debug.WriteLine("Got message: " + msg);
            return msg;
        }

        private int getMsgLen()
        {
            byte[] smallBuf = new byte[100];
            string soFar = "";
            for (int i = 0; i < smallBuf.Length; i++ ) // read until we find a newline
            {
                smallBuf[i] = (byte)input.ReadByte();
                try
                {
                    soFar = Encoding.UTF8.GetString(smallBuf, 0, i+1);
                    if (soFar.EndsWith("\n"))
                    {
                        break;
                    }
                }
                catch (ArgumentException ex) { } // do nothing, it's just not valid yet
            }
            soFar.Trim();
            return int.Parse(soFar);
        }

        /// <summary>
        /// Closes the underlying connection
        /// </summary>
        public void Close()
        {
            input.Close();
        }
    }
}
