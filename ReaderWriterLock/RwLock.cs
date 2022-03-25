using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLock
{
    public class RwLock : IRwLock
    {
        object readDoor = new object();
        AutoResetEvent LockingEvent = new AutoResetEvent(true);
        int b = 0;

        public void ReadLocked(Action action)
        {
            lock (readDoor)
            {
                b++;
                if (b == 1)
                {
                    LockingEvent.WaitOne();
                }
            }

            action();

            lock (readDoor)
            {
                b--;
                if (b == 0)
                {
                    LockingEvent.Set();
                }
            }
        }

        public void WriteLocked(Action action)
        {
            LockingEvent.WaitOne();
            action();
            LockingEvent.Set();
        }
    }
}
