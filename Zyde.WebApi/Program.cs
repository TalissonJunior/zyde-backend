using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Coffee.WebApi;

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
            .CoffeeUseUrl(args)
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
            .EnableDefaults()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        })
        .ConfigureServices(services => services.AddAutofac());
    }
}