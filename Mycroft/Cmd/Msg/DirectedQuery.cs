using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.App;
using Mycroft.Messages.Msg;

namespace Mycroft.Cmd.Msg
{
    class DirectedQuery : Query
    {

        public DirectedQuery(MsgQuery query, AppInstance instance)
            : base(query, instance)
        {

        }
    }
}
