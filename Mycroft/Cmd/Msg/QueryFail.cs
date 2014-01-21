using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.App;

namespace Mycroft.Cmd.Msg
{
    class QueryFail : MsgCommand
    {
        private AppInstance Instance;

        public QueryFail(string data, AppInstance instance)
        {
            Instance = instance;
        }
    }
}
