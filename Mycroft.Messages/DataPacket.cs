using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Messages.Msg
{
    public abstract class DataPacket
    {

        public abstract string Seralize();

        public abstract DataPacket DeSeralize();



    }
}
