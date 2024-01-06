using Performace;
namespace WorkerService3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                })
                //.ConfigureThreadPool()
                .Build();
            var c=Environment.ProcessorCount;
            host.Run();
        }
    }
}