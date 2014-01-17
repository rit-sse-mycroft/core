using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Mycroft.Messages.Msg
{
    [DataContract]
    public class MsgQuery
    {
        [DataMember(Name = "id", IsRequired = true)]
        public string Id { get; set; }

        [DataMember(Name = "capability", IsRequired = true)]
        public string Capability { get; set; }

        [DataMember(Name = "action", IsRequired = true)]
        public string Action { get; set; }

        [DataMember(Name = "data", IsRequired = true)]
        public string Data { get; set; }

        [DataMember(Name = "instanceId", IsRequired = true)]
        public List<string> InstanceId { get; set; }

        [DataMember(Name = "priority", IsRequired = true)]
        public int Priority { get; set; }
    }
}
