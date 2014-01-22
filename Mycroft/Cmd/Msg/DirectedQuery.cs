using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.App;
using Mycroft.Messages.Msg;

namespace Mycroft.Cmd.Msg
{
    class DirectedQuery : Query
    {

        public DirectedQuery(MsgQuery query, AppInstance instance)
            : base(query, instance)
        {
        }

        public override void VisitRegistry(Registry registry)
        {
            if (!HasValidGuid)
            {
                System.Diagnostics.Debug.WriteLine("Guid was invalid: " + guid);
                return;
            }

            foreach (string instanceId in this.query.InstanceId)
            {
                AppInstance toInstance;
                if (registry.TryGetInstance(instanceId, out toInstance))
                {
                    sendQueryTo(toInstance);
                    ShouldArchive = true;
                }
            }
        }

        private void sendQueryTo(AppInstance other)
        {
            var toSend = "MSG_QUERY " + this.query.Serialize();
            other.Send(toSend);
        }
    }
}