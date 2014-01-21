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
using Mycroft.Messages;
using Mycroft.App;

namespace Mycroft.Cmd.App
{
    class Manifest
    {

        public static AppCommand Parse(string json, AppInstance instance)
        {
            AppManifest mfst;
            try
            {
                mfst = AppManifest.Deserialize(json) as AppManifest;
            }
            catch (ParseException ex)
            {
                return new ManifestFail(ex.ToString(), instance);
            }
            return new Create(mfst, instance);
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
