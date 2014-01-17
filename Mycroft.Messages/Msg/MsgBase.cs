using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.Messages;

namespace Mycroft.Messages.Msg
{
    public abstract class MsgBase : DataPacket
    {
        public string FromInstanceId { get; set; }

    }
}
