using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.Helpers;

namespace Mycroft.Messages.Msg
{
    public class MsgQueryFail : MsgBase
    {
        public string Id { get; set; }
        public string Message { get; set; }

        public override string Serialize()
        {
            var dct = new Dictionary<string, object>();
            dct["id"] = Id;
            dct["message"] = Message;
            dct["fromInstanceId"] = FromInstanceId;
            var obj = new DynamicJsonObject(dct);
            var writer = new StringWriter();
            Json.Write(obj, writer);
            return writer.ToString();
        }

        public static new DataPacket Deserialize(string json)
        {
            try
            {
                var ret = new MsgQueryFail();
                var obj = Json.Decode(json);
                ret.Id = obj["id"];
                if (ret.Id == null)
                    throw new ParseException(json, "No id supplied");
                ret.Message = obj["message"];
                if (ret.Message == null)
                    throw new ParseException(json, "No message supplied");
                return ret;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                throw new ParseException(json, "General binding exception. Was something invalid?");
            }
            catch (ArgumentException)
            {
                throw new ParseException(json, "Invalid JSON");
            }
        }
    }
}
