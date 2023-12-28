using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performace;

internal class ThreadPoolMonitor
{
    private int previousThreadCount;
    private ILogger logger;

    public event EventHandler<ThreadPoolEventArgs> ThreadPoolSizeIncreased;


    public ThreadPoolMonitor()
    {
        previousThreadCount = ThreadPool.ThreadCount;
        ThreadPool.QueueUserWorkItem(CheckThreadPoolSize, null);
    }

    private void CheckThreadPoolSize(object state)
    {
        while (true)
        {
            int currentThreadCount = ThreadPool.ThreadCount;

            if (currentThreadCount > previousThreadCount)
            {
                OnThreadPoolSizeIncreased(currentThreadCount, previousThreadCount);

                previousThreadCount = currentThreadCount;
            }

            Thread.Sleep(500); //.net increase thread every 500ms
        }
    }

    protected virtual void OnThreadPoolSizeIncreased(int currentThreadCount, int previousThreadCount)

    {
        ThreadPoolEventArgs args = new ThreadPoolEventArgs(currentThreadCount, previousThreadCount);
        ThreadPoolSizeIncreased?.Invoke(this, args);
    }
}
public class ThreadPoolEventArgs : EventArgs
{
    public int CurrentThreadCount { get; }
    public int PreviousThreadCount { get; }

    public ThreadPoolEventArgs(int currentThreadCount, int previousThreadCount)
    {
        CurrentThreadCount = currentThreadCount;
        PreviousThreadCount = previousThreadCount;
    }
}

