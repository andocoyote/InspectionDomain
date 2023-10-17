using EventStore.Events;

namespace InspectionDomain.DomainModels.Events
{
    public class InspectionEventFactory : IInspectionEventFactory
    {
        public CreateInspection BuildCreateInspection(Event<InspectionDomainModel> genericEvent)
        {
            return new CreateInspection(genericEvent);
        }

        public CreateInspection BuildCreateInspection(IEventStream<InspectionDomainModel> eventStream, string establishmentName, string originatingComponent, string originatingComponentVersion)
        {
            return new CreateInspection(Guid.NewGuid().ToString(), eventStream.GetStreamId().ToString(), eventStream.NextEventNumber(), DateTime.UtcNow, establishmentName, originatingComponent, originatingComponentVersion);
        }
    }
}
