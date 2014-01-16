using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.App;

namespace Mycroft.Cmd.Msg
{
    class Broadcast : MsgCommand
    {
        private AppInstance instance;

        public Broadcast(String rawData, AppInstance instance)
        {
            this.instance = instance;
        }

    }
}
