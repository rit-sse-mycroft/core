using Mycroft.Cmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.App
{
    class ActiveState : State
    {
        public ActiveState(AppInstance instance)
            : base(instance, true)
        { }

        internal override void HandleCommand(Command cmd)
        {

        }
    }
}
