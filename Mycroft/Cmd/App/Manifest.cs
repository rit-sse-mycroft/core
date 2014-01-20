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
using Mycroft.Messages.App;
using Mycroft.App;

namespace Mycroft.Cmd.App
{
    public class Manifest : Command
    {
        public AppInstance Instance { get; private set; }
        public string Version { get; set;  }
        public string Name { get; set;  }
        public string DisplayName { get; set;  }
        public string InstanceId { get; set;  }
        public int API { get; set;  }
        public string Description { get; set; }
        public Dictionary<string, string> Capabilities { get; set; }
        public Dictionary<string, string> Dependencies { get; set; }

        public Manifest(string rawData, AppInstance instance)
        {
            Instance = instance;

            var manifest = AppManifest.DeSerialize(rawData) as AppManifest;
            Validate(manifest);

            Version = manifest.Version;
            Name = manifest.Name;
            DisplayName = manifest.DisplayName;
            API = manifest.API;
            Description = manifest.Description;
            Capabilities = manifest.Capabilities;
            Dependencies = manifest.Dependencies;

            if (manifest.InstanceId != null)
            {
                InstanceId = manifest.InstanceId;
            }
        }

        private static void Validate(AppManifest manifest)
        {
            var errors = new ManifestValidationException();
            var versionRegex = new Regex(@"^(\d+(\.\d+(\.\d+)?)?|[*])$");
            if (manifest.Version == null || !versionRegex.IsMatch(manifest.Version))
            {
                Trace.Write("Version Error!!");
                errors.Fields.Add("version", new List<string>());
                errors.Fields["version"].Add("App version number is not semantic");
            }

            if (manifest.API < 0)
            {
                errors.Fields.Add("API", new List<string>());
                errors.Fields["API"].Add("API was not supplied or was invalid");
            }

            if (manifest.DisplayName == null || manifest.DisplayName == "")
            {
                errors.Fields.Add("displayName", new List<string>());
                errors.Fields["displayName"].Add("Display name was not provided");
            }

            if (manifest.Description == null || manifest.Description == "")
            {
                errors.Fields.Add("description", new List<string>());
                errors.Fields["description"].Add("Description was not provided");
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

        public override string Message
        {
            get
            {
                var ret = new StringBuilder();
                foreach (string field in Fields.Keys)
                {
                    foreach (string message in Fields[field])
                    {
                        ret.Append(field);
                        ret.Append(" ");
                        ret.Append(message);
                        ret.Append("\n");
                    }
                }
                return ret.ToString();
            }
        }
    }
}
