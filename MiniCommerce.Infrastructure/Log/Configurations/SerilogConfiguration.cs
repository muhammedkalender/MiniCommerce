using Microsoft.Extensions.Configuration;
using Serilog;

namespace MiniCommerce.Infrastructure.Log.Configurations;

public static class SerilogConfiguration
{
    public static void ConfigureSerilogAsLogger(IConfiguration configuration)
    {
        var serilogSection = configuration.GetSection("Loggers");

        Serilog.Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(serilogSection)
            .Enrich.FromLogContext()
            .CreateLogger();
    }
}