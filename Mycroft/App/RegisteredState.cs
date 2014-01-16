using Mycroft.Cmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.App
{
    class RegisteredState : State
    {
        public RegisteredState(AppInstance instance, Dispatcher dispatcher)
            : base(instance, dispatcher, true)
        { }

        internal override void HandleCommand(Command cmd)
        {

        }
    }
}
