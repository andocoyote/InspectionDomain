using EventStore.Domain;

namespace EventStore.Events
{
    public interface IEventMapper<DomainModel> where DomainModel : IDomainModel
    {
        Event<DomainModel> MapToEventImplementation(Event<DomainModel> genericEvent);
    }
}
