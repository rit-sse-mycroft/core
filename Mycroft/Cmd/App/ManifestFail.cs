using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.App;
using Mycroft.Messages.App;

namespace Mycroft.Cmd.App
{

    class ManifestFail : AppCommand
    {
        public AppManifestFail Fail { get; private set; }
        private AppInstance instance;

        public ManifestFail(string message, AppInstance instance)
        {
            Fail = new AppManifestFail();
            Fail.Message = message;
            this.instance = instance;
            instance.Issue(this);
        }

        /// <summary>
        /// Send a message to this app instance that manifest failed
        /// </summary>
        /// <param name="appInstance">the app instance to visit</param>
        public override void VisitAppInstance(AppInstance appInstance)
        {
            if (appInstance == instance)
            {
                appInstance.Send("APP_MANIFEST_FAIL " + Fail.Serialize());
            }
        }
    }
}
