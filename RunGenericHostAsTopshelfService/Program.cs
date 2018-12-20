using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Topshelf.Extensions.Hosting;

namespace RunGenericHostAsTopshelfService
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<FileWriterService>();
                });

            builder.RunAsTopshelfService(hc =>
            {
                hc.SetServiceName("GenericHostInTopshelf");
                hc.SetDisplayName("Generic Host In Topshelf");
                hc.SetDescription("Runs a generic host as a Topshelf service.");
            });
        }
    }
}
