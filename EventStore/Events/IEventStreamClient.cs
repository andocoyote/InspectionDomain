using EventStore.Domain;

namespace EventStore.Events
{
    public interface IEventStreamClient<DomainModel> where DomainModel : IDomainModel
    {
        Task<IEventStream<DomainModel>> CreateEventStream();
        Task<IEventStream<DomainModel>> GetEventStream(Guid streamId);
    }
}
