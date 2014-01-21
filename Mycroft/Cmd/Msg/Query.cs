using System;
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

        protected string Id { get; set; }
        protected string Capability { get; set; }
        protected string Action { get; set; }
        protected dynamic Data { get; set; }
        protected int priority;
        protected AppInstance Instance;
        protected List<string> InstanceId;

        public Query(MsgQuery query, AppInstance instance)
        {
            this.Instance = instance;

            this.Id = query.Id;
            this.Capability = query.Capability;
            this.Action = query.Action;
            this.Data = query.Data;
            this.priority = query.Priority;
            this.InstanceId = query.InstanceId;
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
    
    }
}
