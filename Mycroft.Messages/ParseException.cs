﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Messages
{
    public class ParseException : Exception
    {
        public string Received { get; private set; }
        public string Message { get; private set; }

        public void ParseException(string received, string message)
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
    }
}
