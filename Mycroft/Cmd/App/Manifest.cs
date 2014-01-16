using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mycroft.Cmd.App
{
    [DataContract]
    public class Manifest : Command
    {
        [DataMember(Name = "version", IsRequired = true )]
        public string Version { get; set;  }
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set;  }
        [DataMember(Name = "displayName", IsRequired = true)]
        public string DisplayName { get; set;  }
        [DataMember(Name = "instanceId")]
        public string InstanceId { get; set;  }
        [DataMember(Name = "API", IsRequired = true)]
        public int API { get; set;  }
        [DataMember(Name = "description", IsRequired = true)]
        public string Description;
        [DataMember(Name = "capabilities")]
        public Dictionary<string, string> Capabilities { get; set; }
        [DataMember(Name = "dependencies")]
        public Dictionary<string, string> Dependencies { get; set; }

        public static Manifest Parse(string manifestJson)
        {
            var settings = new DataContractJsonSerializerSettings();
            settings.UseSimpleDictionaryFormat = true;
            var serializer = new DataContractJsonSerializer(typeof(Manifest), settings);
            Manifest manifest;
            var memStream = new MemoryStream(Encoding.UTF8.GetBytes(manifestJson));
            manifest = serializer.ReadObject(memStream) as Manifest;
            Validate(manifest);
            return manifest;
        }

        private static void Validate(Manifest manifest)
        {
            var errors = new ManifestValidationException();
            var versionRegex = new Regex(@"^(\d+(\.\d+(\.\d+)?)?|[*])$");
            if (!versionRegex.IsMatch(manifest.Version))
            {
                Trace.Write("Version Error!!");
                errors.Fields.Add("version", new List<string>());
                errors.Fields["version"].Add("App version number is not semantic");
            }
            
            foreach(var item in manifest.Capabilities){
                if (!versionRegex.IsMatch(item.Value))
                {
                    if (!errors.Fields.ContainsKey("capabilities"))
                        errors.Fields.Add("capabilities", new List<string>());
                    errors.Fields["capabilities"].Add("Capability version number for " + item.Key + "is not semantic");
                }
            }

            foreach (var item in manifest.Dependencies)
            {
                if (!versionRegex.IsMatch(item.Value))
                {
                    if(!errors.Fields.ContainsKey("dependencies"))
                        errors.Fields.Add("dependencies", new List<string>());
                    errors.Fields["dependencies"].Add("Dependency version number for " + item.Key + "is not semantic");
                }
            }

            Trace.Write(errors.Fields.Count);
            if (errors.Fields.Count > 0)
                throw errors;

        }
    }

    class ManifestValidationException : Exception
    {
        public Dictionary<string, List<string>> Fields { get; set; }

        public ManifestValidationException() : base() {
            Fields = new Dictionary<string,List<string>>();

        }
    }
}
