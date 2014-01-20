﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.IO;

namespace Mycroft.Messages.Msg
{
    public class MsgQuery : MsgBase
    {
        public string Id { get; set; }

        public string Capability { get; set; }

        public string Action { get; set; }

        public dynamic Data { get; set; }

        public List<string> InstanceId { get; set; }

        public int Priority { get; set; }


        override
        public string Serialize() 
        {
            var dct = new Dictionary<string, object>();
            dct["id"] = Id;
            dct["capability"] = Capability;
            dct["action"] = Action;
            dct["data"] = Data;
            dct["instanceId"] = InstanceId;
            dct["priority"] = Priority;
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
                MsgQuery ret = new MsgQuery();
                dynamic obj = Json.Decode(json);
                ret.Id = obj["id"];
                ret.Capability = obj["capability"];
                ret.Action = obj["action"];
                ret.Data = obj["data"];
                ret.InstanceId = new List<string>();
                DynamicJsonArray instanceIds = obj["instanceId"];
                foreach(object elem in instanceIds) {
                    ret.InstanceId.Add(elem.ToString());
                }
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
