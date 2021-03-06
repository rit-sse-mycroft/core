﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.IO;

namespace Mycroft.Messages.Msg
{
    public class MsgQuerySuccess : MsgBase
    {
        public string Id { get; set; }
        public dynamic Ret { get; set; }

        public override string Serialize()
        {
            var dct = new Dictionary<string, object>();
            dct["id"] = Id;
            dct["ret"] = Ret;
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
                var ret = new MsgQuerySuccess();
                dynamic obj = Json.Decode(json);
                ret.Id = obj["id"];
                if (ret.Id == null)
                    throw new ParseException(json, "No id was supplied");
                ret.Ret = obj["ret"];
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
