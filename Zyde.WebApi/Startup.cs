using Zyde.Application.Configurations;
using Zyde.WebApi.Extensions;
using Autofac;
using Zyde.Application.Configurations.Mapper;
using Coffee.WebApi;

namespace Zyde.WebApi;

public sealed class Startup
{
    private readonly IConfigurationSection appSettingsSection;
    private readonly AppSettings appSettings;
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
        appSettingsSection = configuration.GetSection(nameof(AppSettings));
        appSettings = appSettingsSection.Get<AppSettings>();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .Configure<AppSettings>(appSettingsSection)
            .AddSwagger(appSettings)
            .AddControllers();

        services.AddEmails();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new CoffeeModule()
        {
            ConnectionString = appSettings.ConnectionStrings.DefaultConnection
        });

        builder.RegisterInstance(AutoMapperConfig.Initialize()).SingleInstance();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app
            .ConfigureSwagger()
            .UseDeveloperExceptionPage();
        }

        app
        .UseHttpsRedirection()
        .UseRouting()
        .ConfigureCors(appSettings)
        .ConfigureLogs()
        .UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}