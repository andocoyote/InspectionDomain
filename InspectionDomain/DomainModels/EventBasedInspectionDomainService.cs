using EventStore.Events;
using InspectionDomain.DomainModels.Events;

namespace InspectionDomain.DomainModels
{
    public class EventBasedInspectionDomainService : IInspectionDomainService
    {
        private IEventStreamClient<InspectionDomainModel> streamClient;
        private IInspectionEventFactory inspectionEventFactory;

        public EventBasedInspectionDomainService(IEventStreamClient<InspectionDomainModel> streamClient, IInspectionEventFactory inspectionEventFactory)
        {
            this.streamClient = streamClient;
            this.inspectionEventFactory = inspectionEventFactory;
        }

        public async Task<InspectionDomainModel> CreateInspection(string establishmentName, string originatingComponent, string originatingComponentVersion)
        {
            var newStream = await this.streamClient.CreateEventStream();
            CreateInspection createInspectionEvent = this.inspectionEventFactory.BuildCreateInspection(newStream, establishmentName, originatingComponent, originatingComponentVersion);
            await newStream.AppendEvent(createInspectionEvent);
            return newStream.ProjectModel();
        }
    }
}
