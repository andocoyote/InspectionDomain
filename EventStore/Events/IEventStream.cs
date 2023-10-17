using EventStore.Domain;

namespace EventStore.Events
{
    public interface IEventStream<DomainModel> where DomainModel : IDomainModel
    {
        Task AppendEvent<EventType>(EventType newEvent) where EventType : Event<DomainModel>;
        DomainModel ProjectModel();
        Guid GetStreamId();
        long NextEventNumber();
        Version CurrentVersion();
    }
}
