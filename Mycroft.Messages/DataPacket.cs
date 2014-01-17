using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Messages
{
    public abstract class DataPacket
    {

        public abstract string Seralize();

        public static DataPacket DeSeralize(string json) { return null; }

    }
}
