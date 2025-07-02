using Microsoft.Extensions.Configuration;
using Serilog;

namespace MiniCommerce.Infrastructure.Configurations;

public static class SerilogConfiguration
{
    public static void ConfigureSerilogAsLogger(IConfiguration configuration)
    {
        var serilogSection = configuration.GetSection("Loggers");
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(serilogSection)
            .Enrich.FromLogContext()
            .Enrich.WithCorrelationId()
            .CreateLogger();
    }
}