using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.IO;

namespace Mycroft.Messages.Msg
{
    public class MsgQuery : DataPacket
    {
        public string Id { get; set; }

        public string Capability { get; set; }

        public string Action { get; set; }

        public Dictionary<string, object> Data { get; set; }

        public List<string> InstanceId { get; set; }

        public int Priority { get; set; }


        override
        public string Seralize() { return null; }

        public static DataPacket DeSeralize(string json)
        {
            dynamic obj = Json.Decode(json);
            return null;
        }
    }
}
