![Build](https://github.com/erizet/TopshelfHosting/workflows/Build/badge.svg)
# TopshelfHosting
Do you want to write services the dotnet core way, using the [IHostedService](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostedservice?view=aspnetcore-2.1) interface? Do you also like the convenience of running windows services using [Topshelf?](https://github.com/Topshelf/Topshelf)
Then use this extension method to run your generic host as a Topshelf service.

## Install
Topshelf.Extensions.Hosting is available as a [Nuget-package](https://www.nuget.org/packages/Topshelf.Extensions.Hosting). From the Package Manager Console enter:

        Install-Package Topshelf.Extensions.Hosting
        
## How to use
Build a generic host the normal way. Use the [HostBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostbuilder?view=aspnetcore-2.1) class as you normally do when building an console app. Replace the .RunAsConsole call with a call to the .RunAsTopshelfService extension method.

First create a service that inherits from IHostedService

            class FileWriterService : IHostedService, IDisposable

Then build the generic host to host the service above.

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<FileWriterService>();
                });

The last thing to run this as a Topshelf service is to call the RunAsTopshelfService extension method.

            builder.RunAsTopshelfService(hc =>
            {
                hc.SetServiceName("GenericHostInTopshelf");
                hc.SetDisplayName("Generic Host In Topshelf");
                hc.SetDescription("Runs a generic host as a Topshelf service.");
            });

You'll configure the windows service using the callback on the method. Configuration is done the normal Topshelf way.

## Contribution
I'll be more than happy to get contributions!!!
