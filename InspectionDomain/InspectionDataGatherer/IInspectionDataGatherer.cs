using CommonFunctionality.Model;

namespace InspectionDomain.InspectionDataGatherer
{
    public interface IInspectionDataGatherer
    {
        Task<List<InspectionData>> GatherData();
    }
}