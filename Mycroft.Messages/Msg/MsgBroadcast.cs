using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Mycroft.Messages;


namespace Mycroft.Messages.Msg
{
    [DataContract]
    class MsgBroadcast : DataPacket
    {

        [DataMember(Name = "id", IsRequired = true)]
        public string Id { get; set; }

        [DataMember(Name = "content", IsRequired = true)]
        public string Content { get; set; }


        override
        public string Seralize(){return null;}

        public static DataPacket DeSeralize(string json) { return null; }
    }
}
