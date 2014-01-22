using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace Mycroft.Cmd.Msg
{
    public class MessageArchive : ICommandable
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

        public Boolean TryPostMessage(MsgCommand mc)
        {
            rwl.EnterWriteLock();
            try
            {


                //TODO check to see that the guid is not in use and if it is send a message back to the sender
                if (archive.Contains(mc.guid))
                {
                    return false;
                }

                var timeCanDie = DateTime.Now + timeToLive;
                archive.Add(mc.guid, mc, timeCanDie);

            }
            finally
            {
                // Ensure that the lock is released.
                rwl.ExitWriteLock();
            }
            return true;
        }


        /// <summary>
        /// Applies a command to the Message archive
        /// </summary>
        /// <param name="command"></param>
        public void Issue(Command command)
        {
            command.VisitMessageArchive(this);
        }


    }
}
