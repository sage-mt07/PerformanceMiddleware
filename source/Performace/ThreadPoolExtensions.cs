using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performace
{
    public static class ApplicationBuilderExtensions
    {
        public static IHostApplicationBuilder ConfigureThreadPool(this IHostApplicationBuilder hostBuilder)
        {
            var settings = hostBuilder.Configuration.Get<ThreadPoolSettings>();
            ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
            if (workerThreads < settings.MinWorkerThreads)
                workerThreads = settings.MinWorkerThreads;
            if (completionPortThreads < settings.MinCompletionThreads)
                completionPortThreads = settings.MinCompletionThreads;


            if (settings != null)
            {
                ThreadPool.SetMinThreads(settings.MinWorkerThreads, settings.MinCompletionThreads);
                _logger.LogInformation($"workerThreads={workerThreads} completionPortThreads={completionPortThreads}");
            }
            var monitor = new ThreadPoolMonitor();
            monitor.ThreadPoolSizeIncreased += (current, previos) =>
            {
               // _logger.LogWarning("");
            };
            return hostBuilder;
        }
    }
}
