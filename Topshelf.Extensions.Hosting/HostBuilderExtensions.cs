using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Topshelf.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseTopshelfLifetime(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IHostLifetime, TopshelfLifetime>();
            });
        }

        /// <summary>
        /// Builds and run the host as a Topshelf service.
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="options">Configuration of the topshelf service.</param>
        /// <returns></returns>
        public static TopshelfExitCode RunAsTopshelfService(
            this IHostBuilder hostBuilder, 
            Action<HostConfigurators.HostConfigurator> configureTopshelfHost,
            Action<IHost> beforeHostStarting = null,
            Action<IHost> afterHostStopped = null)
        {
            if (configureTopshelfHost == null)
                throw new ArgumentNullException(nameof(configureTopshelfHost));

            hostBuilder.UseTopshelfLifetime();

            IHost host = null;
            try
            {
                var rc = HostFactory.Run(x =>
                {
                    configureTopshelfHost(x);
                    x.Service<IHost>((Action<Topshelf.ServiceConfigurators.ServiceConfigurator<IHost>>)(s =>
                    {
                        s.ConstructUsing(() =>
                        {
                            host = hostBuilder.Build();
                            return host;
                        });
                        s.WhenStarted(service =>
                        {
                            beforeHostStarting?.Invoke(service);

                            service.Start();
                        });
                        s.WhenStopped(service =>
                        {
                            service.StopAsync().Wait();

                            afterHostStopped?.Invoke(service);
                        });
                    }));
                });

                return rc;
            }
            finally
            {
                if (host != null)
                    host.Dispose();
            }
        }
    }
}
