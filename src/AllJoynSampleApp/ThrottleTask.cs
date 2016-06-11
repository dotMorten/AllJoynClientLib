using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllJoynSampleApp
{
    /// <summary>
    /// Reduces calls on the network to a certain amount of milliseconds
    /// to avoid saturating a device with requests.
    /// Use this for instance on a brightness slider to reduce the number of calls the slider generates.
    /// If a second call is sent too soon, it gets queued up. If more calls come in, it replaces the pending
    /// one so only the last one will get executed.
    /// </summary>
    public class ThrottleTask
    {
        System.Threading.Timer timer;
        Action pendingAction;
        int milliseconds;
        object lockObj = new object();
        public ThrottleTask(int milliseconds)
        {
            this.milliseconds = milliseconds;
        }

        public void Invoke(Action action)
        {
            Action a = null;
            lock (lockObj)
            {
                if (timer == null)
                {
                    timer = new System.Threading.Timer(callback, null, milliseconds, System.Threading.Timeout.Infinite);
                    a = action;
                }
                else
                {
                    lock (lockObj)
                        pendingAction = action;
                }
            }
            if (a != null) a();
        }

        private void callback(object state)
        {
            Action a = null;
            lock (lockObj)
            {
                if (pendingAction != null)
                {
                    a = pendingAction;
                    pendingAction = null;
                }
                timer.Dispose();
                timer = null;
            }
            if (a != null)
                a();
        }
    }
}
