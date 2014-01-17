using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Messages.Msg
{
    [DataContract]
    class MsgBroadcast
    {

        [DataMember(Name = "id", IsRequired = true)]
        public string Id { get; set; }

        [DataMember(Name = "content", IsRequired = true)]
        public string Content { get; set; }

    }
}
