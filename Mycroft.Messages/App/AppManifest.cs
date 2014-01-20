using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.IO;

namespace Mycroft.Messages.App
{
    public class AppManifest : AppBase
    {
        public string Version { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string InstanceId { get; set; }
        public int API { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Capabilities { get; set; }
        public Dictionary<string, string> Dependencies { get; set; }

        public override string Serialize()
        {
            var dct = new Dictionary<string, object>();
            dct.Add("version", Version);
            dct.Add("API", API);
            dct.Add("description", Description);
            dct.Add("name", Name);
            if (DisplayName != null)
            {
                dct.Add("displayName", DisplayName);
            }
            if (InstanceId != null)
            {
                dct.Add("instanceId", InstanceId);
            }
            if (Capabilities.Count != 0)
            {
                dct.Add("capabilities", Capabilities);
            }
            if (Dependencies.Count != 0)
            {
                dct.Add("dependencies", Dependencies);
            }
            var obj = new DynamicJsonObject(dct);
            var writer = new StringWriter();
            Json.Write(obj, writer);
            return writer.ToString();
        }

        /// <summary>
        /// Returns a new AppManifest from the given json.
        /// If a property is not given in the json then the instance
        /// variable to which it corresponds is set to null.
        /// Capabilities will always be non-null, but may be empty
        /// Dependencies will always be non-null, but also may be empty
        /// If API was not supplied it is set to -1, since ints cannot be nulled
        /// </summary>
        /// <param name="json">the manifest json to parse</param>
        /// <returns>a new AppManifest</returns>
        public static DataPacket DeSerialize(string json)
        {
            try
            {
                var ret = new AppManifest();
                var obj = Json.Decode(json);
                ret.Version = TryGetValueFromDynamic(obj, "version");
                dynamic API = TryGetValueFromDynamic(obj, "API");
                if (API != null)
                    ret.API = TryGetValueFromDynamic(obj, "API");
                else
                    ret.API = -1;
                ret.Description = TryGetValueFromDynamic(obj, "description");
                ret.Name = TryGetValueFromDynamic(obj, "name");
                ret.DisplayName = TryGetValueFromDynamic(obj, "displayName");
                ret.InstanceId = TryGetValueFromDynamic(obj, "instanceId");

                dynamic capabilities = TryGetValueFromDynamic(obj, "capabilities");
                // process the capabilities into a dictionary
                ret.Capabilities = new Dictionary<string, string>();
                if (capabilities != null)
                {
                    var jobj = capabilities as DynamicJsonObject;
                    foreach (string capability in jobj.GetDynamicMemberNames())
                    {
                        ret.Capabilities.Add(capability, capabilities[capability]);
                    }
                }

                dynamic dependencies = TryGetValueFromDynamic(obj, "dependencies");
                // process dependencies into a dictionary
                ret.Dependencies = new Dictionary<string, string>();
                if (dependencies != null)
                {
                    var jobj = dependencies as DynamicJsonObject;
                    foreach (string dependency in jobj.GetDynamicMemberNames())
                    {
                        ret.Dependencies.Add(dependency, dependencies[dependency]);
                    }
                }

                return ret;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
            {
                return null;
            }
        }

        private static object TryGetValueFromDynamic(dynamic obj, string key)
        {
            try
            {
                return obj[key];
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
            {
                return null;
            }
        }
    }
}
