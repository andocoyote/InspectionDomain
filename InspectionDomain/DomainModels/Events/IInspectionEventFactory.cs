using EventStore.Events;

namespace InspectionDomain.DomainModels.Events
{
    public interface IInspectionEventFactory
    {
        CreateInspection BuildCreateInspection(Event<InspectionDomainModel> genericEvent);
        CreateInspection BuildCreateInspection(IEventStream<InspectionDomainModel> eventStream, string establishmentName, string originatingComponent, string originatingComponentVersion);
    }
}
