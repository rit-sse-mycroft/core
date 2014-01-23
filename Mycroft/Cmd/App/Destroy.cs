using Mycroft.App;
using System;


namespace Mycroft.Cmd.App
{
    class Destroy : AppCommand
    {
        private AppInstance instance;

        public Destroy(AppInstance instance)
        {
            this.instance = instance;
        }

        /// <summary>
        /// Removes an app instance from the registry
        /// </summary>
        /// <param name="registry"></param>
        public override void VisitRegistry(Registry registry)
        {
            registry.Remove(instance);
            Console.WriteLine("{0} {1} has disconnected", instance.DisplayName, instance.InstanceId);
        }
    }
}
