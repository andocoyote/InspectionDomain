namespace InspectionDomain.Providers.EstablishmentsProvider
{
    public interface IEstablishmentsProvider
    {
        List<EstablishmentsModel> ReadEstablishmentsFile();
    }
}