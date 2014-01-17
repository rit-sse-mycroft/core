using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.IO;

namespace Mycroft.Messages.Msg
{
    class MsgQuerySuccess : DataPacket
    {
        public string Id { get; set; }
        public dynamic Ret { get; set; }

        public override string Seralize()
        {
            var dct = new Dictionary<string, object>();
            dct["id"] = Id;
            dct["ret"] = Ret;
            var obj = new DynamicJsonObject(dct);
            var writer = new StringWriter();
            Json.Write(obj, writer);
            return writer.ToString();
        }

        public static DataPacket DeSerialize(string json)
        {
            try
            {
                var ret = new MsgQuerySuccess();
                dynamic obj = Json.Decode(json);
                ret.Id = obj["id"];
                ret.Ret = obj["ret"];
                return ret;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
            {
                return null;
            }
        }
    }
}
