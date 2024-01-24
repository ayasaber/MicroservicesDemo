using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Logging.Services
{
    public static class ElasticsearchLogging
    {
        public static void AddConfiguration(string applicationName)
        {
            Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile($"{Directory.GetParent(Directory.GetCurrentDirectory())}\\Logging\\appsettings.json")
              .Build();

            var node = new Uri(configuration["ElasticConfiguration:Uri"]);

            var elasticOptions = new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                OverwriteTemplate = true,
                IndexFormat = $"App-{applicationName.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                ModifyConnectionSettings = (settings) =>
                {

                    settings.EnableApiVersioningHeader();

                    settings.CertificateFingerprint(configuration["ElasticConfiguration:FingerPrint"]);

                    settings.BasicAuthentication(configuration["ElasticConfiguration:Username"], configuration["ElasticConfiguration:Password"]);
                    settings.DeadTimeout(TimeSpan.FromSeconds(300));

                    return settings;
                }
            };

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .WriteTo.Console()
                .WriteTo.Debug()
                
                 .WriteTo.Elasticsearch(elasticOptions)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

    }
}