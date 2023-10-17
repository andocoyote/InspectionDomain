using Azure.Identity;
using CommonFunctionality.AppToken;
using CommonFunctionality.CosmosDbProvider;
using CommonFunctionality.Model;
using CommonFunctionality.StorageAccount;
using DotNetCoreSqlDb.Models;
using InspectionDomain.Configuration;
using InspectionDomain.InspectionDataGatherer;
using InspectionDomain.InspectionDataWriter;
using InspectionDomain.Providers.EstablishmentsProvider;
using InspectionDomain.Providers.EstablishmentsTableProvider;
using InspectionDomain.Providers.ExistingInspectionsTableProvider;
using InspectionDomain.Providers.SQLDatabaseProvider;
using HttpClientTest.HttpHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using InspectionDomain.DomainModels;
using EventStore.Events;
using InspectionDomain.DomainModels.Events;
using EventStore.Domain;
using Microsoft.Azure.Cosmos;

// Creating WebJobs that target .NET
// https://learn.microsoft.com/en-us/azure/app-service/webjobs-sdk-get-started

// Creating loggers from LoggerFactory
// https://stackoverflow.com/questions/55049683/ilogger-injected-via-constructor-for-http-trigger-functions-with-azure-function

namespace InspectionDomain
{
    internal class Program
    {
        static async Task Main()
        {
            HostBuilder builder = new HostBuilder();

            // The ConfigureWebJobs extension method initializes the WebJobs host
            builder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
                b.AddTimers();
            });

            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.AddEnvironmentVariables();

                // Load the appropriate appsettings*.json file, depending on the environment
                string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                configurationBuilder.AddJsonFile($"appsettings.json");
                configurationBuilder.AddJsonFile($"appsettings.{environment}.json");

                var configRoot = configurationBuilder.Build();

                // appsettings*.json contains the Key Vault name, so add the Key Vault to the configuration
                configurationBuilder.AddAzureKeyVault(
                    new Uri($"https://{configRoot["KeyVault:VaultName"]}.vault.azure.net/"),
                    new DefaultAzureCredential());

                var config = configurationBuilder.Build();
            });

            // Configure the Dependency Injection container
            builder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ILoggerFactory, LoggerFactory>();
                services.AddSingleton<IInspectionDataWriter, InspectionDomain.InspectionDataWriter.InspectionDataWriter>();
                services.AddSingleton<ICommonServiceLayerProvider, CommonServiceLayerProvider>();
                services.AddSingleton<ISQLDatabaseProvider, SQLDatabaseProvider>();
                services.AddSingleton<IEstablishmentsTableProvider, Providers.EstablishmentsTableProvider.ExistingInspectionsTableProvider>();
                services.AddSingleton<IInspectionDataGatherer, InspectionDataGatherer.InspectionDataGatherer>();
                services.AddSingleton<IEstablishmentsProvider, EstablishmentsProvider>();
                services.AddSingleton<ICosmosDbProviderFactory<InspectionData>, InspectionDataCosmosDbProviderFactory>();
                services.AddSingleton<IExistingInspectionsTableProvider, Providers.ExistingInspectionsTableProvider.ExistingInspectionsTableProvider>();

                // Event Store dependencies
                // This allows the framework to provide the Cosmos DB Container to the IEventStreamClient
                services.AddSingleton((s) =>
                {
                    CosmosClient cosmosClient = new CosmosClient(
                        hostContext.Configuration["CosmosDb:Endpoint"],
                        hostContext.Configuration["CosmosDb:Keys:ReadWrite"],
                        new CosmosClientOptions()
                        {
                            ApplicationRegion = Regions.WestUS3,
                        });

                    Database db = cosmosClient.GetDatabase("EventStore");
                    return db.GetContainer("TestEvents");
                });
                services.AddSingleton<IInspectionEventFactory, InspectionEventFactory>();
                services.AddSingleton<IEventMapper<InspectionDomainModel>, InspectionEventMapper>();
                services.AddSingleton<IEventStreamClient<InspectionDomainModel>, EventStreamClient<InspectionDomainModel>>();
                services.AddSingleton<IInspectionDomainService, EventBasedInspectionDomainService>();

                services.AddDbContext<FoodInspectorDatabaseContext>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")));

                services.AddLogging();

                AddOptions(services, hostContext.Configuration);
            });

            // The AddConsole method adds console logging to the configuration
            builder.ConfigureLogging((context, b) =>
            {
                b.SetMinimumLevel(LogLevel.Information);
                b.AddConsole();
                b.AddApplicationInsightsWebJobs(o => { o.ConnectionString = context.Configuration.GetConnectionString("AzureWebJobsDashboard"); });
            });

            IHost host = builder.Build();

            using (host)
            {
                await host.RunAsync();
            }
        }

        /// <summary>
        /// Configures IOption instances for dependent service settings.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to configure options for.</param>
        /// <param name="configRoot">The current <see cref="IConfiguration"/> to use when setting options.</param>
        private static void AddOptions(IServiceCollection services, IConfiguration configRoot)
        {
            // Bind each configuration section and add it to the dependency injection service container
            // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0
            services.Configure<CosmosDbOptions>(
                configRoot.GetSection("CosmosDb"));
            services.Configure<KeyVaultOptions>(
                configRoot.GetSection("KeyVault"));
            services.Configure<StorageAccountOptions>(
                configRoot.GetSection("Storage"));
            services.Configure<AppTokenOptions>(
                configRoot.GetSection("AppToken"));
        }
    }
}