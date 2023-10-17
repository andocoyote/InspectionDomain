using EventStore.Events;

namespace InspectionDomain.DomainModels.Events
{
    public class CreateInspection : Event<InspectionDomainModel>
    {
        private const string CreateInspectionEstablishmentName = "CreateInspectionEstablishmentName";

        public string Owner()
        {
            //return this.Data[CreateCarOwnerKey];
            return string.Empty;
        }

        public CreateInspection() : base()
        { }

        public CreateInspection(Event<InspectionDomainModel> genericEvent) :
            base(genericEvent.id, genericEvent.StreamId, genericEvent.EventNumber, genericEvent.Created, nameof(CreateInspection), genericEvent.OriginatingComponent, genericEvent.Version.ToString())
        {
            //this.Data[CreateCarOwnerKey] = genericEvent.Data[CreateCarOwnerKey];
        }

        public CreateInspection(string id, string streamId, long eventNumber, DateTime created, string establishmentName, string originatingComponent, string originatingComponentVersion) :
            base(id, streamId, eventNumber, created, nameof(CreateInspection), originatingComponent, originatingComponentVersion)
        {
            //this.Data[CreateCarOwnerKey] = newOwner;
        }

        public override InspectionDomainModel ApplyEvent(InspectionDomainModel model)
        {
            return new InspectionDomainModel(this.StreamId, false, false, true);
        }
    }
}
