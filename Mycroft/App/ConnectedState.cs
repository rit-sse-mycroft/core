using Mycroft.Cmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.App
{
    class ConnectedState : State
    {
        public ConnectedState(AppInstance instance, Dispatcher dispatcher)
            : base(instance, dispatcher, false)
        { }

        internal override void HandleCommand(Command cmd)
        {
            // Should only handle the manifest command, then register the app
            // with the dispatcher
        }
    }
}
