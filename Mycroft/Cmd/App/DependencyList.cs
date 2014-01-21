using Mycroft.App;
using Mycroft.Messages.App;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mycroft.Cmd.App
{
    class DependencyList : AppCommand
    {
        private AppInstance instance;

        public DependencyList(AppInstance instance)
        {
            this.instance = instance;
        }

        // Generate the dependency list
        public override void VisitRegistry(Registry registry)
        {
            var dependencies = instance.Dependencies;

            var message = new AppDependency();
            foreach (var capability in dependencies)
            {
                var capabilityList = new Dictionary<string, string>();

                foreach(var appInstance in registry.GetProviders(capability))
                {
                    capabilityList.Add(appInstance.InstanceId, appInstance.AppStatus.ToString());
                }

                message.Dependencies[capability.Name] = capabilityList;
            }

            instance.Send("APP_DEPENDENCY " + message.Serialize());
        }
    }
}
