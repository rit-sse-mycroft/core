using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.Messages;
using System.Runtime.Serialization;


namespace Mycroft.Messages.Msg
{
     [DataContract]
    class MsgBroadcastFail : DataPacket
    {



        [DataMember(Name = "id", IsRequired = true)]
        public string Id { get; set; }

        [DataMember(Name = "message", IsRequired = true)]
        public string Content { get; set; }



        override
        public string Seralize() {return null; }
        override
        public DataPacket DeSeralize() { return null; }


    }
}
