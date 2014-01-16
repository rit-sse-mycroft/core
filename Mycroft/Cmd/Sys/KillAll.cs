using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.App;

namespace Mycroft.Cmd.Sys
{
    class KillAll : SysCommand
    {
        private AppInstance instance;

        public KillAll(String rawData, AppInstance instance)
        {
            this.instance = instance;
        }
    }
}
