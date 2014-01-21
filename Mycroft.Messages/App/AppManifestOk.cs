﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.IO;

namespace Mycroft.Messages.App
{
    public class AppManifestOk : AppBase
    {
        public string InstanceId { get; set; }

        public override string Serialize()
        {
            var dct = new Dictionary<string, object>();
            dct.Add("instanceId", InstanceId);
            var obj = new DynamicJsonObject(dct);
            var writer = new StringWriter();
            Json.Write(obj, writer);
            return writer.ToString();
        }

        public static new DataPacket Deserialize(string json)
        {
            try
            {
                var ret = new AppManifestOk();
                dynamic obj = Json.Decode(json);
                ret.InstanceId = obj["instanceId"];
                if (ret.InstanceId == null)
                {
                    throw new ParseException(json, "Does not contain instanceId");
                }
                return ret;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                throw new ParseException(json, "General binding exception, is instanceId valid?");
            }
            catch (ArgumentException)
            {
                throw new ParseException(json, "Invalid JSON");
            }
        }
    }
}
