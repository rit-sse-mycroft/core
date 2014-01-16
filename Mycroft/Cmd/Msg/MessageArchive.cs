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

        MemoryCache archive;

        TimeSpan timeToLive { get; set; }


        public MessageArchive(Int32 hours = 0, Int32 minutes = 30, Int32 seconds = 0)
        {

            archive = new MemoryCache("ArchiveCache");// <- name is arbatrary 
            timeToLive = new TimeSpan(hours, minutes, seconds);

        }




        public MsgCommand this[String guid]
        {
            get
            {
                rwl.EnterReadLock();
                try
                {

                    return (MsgCommand)archive.Get(guid);

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
                archive.Add(mc.guid, mc, timeCanDie);
            }
            finally
            {
                // Ensure that the lock is released.
                rwl.ExitWriteLock();
            }
        }
    }
}
