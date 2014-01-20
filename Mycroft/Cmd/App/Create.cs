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
            if (manifest.InstanceId != null)
                instance.InstanceId = manifest.InstanceId;
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
        }

        /// <summary>
        /// Send a message to this AppInstance that the manifest was parsed
        /// </summary>
        /// <param name="appInstance">the app to notify</param>
        public override void VisitAppInstance(AppInstance appInstance)
        {
            if (appInstance == instance)
            {
                appInstance.Send("APP_MANIFEST_OK " + okMsg.Serialize());
            }
        }
    }
}
