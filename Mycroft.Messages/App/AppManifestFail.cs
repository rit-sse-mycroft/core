using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.Helpers;

namespace Mycroft.Messages.App
{
    public class AppManifestFail : AppBase
    {
        public string Message { get; set; }

        public override string Serialize()
        {
            var dct = new Dictionary<string, object>();
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
                var ret = new AppManifestFail();
                var obj = Json.Decode(json);
                ret.Message = obj["message"];
                if (ret.Message == null)
                {
                    throw new ParseException(json, "Did not contain 'message'");
                }
                return ret;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                // NOTE: this probably should never be caught because obj["foo"], when foo doesn't exist,
                // just returns null and doesnt throw an exception. But I'm keeping this just in case.
                throw new ParseException(json, "Did not contain 'message'");
            }
            catch (ArgumentException)
            {
                throw new ParseException(json, "Invalid JSON");
            }
        }
    }
}
