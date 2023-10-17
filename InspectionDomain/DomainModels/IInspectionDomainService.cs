namespace InspectionDomain.DomainModels
{
    public interface IInspectionDomainService
    {
        Task<InspectionDomainModel> CreateInspection(string establishmentName, string originatingComponent, string originatingComponentVersion);
    }
}
