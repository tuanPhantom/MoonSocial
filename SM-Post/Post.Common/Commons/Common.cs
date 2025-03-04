using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Post.Common.Commons;

public static partial class Common
{
    /// <summary>
    /// ConfigureLogging
    /// </summary>
    public static void ConfigureLogging(WebApplicationBuilder builder, string source = null)
    {
        // var hostName = Dns.GetHostName();
        // var isController = Matching.FromSource(source);
        // var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        // var configuration = new ConfigurationBuilder()
        //     .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
        //     .AddJsonFile(
        //         $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
        //         optional: true)
        //     .Build();
        //
        // // postgres
        // Log.Logger = new LoggerConfiguration()
        //     .MinimumLevel.Debug()
        //     .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
        //     .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        //     .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        //     .Enrich.WithProperty("Environment", environment)
        //     .Enrich.WithProperty("ServerName", hostName)
        //     .Enrich.WithProperty("IP", Dns.GetHostEntry(hostName))
        //     .Filter.ByIncludingOnly(isController)
        //     .ReadFrom.Configuration(configuration)
        //     .CreateLogger();

        // Define the sink options
        var sinkOptions = new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true };

        // Define column options if needed
        var columnOptions = new ColumnOptions
        {
            // Customize column options as required
        };

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console() // Log to console  
            .WriteTo.MSSqlServer(connectionString: builder.Configuration.GetConnectionString("DefaultConnection"), sinkOptions: sinkOptions, columnOptions: columnOptions)
            .CreateLogger(); // Use Serilog as the logging provider builder.Host.UseSerilog();
    }
}