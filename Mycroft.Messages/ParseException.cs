﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Messages
{
    public class ParseException : Exception
    {
        /// <summary>
        /// What we got from the stream: can be anything really, because any
        /// random combination of characters could be invalid.
        /// </summary>
        public string Received { get; private set; }
        /// <summary>
        /// The message you would like to relay to the client.
        /// </summary>
        public string Message { get; private set; }

        public ParseException(string received, string message)
        {
            Received = received;
            Message = message;
        }

        /// <summary>
        /// Serialize this to exception to json as defined in Mycroft.Msg.MsgGeneralFailure.Serialize
        /// </summary>
        /// <returns>The JSON</returns>
        public string Serialize()
        {
            var msgFail = new Msg.MsgGeneralFailure();
            msgFail.Message = Message;
            msgFail.Received = Received;
            return msgFail.Serialize();
        }

        public override string ToString()
        {
            StringBuilder msgBuilder = new StringBuilder();
            msgBuilder.Append("JSON Received: \n");
            msgBuilder.Append(Received);
            msgBuilder.Append("\nMessage: \n");
            msgBuilder.Append(Message);
            return msgBuilder.ToString();
        }

    }
}
