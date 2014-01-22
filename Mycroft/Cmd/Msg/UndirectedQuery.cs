using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.Messages.Msg;
using Mycroft.App;
using System.Diagnostics;

namespace Mycroft.Cmd.Msg
{
    class UndirectedQuery : Query
    {

        public UndirectedQuery(MsgQuery query, AppInstance instance)
            : base(query, instance)
        {
        }

        public override void VisitRegistry(Registry registry)
        {
            if (!HasValidGuid)
            {
                Debug.WriteLine("GUID " + this.guid + " is invalid, not sending query");
                return;
            }

            var cap = GetCapability();
            var msg = "MSG_QUERY " + query.Serialize();

            if (cap != null)
            {
                foreach (AppInstance other in registry.GetProviders(cap))
                {
                    other.Send(msg);
                    ShouldArchive = true;
                }
            }
            else
            {
                Debug.WriteLine("Warning: App did not declare a " + query.Capability + " dependency, not sending message");
            }
        }

        /// <summary>
        /// Get the capability to which this query is referring.
        /// This is determined by the version number supplied in the
        /// dependency that it declared and the name that was supplied
        /// in the query.
        /// </summary>
        /// <returns>the capability or null if not found</returns>
        private Capability GetCapability()
        {
            foreach (Capability cap in FromInstance.Dependencies)
            {
                if (cap.Name == this.query.Capability)
                    return cap;
            }
            return null;
        }
    }
}
