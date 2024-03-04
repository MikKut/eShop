using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System;
using Serilog;
using Serilog.Events;
using IdentityServer;
using Microsoft.AspNetCore.Hosting;
using System.Net.Sockets;
using System.Net.Http;

public class Program
{
    public static int Main(string[] args)
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
            .CreateLogger();

        try
        {
            Log.Information("Starting host...");
            CreateHostBuilder(args).Build().Run();
            return 0;
        }
        catch (HttpRequestException httpEx)
        {
            Log.Error(httpEx, "Error connecting to server.");
            return 1;
        }
        catch (SocketException socketEx)
        {
            Log.Error(socketEx, "Connection refused.");
            return 1;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly.");
            return 1;
        }
        finally
        {
            Log.Information("Finally ok.");
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
