using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performace
{
    public class ThreadPoolSettings
    {
        public int MinWorkerThreads { get; set; }
        public int MinCompletionThreads { get; set; }
    }
}
