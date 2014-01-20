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

        public static DataPacket DeSerialize(string json)
        {
            try
            {
                var ret = new AppManifestFail();
                dynamic obj = Json.Decode(json);
                ret.Message = obj["message"];
                return ret;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
            {
                return null;
            }
        }
    }
}
