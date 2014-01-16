using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.App;

namespace Mycroft.Cmd.Sys
{
    class SysUnlock : SysCommand
    {
        private AppInstance instance;

        public SysUnlock(String rawData, AppInstance instance)
        {
            this.instance = instance;
        }
    }
}
