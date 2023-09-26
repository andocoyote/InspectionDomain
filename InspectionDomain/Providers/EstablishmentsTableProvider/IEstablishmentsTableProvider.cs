using InspectionDomain.Providers.EstablishmentsProvider;

namespace InspectionDomain.Providers.EstablishmentsTableProvider
{
    public interface IEstablishmentsTableProvider
    {
        Task CreateEstablishmentsSet();

        Task<List<EstablishmentsModel>> GetEstablishmentsSet();
    }
}