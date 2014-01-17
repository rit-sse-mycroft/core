using Mycroft.Cmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.App
{
    /// <summary>
    /// Stores AppInstances
    /// </summary>
    /// <remarks>Not thread safe; only the Dispatcher's main processing
    /// thread should touch this class</remarks>
    public class Registry : ICommandable
    {
        private Dictionary<string, AppInstance> instances = new Dictionary<string, AppInstance>();

        /// <summary>
        /// Adds an instance to the registry
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>Returns true if the instance has been registered,
        /// false if the instance is already in use</returns>
        public bool Register(AppInstance instance)
        {
            // Make sure the instance ID isn't already in use
            if(instances.ContainsKey(instance.InstanceId))
            {
                return false;
            }

            instances[instance.InstanceId] = instance;
            return true;
        }

        /// <summary>
        /// Removes an app from the registry, used when an instance disconnects
        /// </summary>
        /// <param name="instance"></param>
        /// <return>Returns true if the instance was removed, and false
        /// if it was not found in the registry</return>
        public bool Remove(AppInstance instance)
        {
            return instances.Remove(instance.InstanceId);
        }

        /// <summary>
        /// Retrives an app instance with that ID
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public AppInstance GetAppInstance(string instanceId)
        {
            AppInstance instance;
            instances.TryGetValue(instanceId, out instance);
            return instance;
        }

        public void Issue(Command command)
        {
            command.VisitRegistry(this);
        }
    }
}
