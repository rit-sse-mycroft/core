﻿using Mycroft.Cmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.App
{
    /// <summary>
    /// Delegates AppInstance behavior to a State object that manages how
    /// it's registered with the system
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// Indicates that app instance is registered when it's in this state
        /// </summary>
        public bool IsRegistered { get; private set; }

        /// <summary>
        /// The App Instance in this state
        /// </summary>
        protected AppInstance AppInstance { get; private set; }

        /// <summary>
        /// The dispatcher used by the system
        /// </summary>
        protected Dispatcher Dispatcher { get; private set; }

        internal State(AppInstance instance, Dispatcher dispatcher, bool isRegistered)
        {
            this.AppInstance = instance;
            this.IsRegistered = isRegistered;
            this.Dispatcher = dispatcher;
        }

        /// <summary>
        /// Processes a command object that was presented from the stream
        /// </summary>
        /// <param name="cmd">The command to be used on the system</param>
        internal abstract void HandleCommand(Command cmd);
    }
}
