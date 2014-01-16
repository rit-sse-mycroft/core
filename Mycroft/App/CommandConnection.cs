using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.App
{
    class CommandConnection
    {
        /// <summary>
        /// Source of all commands
        /// </summary>
        public Stream input {get; private set;}
        /// <summary>
        /// Wrap a command connection around a generic input stream
        /// </summary>
        /// <param name="input">The source of all commands</param>
        public CommandConnection(Stream input)
        {
            this.input = input;
        }

        public async Task<string> GetCommandAsync()
        {
            int msgLen = await Task.Run<int>((Func<int>)(GetMsgLen));

            byte[] buff = new byte[msgLen];
            input.Read(buff, 0, buff.Length);
            string msg = Encoding.UTF8.GetString(buff, 0, buff.Length);
            System.Diagnostics.Debug.WriteLine("Got message: " + msg);
            return msg;
        }

        private int GetMsgLen()
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
                catch (ArgumentException) { } // do nothing, it's just not valid yet
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
