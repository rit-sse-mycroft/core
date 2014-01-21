using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.IO;

namespace Mycroft.Messages.App
{
    public class AppDependency : AppBase
    {
        public Dictionary<string, Dictionary<string, string>> Dependencies { get; set; }

        public AppDependency()
        {
            Dependencies = new Dictionary<string, Dictionary<string, string>>();
        }

        public override string Serialize()
        {
            dynamic obj = new DynamicJsonObject(new Dictionary<string, object>());

            // we need to create nested objects that contain the dependency information
            foreach (string capability in Dependencies.Keys)
            {
                dynamic inner = new DynamicJsonObject(new Dictionary<string, object>());
                foreach (string instanceId in Dependencies[capability].Keys)
                {
                    inner[instanceId] = Dependencies[capability][instanceId];
                }
                obj[capability] = inner;
            }
            var writer = new StringWriter();
            Json.Write(obj, writer);
            return writer.ToString();
        }

        public new static DataPacket Deserialize(string json)
        {
            try
            {
                var ret = new AppDependency();
                ret.Dependencies = new Dictionary<string, Dictionary<string, string>>();

                dynamic obj = Json.Decode(json);
                DynamicJsonObject djobj = obj as DynamicJsonObject;

                // iterate over each of the capabilities
                foreach (string capability in djobj.GetDynamicMemberNames())
                {
                    // iterate over each of the instance ids for this capability
                    DynamicJsonObject inner = obj[capability] as DynamicJsonObject;
                    if (inner == null)
                    {
                        throw new ParseException(json, "JSON not structured correctly");
                    }
                    ret.Dependencies[capability] = new Dictionary<string, string>();
                    foreach (string instanceId in inner.GetDynamicMemberNames())
                    {
                        string status = obj[capability][instanceId];
                        ret.Dependencies[capability][instanceId] = status;
                    }
                }
                return ret;
            }
            catch (System.ArgumentException)
            {
                throw new ParseException(json, "Invalid JSON");
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                throw new ParseException(json, "Valid JSON but invalid content");
            }
        }
    }
}
