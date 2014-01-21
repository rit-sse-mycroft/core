using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.IO;

namespace Mycroft.Messages.App
{
    public class AppInUse : AppBase
    {
        public int Priority { get; set; }

        public override string Serialize()
        {
            var dct = new Dictionary<string, object>();
            dct["priority"] = Priority;
            var obj = new DynamicJsonObject(dct);
            var writer = new StringWriter();
            Json.Write(obj, writer);
            return writer.ToString();
        }

        public new static DataPacket Deserialize(string json)
        {
            try
            {
                var ret = new AppInUse();
                var obj = Json.Decode(json);
                ret.Priority = obj["priority"];
                return ret;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                return null;
            }
        }
    }
}
