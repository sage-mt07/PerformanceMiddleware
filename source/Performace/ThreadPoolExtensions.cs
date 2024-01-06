using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Performace
{
    public static class ApplicationBuilderExtensions
    {
        public static IHostBuilder ConfigureThreadPool(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((context, services) =>
            {
                var config = context.Configuration;
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<object>>();
                var settings = config.Get<ThreadPoolSettings>();
                ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
                if (workerThreads < settings.MinWorkerThreads)
                    workerThreads = settings.MinWorkerThreads;
                if (completionPortThreads < settings.MinCompletionThreads)
                    completionPortThreads = settings.MinCompletionThreads;

                if (settings != null)
                {

                    ThreadPool.SetMinThreads(workerThreads, completionPortThreads);
                    logger.LogInformation($"workerThreads={workerThreads} completionPortThreads={completionPortThreads}");
                }
                var monitor = new ThreadPoolMonitor();
                monitor.ThreadPoolSizeIncreased += (sender, arguments) =>
                {
                    logger.LogWarning($"warn---- {arguments.PreviousThreadCount} {arguments.CurrentThreadCount}");
                };
            });

        }

    }


}
