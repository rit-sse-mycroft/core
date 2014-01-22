using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.App;
using Mycroft.Messages.Msg;
using System.Diagnostics;

namespace Mycroft.Cmd.Msg
{
    class Broadcast : MsgCommand
    {
        private string verb = "MSG_BROADCAST";
        private dynamic msgContent;
        private List<AppInstance> sendto;

        private string msg;




        public Broadcast(String rawData, AppInstance instance)
        {
            var dp = (MsgBroadcast)MsgBroadcast.Deserialize(rawData);
            guid = dp.Id;
            msgContent = dp.Content;

            var bcast = new MsgBroadcast();
            bcast.FromInstanceId = instance.InstanceId;
            bcast.Id = guid;
            bcast.Content = msgContent;

            msg = verb + ' ' + bcast.Serialize();

            this.FromInstance = instance;
            sendto = new List<AppInstance>();
        }



        override
        public void VisitRegistry(Registry registry)
        {
            //get all the recipenats 
            foreach (var cap in FromInstance.Capabilities)
            {
                sendto.AddRange(registry.GetDependents(cap));
            }

            //send to all the recipenats
            foreach (var appinstance in sendto)
            {
                appinstance.Send(msg);
            }

        }


        override
        public void VisitMessageArchive(MessageArchive messageArchive)
        {
            if (!messageArchive.TryPostMessage(this))
            {
                Debug.WriteLine("failed because message currently exists");
                var fail = new MsgGeneralFailure();
                fail.Message = "Message key '" + guid + "' currently exists in message archive, can't override";
                fail.FromInstanceId = FromInstance.InstanceId;
                fail.Received = "";

                FromInstance.Send("MSG_GENERAL_FAILURE " + fail.Serialize());


            }
        }




    }
}
