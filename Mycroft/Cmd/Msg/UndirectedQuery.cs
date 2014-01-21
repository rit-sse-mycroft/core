using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.Messages.Msg;
using Mycroft.App;

namespace Mycroft.Cmd.Msg
{
    class UndirectedQuery : Query
    {

        public UndirectedQuery(MsgQuery query, AppInstance instance)
            : base(query, instance)
        {

        }
    }
}
