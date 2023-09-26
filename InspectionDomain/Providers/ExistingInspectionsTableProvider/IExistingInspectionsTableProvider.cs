using InspectionDomain.Providers.ExistingInspectionsTableProvider;

namespace InspectionDomain.Providers.ExistingInspectionsTableProvider
{
    public interface IExistingInspectionsTableProvider
    {
        public Task AddInspectionRecord(string inspectionSerialNum, string inspectionId);
        public Task<List<ExistingInspectionModel>> QueryInspectionRecord(string inspectionSerialNum, string inspectionId);
    }
}