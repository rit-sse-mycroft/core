using Mycroft.App;
using System;
using Mycroft.Messages.App;

namespace Mycroft.Cmd.App
{
    class Create : AppCommand
    {
        private AppManifest manifest;
        private AppInstance instance;
        private AppManifestOk okMsg;
        private ManifestFail failure;

        public Create(AppManifest manifest, AppInstance instance)
        {
            this.manifest = manifest;
            this.instance = instance;
            this.okMsg = new AppManifestOk();
            this.okMsg.InstanceId = manifest.InstanceId != null ? manifest.InstanceId : instance.InstanceId;
        }

        /// <summary>
        /// Copy all information from the manifest into the new app instance
        /// and add it to the registry
        /// </summary>
        /// <param name="registry">the registry being visited</param>
        public override void VisitRegistry(Registry registry)
        {
            // Drop the connection if the instance ID is a duplicate
            if (registry.HasInstance(manifest.InstanceId))
            {
                failure = new ManifestFail("Instance ID " + manifest.InstanceId + " already in use", instance);
                return;
            }

            if (manifest.InstanceId != null)
            {
                instance.InstanceId = manifest.InstanceId;
            }
            instance.Name = manifest.Name;
            instance.Version = new Version(manifest.Version);
            instance.ApiVersion = (uint) manifest.API;
            instance.Description = manifest.Description;
            instance.DisplayName = manifest.DisplayName;
            foreach (string capability in manifest.Capabilities.Keys)
            {
                var c = new Capability(
                    capability,
                    new Version(manifest.Capabilities[capability])
                );
                instance.AddCapability(c);
            }
            foreach (string capability in manifest.Dependencies.Keys)
            {
                var c = new Capability(
                    capability,
                    new Version(manifest.Dependencies[capability])
                );
                instance.AddDependency(c);
            }
            registry.Register(instance);

            // We have to send the message to the instance on our own
            instance.AppStatus = Status.down;
            instance.Send("APP_MANIFEST_OK " + okMsg.Serialize());
            Console.WriteLine("\"{0}\" created with instance ID {1}", instance.DisplayName, instance.InstanceId);
        }

        /// <summary>
        /// Create a dependency list message and send it to the dispatcher
        /// </summary>
        /// <param name="dispatcher"></param>
        public override void VisitDispatcher(Dispatcher dispatcher)
        {
            if (failure != null)
            {
                dispatcher.Enqueue(failure);
            }
            else
            {
                dispatcher.Enqueue(new DependencyList(instance));
            }
        }
    }
}
