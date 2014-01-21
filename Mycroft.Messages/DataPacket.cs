using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Messages
{
    public abstract class DataPacket
    {

        public abstract string Serialize();

        public static DataPacket Deserialize(string json) { return null; }

    }
}
