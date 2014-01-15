using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace Mycroft.Cmd.Msg
{
    class MessageArchive
    {
        static ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();
        TimeSpan timeToLive;

        MemoryCache archive2;


        public MessageArchive(Int32 hours = 0, Int32 minutes = 30, Int32 seconds = 0)
        {

            archive2 = new MemoryCache("ArchiveCache");

            timeToLive = new TimeSpan(hours, minutes, seconds);

        }

        public MsgCommand this[String guid]
        {
            get
            {
                rwl.EnterReadLock();
                try
                {
                    return (MsgCommand) archive2.Get(guid);
                }
                finally
                {
                    // Ensure that the lock is released.
                    rwl.ExitReadLock();
                }
            }
        }

        public void PostMessage(MsgCommand mc)
        {
            rwl.EnterWriteLock();
            try
            {
                var timeCanDie = DateTime.Now + timeToLive;
                archive2.Add(mc.guid, mc, timeCanDie); 
            }
            finally
            {
                // Ensure that the lock is released.
                rwl.ExitWriteLock();
            }
        }
    }
}
