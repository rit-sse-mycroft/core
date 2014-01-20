﻿using Mycroft.Cmd;
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
        /// <summary>
        /// The instances registered in the app
        /// </summary>
        private Dictionary<string, AppInstance> instances = new Dictionary<string, AppInstance>();

        /// <summary>
        /// Maps Capabilities to a list of instanceIds that depend on the capability
        /// </summary>
        private Dictionary<Capability, SortedSet<string>> dependents = new Dictionary<Capability, SortedSet<string>>();

        /// <summary>
        /// Maps Capabilities to a list of instanceIds that provide a capability
        /// </summary>
        private Dictionary<Capability, SortedSet<string>> providers = new Dictionary<Capability, SortedSet<string>>();

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
            var capabilities = instance.Capabilities;

            foreach (var capability in capabilities)
            {
                // Add the capability if it isn't known
                if(!providers.ContainsKey(capability))
                {
                    providers[capability] = new SortedSet<string>();
                }
                providers[capability].Add(instance.InstanceId);

                // Add the dependency if it isn't known
                if(!dependents.ContainsKey(capability))
                {
                    dependents[capability] = new SortedSet<string>();
                }
                dependents[capability].Add(instance.InstanceId);
            }
        
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

        /// <summary>
        /// Gets instances that depend on a capability
        /// </summary>
        /// <param name="capability"></param>
        /// <returns></returns>
        public IEnumerable<AppInstance> GetDependents(Capability capability)
        {
            return dependents[capability].Select(instanceId => instances[instanceId]);
        }

        /// <summary>
        /// Gets instances that provide a capability
        /// </summary>
        /// <param name="capability"></param>
        /// <returns></returns>
        public IEnumerable<AppInstance> GetProviders(Capability capability)
        {
            return providers[capability].Select(instanceId => instances[instanceId]);
        }

        /// <summary>
        /// Returns all instances that depend on or provide a capability
        /// </summary>
        /// <param name="capability"></param>
        /// <returns></returns>
        public IEnumerable<AppInstance> GetAllRelated(Capability capability)
        {
            var all = dependents[capability];
            all.UnionWith(providers[capability]);
            return all.Select(instanceId => instances[instanceId]);
        }

        public void Issue(Command command)
        {
            command.VisitRegistry(this);
        }
    }
}
