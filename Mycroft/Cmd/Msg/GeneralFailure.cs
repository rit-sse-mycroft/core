using Mycroft.App;
using Mycroft.Messages;
using Mycroft.Messages.Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Cmd.Msg
{
    class GeneralFailure : MsgCommand
    {
        private MsgGeneralFailure Failure;
        private AppInstance Instance;

        public GeneralFailure(MsgGeneralFailure failure, AppInstance instance)
        {
            this.Failure = failure;
            this.Instance = instance;
            if (instance != null)
                instance.Issue(this);
        }

        public GeneralFailure(ParseException ex, AppInstance instance)
        {
            this.Failure = new MsgGeneralFailure();
            this.Failure.Received = ex.Received;
            this.Failure.Message = ex.Message;
            this.Instance = instance;
            if (instance != null)
                instance.Issue(this);
        }

        /// <summary>
        /// Send the failure message to our own app instance
        /// </summary>
        /// <param name="appInstance">An app instance</param>
        public override void VisitAppInstance(AppInstance appInstance)
        {
            if (appInstance == this.Instance)
            {
                appInstance.Send("MSG_GENERAL_FAILURE " + Failure.Serialize());
            }
        }
    }
}
