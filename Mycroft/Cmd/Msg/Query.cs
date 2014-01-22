﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.App;
using Mycroft.Messages.Msg;

namespace Mycroft.Cmd.Msg
{
    abstract class Query : MsgCommand
    {

        protected MsgQuery query;
        protected AppInstance instance;
        protected bool HasValidGuid;

        public Query(MsgQuery query, AppInstance instance)
        {
            this.instance = instance;
            this.query = query;
            if (instance != null)
                this.query.FromInstanceId = instance.InstanceId;
            this.guid = query.Id;
            this.HasValidGuid = false;
        }

        /// <summary>
        /// Get the Query object associated with this MSG_QUERY
        /// </summary>
        /// <param name="data">the JSON data without a verb</param>
        /// <param name="instance">the app instance that issued this query</param>
        /// <returns>either a DirectedQuery or UndirectedQuery</returns>
        public static new Command Parse(string data, AppInstance instance)
        {
            MsgQuery query = MsgQuery.Deserialize(data) as MsgQuery;
            Query ret = null;
            if (query.InstanceId.Count == 0)
                ret = new UndirectedQuery(query, instance);
            else
                ret = new DirectedQuery(query, instance);
            return ret;
        }

        /// <summary>
        /// Register this query with the message archive.
        /// If the GUID given already exists in the archive the app instance
        /// is notified and HasValidGuid is set to false
        /// </summary>
        /// <param name="messageArchive">The archive to visit</param>
        public override void VisitMessageArchive(MessageArchive messageArchive)
        {
            if (!messageArchive.TryPostMessage(this))
            {
                HasValidGuid = false;
                var genFail = new MsgGeneralFailure();
                genFail.Received = "";
                genFail.Message = "The guid " + this.guid + " was already taken";
                var msg = "MSG_GENERAL_FAILURE " + genFail.Serialize();
                instance.Send(msg);
            }
            else
                HasValidGuid = true;
        }
    
    }
}
