using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.IO;

namespace Mycroft.Messages.Msg
{
    public class MsgGeneralFailure : MsgBase
    {
        public string Received { get; set; }
        public string Message { get; set; }

        public override string Serialize()
        {
            var dct = new Dictionary<string, object>();
            dct["received"] = Received;
            dct["message"] = Message;
            var obj = new DynamicJsonObject(dct);
            var writer = new StringWriter();
            Json.Write(obj, writer);
            return writer.ToString();
        }

        public static new DataPacket Deserialize(string json)
        {
            try
            {
                var ret = new MsgGeneralFailure();
                var obj = Json.Decode(json);
                ret.Message = obj["message"];
                ret.Received = obj["received"];
                return ret;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                return null;
            }
        }
    }
}