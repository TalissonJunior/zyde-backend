using Zyde.Application.Configurations;
using Autofac;
using Zyde.Application.Configurations.Mapper;
using Coffee.WebApi;
using Coffee.Application.Configurations;
using Zyde.Application.Protocols;
using Zyde.WebApi.BackgroundServices;
using StackExchange.Redis;

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
        services.Configure<AppSettings>(appSettingsSection);
        services.Configure<CoffeeAppSettings>(appSettingsSection);
        services.AddControllers();

        services.AddDefaults(appSettings);

        // Register ProtocolListener
        services.AddSingleton<ProtocolListener>();

        // Register the hosted service
        services.AddHostedService<ProtocolListenerHostedService>();

         services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configurationOptions = appSettings.ConnectionStrings.RedisConnection;
            return ConnectionMultiplexer.Connect(configurationOptions);
        });

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = appSettings.ConnectionStrings.RedisConnection;
            options.InstanceName = "zyde";
        });
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
        
        app.UseDefaults(appSettings, env);

        app
        .UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}