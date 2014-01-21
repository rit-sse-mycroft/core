using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.App;

namespace Mycroft.Cmd.Msg
{
    abstract class Query : MsgCommand
    {

        /// <summary>
        /// Get the Query object associated with this MSG_QUERY
        /// </summary>
        /// <param name="data">the JSON data without a verb</param>
        /// <param name="instance">the app instance that issued this query</param>
        /// <returns>either a DirectedQuery or UndirectedQuery</returns>
        public static new Command Parse(string data, AppInstance instance)
        {
            return null;
        }
    
    }
}
