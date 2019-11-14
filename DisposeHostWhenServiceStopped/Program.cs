using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Topshelf.Extensions.Hosting;

namespace DisposeHostWhenServiceStopped
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await CreateHostBuilder(args).RunConsoleAsync();

            CreateHostBuilder(args).RunAsTopshelfService(s =>
            {
                s.SetServiceName("Name");
                s.SetDisplayName("Name");
            });
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return new HostBuilder()
                .ConfigureServices((h, s) => s
                    .AddHostedService<MyService>()
                    .AddSingleton<Service>());
        }
    }

    public class MyService : IHostedService
    {
        private readonly Service _service;

        public MyService(Service service)
        {
            _service = service;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _service.DoSomething();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class Service : IDisposable
    {
        public void DoSomething()
        {
            throw new Exception("Break things");
        }

        public void Dispose()
        {
            Console.WriteLine("Service is disposed");
        }
    }
}
