using Zyde.Application.Configurations;

namespace Zyde.WebApi.Extensions;

public static class CorsExtensions
{ 
    public static IApplicationBuilder ConfigureCors(
        this IApplicationBuilder app, 
        AppSettings appSettings
    )
    {
        app.UseCors(builder =>
        {
            builder
            .WithOrigins(appSettings.Urls.ClientUrl)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });

        return app;
    }
}