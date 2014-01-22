using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.App;
using Mycroft.Messages.Msg;

namespace Mycroft.Cmd.Msg
{
    class Reply : MsgCommand
    {
        private AppInstance instance;
        private MsgQuerySuccess msgQSuc;

        public Reply(String rawData, AppInstance instance)
        {
            this.msgQSuc = MsgQuerySuccess.Deserialize(rawData) as MsgQuerySuccess;
            this.instance = instance;
            msgQSuc.FromInstanceId = instance.InstanceId;
        }

        public override void VisitMessageArchive(MessageArchive messageArchive)
        {
            var msg = "MSG_QUERY_SUCCESS " + msgQSuc.Serialize();
            var toInst = messageArchive[msgQSuc.Id];
            if (toInst != null)
            {
                toInst.FromInstance.Send(msg);
            }
        }
    }
}
