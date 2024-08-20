using Microsoft.Extensions.FileProviders;

namespace Zyde.WebApi.Extensions;

public static class LogsExtensions
{ 
    public static IApplicationBuilder ConfigureLogs(this IApplicationBuilder app)
    {
        CreateDefaultDirectories();
        
        // Logs
        // Enable directory browser logging visualization just by typing 
        // "/logs"
        app.UseDirectoryBrowser(new DirectoryBrowserOptions(){
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logs")), 
            RequestPath = new PathString("/logs"), 
        });

        // Enable the visualization of logs files
        app.UseStaticFiles(new StaticFileOptions{
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logs")), 
            RequestPath = new PathString("/logs"), ServeUnknownFileTypes = true, DefaultContentType = "text/plain"
        });

        return app;
    }
    
    public static void CreateDefaultDirectories()
    {
        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logs"));
    }
}