using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.IO;
using Mycroft.Messages;


namespace Mycroft.Messages.Msg
{
    public class MsgBroadcast : MsgBase
    {

        public string Id { get; set; }

        public dynamic Content { get; set; }


        override
        public string Serialize()
        {
            var dct = new Dictionary<string, object>();
            dct["id"] = Id;
            dct["content"] = Content;
            dct["fromInstanceId"] = FromInstanceId;
            var obj = new DynamicJsonObject(dct);
            var writer = new StringWriter();
            Json.Write(obj, writer);
            return writer.ToString();
        }

        public static new DataPacket DeSerialize(string json)
        {
            try
            {
                var ret = new MsgBroadcast();
                dynamic obj = Json.Decode(json);
                ret.Id = obj["id"];
                ret.Content = obj["content"];
                return ret;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                return null;
            }
        }
    }
}
