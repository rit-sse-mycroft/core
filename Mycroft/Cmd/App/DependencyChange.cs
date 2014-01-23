using Mycroft.App;
using Mycroft.Messages.App;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mycroft.Cmd.App
{
    class DependencyChange : AppCommand
    {
        private AppInstance instance;

        /// <summary>
        /// Used to notify that an app instance has gone down
        /// </summary>
        /// <param name="instance"></param>
        public DependencyChange(AppInstance instance, Status status)
        {
            this.instance = instance;
            instance.AppStatus = status;
        }

        /// <summary>
        /// Notify dependencies that the app has gone down
        /// </summary>
        /// <param name="registry"></param>
        public override void VisitRegistry(Registry registry)
        {
            Console.WriteLine(
                "{0} {1} is now \"{2}\"",
                instance.DisplayName,
                instance.InstanceId,
                instance.AppStatus.ToString()
            );

            var capabilities = instance.Capabilities;

            // apps that depend on this, mapped to the capabilities they expect it to use
            var apps = new Dictionary<AppInstance, List<string>>();

            // Build the list of things that depend on this
            foreach (var capability in capabilities)
            {
                foreach (var app in registry.GetDependents(capability))
                {
                    if(!apps.ContainsKey(app))
                    {
                        apps[app] = new List<string>();
                    }
                    apps[app].Add(capability.Name);
                }
            }

            // Assemble the message that gets sent for each app
            foreach (var app in apps)
            {
                var message = new AppDependency();
                message.Dependencies = new Dictionary<string, Dictionary<string, string>>();
                foreach(var capability in app.Value)
                {
                    message.Dependencies[capability] = new Dictionary<string, string>()
                    {
                        { instance.InstanceId, instance.AppStatus.ToString() }
                    };
                }

                if (message.Dependencies.Count > 0)
                {
                    app.Key.Send("APP_DEPENDENCY " + message.Serialize());
                }
            }
        }
    }
}
