using CommonFunctionality.CosmosDbProvider;
using CommonFunctionality.Model;
using EventStore.CosmosDb;
using EventStore.Domain;
using InspectionDomain.InspectionDataWriter;
using InspectionDomain.Providers.EstablishmentsProvider;
using InspectionDomain.Providers.EstablishmentsTableProvider;
using InspectionDomain.Providers.ExistingInspectionsTableProvider;
using InspectionDomain.Providers.SQLDatabaseProvider;
using HttpClientTest.HttpHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InspectionDomain.DependencyInjection
{
    public static class ContainerBuilder
    {
        public static void ConfigureServices(IServiceCollection serviceCollection, IConfiguration config)
        {
            serviceCollection.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration, Microsoft.Extensions.Configuration.ConfigurationManager>();
            serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceCollection.AddSingleton<IInspectionDataWriter, InspectionDataWriter.InspectionDataWriter>();
            serviceCollection.AddSingleton<ICommonServiceLayerProvider, CommonServiceLayerProvider>();
            serviceCollection.AddSingleton<ISQLDatabaseProvider, SQLDatabaseProvider>();
            serviceCollection.AddSingleton<IEstablishmentsTableProvider, Providers.EstablishmentsTableProvider.ExistingInspectionsTableProvider>();
            serviceCollection.AddSingleton<IEstablishmentsProvider, EstablishmentsProvider>();
            serviceCollection.AddSingleton<ICosmosDbProviderFactory<InspectionData>, InspectionDataCosmosDbProviderFactory>();
            serviceCollection.AddSingleton<IExistingInspectionsTableProvider, Providers.ExistingInspectionsTableProvider.ExistingInspectionsTableProvider>();
            serviceCollection.AddSingleton<ICosmosClient<TestEvent>, CosmosClient<TestEvent>>();
            serviceCollection.AddLogging();

            serviceCollection.AddOptions<EventStore.CosmosDb.CosmosClientOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    config.GetSection("EventStore").Bind(settings);
                });
        }
    }
}
