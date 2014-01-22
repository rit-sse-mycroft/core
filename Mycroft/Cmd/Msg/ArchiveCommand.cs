using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Cmd.Msg
{
    class ArchiveCommand : Command
    {
        private MsgCommand ToArchive;

        public ArchiveCommand(MsgCommand toArchive)
        {
            ToArchive = toArchive;
        }

        public override void VisitMessageArchive(MessageArchive messageArchive)
        {
            if (!messageArchive.TryPostMessage(ToArchive))
            {
                Debug.WriteLine("Tried to post to archive but it failed");
            }
        }
    }
}
