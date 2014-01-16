using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Cmd
{
    public interface ICommandable
    {
        void Issue(Command command);
    }
}
