using EventStore.Events;
using InspectionDomain.DomainModels.Events;

namespace InspectionDomain.DomainModels
{
    public class InspectionEventMapper : IEventMapper<InspectionDomainModel>
    {
        private IInspectionEventFactory carEventFactory;

        public InspectionEventMapper(IInspectionEventFactory carEventFactory)
        {
            this.carEventFactory = carEventFactory;
        }

        public Event<InspectionDomainModel> MapToEventImplementation(Event<InspectionDomainModel> genericEvent)
        {
            switch (genericEvent.EventType)
            {
                case nameof(CreateInspection):
                    return this.carEventFactory.BuildCreateInspection(genericEvent);
                default:
                    throw new InvalidOperationException("Event type not found: " + genericEvent.EventType);
            }
        }
    }
}
