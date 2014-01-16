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
        public ConnectedState(AppInstance instance)
            : base(instance, false)
        { }

        internal override void HandleCommand(Command cmd)
        {

        }
    }
}
