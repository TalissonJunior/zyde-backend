using System.Reflection;
using Autofac.Extensions.DependencyInjection;

namespace Zyde.WebApi;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) {
        return Host.CreateDefaultBuilder(args)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureWebHostDefaults(webBuilder => { 
            webBuilder
            .UseStartup<Startup>()
            .UseKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = null;
            })
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseSetting("detailedErrors", "true")
            .CaptureStartupErrors(true)
            .UseIISIntegration();
        })
        .ConfigureAppConfiguration((builderContext, config) =>
        {
            config
            .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: false)
            .AddEnvironmentVariables();
        })
        .ConfigureServices(services => services.AddAutofac());
    }
}