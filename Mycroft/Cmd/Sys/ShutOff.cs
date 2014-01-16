using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.App;

namespace Mycroft.Cmd.Sys
{
    class ShutOff : SysCommand
    {
        private AppInstance instance;

        public ShutOff(String rawData, AppInstance instance)
        {
            this.instance = instance;
        }
    }
}
