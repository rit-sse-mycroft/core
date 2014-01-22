using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.IO;
using System.Dynamic;
using System.Diagnostics;

namespace Mycroft.Messages.Msg
{
    public class MsgQuery : MsgBase
    {
        public string Id { get; set; }

        public string Capability { get; set; }

        public string Action { get; set; }

        public Dictionary<string, object> Data { get; set; }

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

        public static new DataPacket Deserialize(string json)
        {
            try
            {
                MsgQuery ret = new MsgQuery();
                dynamic obj = Json.Decode(json);
                ret.Id = obj["id"];
                if (ret.Id == null)
                    throw new ParseException(json, "No id found");
                ret.Capability = obj["capability"];
                if (ret.Capability == null)
                    throw new ParseException(json, "No capability found");
                ret.Action = obj["action"];
                if (ret.Action == null)
                    throw new ParseException(json, "No action found");
                ret.Data = ParseDataThing(obj["data"]);
                ret.InstanceId = new List<string>();
                DynamicJsonArray instanceIds = obj["instanceId"];
                if (instanceIds != null)
                {
                    foreach (object elem in instanceIds)
                    {
                        ret.InstanceId.Add(elem.ToString());
                    }
                }
                ret.Priority = obj["priority"];
                return ret;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                throw new ParseException(json, "General binding exception. Is something invalid?");
            }
            catch (ArgumentException)
            {
                throw new ParseException(json, "Invalid JSON");
            }
        }

        /// <summary>
        /// Returns either a dictionary<string, object>, list<string>,
        /// or other primative
        /// </summary>
        /// <param name="obj">the object to parse</param>
        /// <returns>what this object is</returns>
        private static Object ParseDataThing(dynamic obj)
        {
            // parse as a dictionary
            if (obj is DynamicJsonObject)
            {
                var djobj = obj as DynamicJsonObject;
                dynamic dobj = obj;
                var ret = new Dictionary<string, object>();
                foreach (string key in djobj.GetDynamicMemberNames())
                {
                    ret[key] = ParseDataThing(dobj[key]);
                }
                return ret;
            }
            else if (obj is DynamicJsonArray)
            {
                var djarr = obj as DynamicJsonArray;
                var ret = new List<object>();
                for (var i = 0; i < djarr.Count(); i++)
                {
                    ret.Add(djarr[i]);
                }
                return ret;
            }
            else
            {
                return obj;
            }
        }
    }
}
