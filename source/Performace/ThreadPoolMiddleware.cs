using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace Performace;

public class ThreadPoolMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ThreadPoolSettings _threadPoolSettings=new ThreadPoolSettings();
    private ILogger _logger;
    public ThreadPoolMiddleware(RequestDelegate next, IConfiguration configuration,ILogger logger)
    {
        _next = next;
        configuration.GetSection("ThreadPoolSettings").Bind(_threadPoolSettings);
       _logger = logger;
       
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
        if (workerThreads < _threadPoolSettings.MinWorkerThreads)
            workerThreads = _threadPoolSettings.MinWorkerThreads;
        if (completionPortThreads < _threadPoolSettings.MinCompletionThreads)
            completionPortThreads = _threadPoolSettings.MinCompletionThreads;

        ThreadPool.SetMinThreads(workerThreads, completionPortThreads);
        _logger.LogInformation($"workerThreads={workerThreads} completionPortThreads={completionPortThreads}");

        var monitor = new ThreadPoolMonitor();
        monitor.ThreadPoolSizeIncreased+= ( current,previos)=>
        {
            _logger.LogWarning("");
        };
       
        await _next(context);
    }
}
public static class ThreadPoolMiddlewareExtensions
{
    public static IApplicationBuilder UseThreadPoolMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ThreadPoolMiddleware>();
    }
}

public class ThreadPoolSettings
{
    public int MinWorkerThreads { get; set; }
    public int MinCompletionThreads { get; set; }
}