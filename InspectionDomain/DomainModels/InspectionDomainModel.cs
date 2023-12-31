﻿using EventStore.Domain;

namespace InspectionDomain.DomainModels
{
    public class InspectionDomainModel : IDomainModel
    {
        public string StreamId { get; init; }
        public bool IsFailed { get; }
        public bool IsCancelled { get; }
        public bool IsSucceeded { get; }

        public InspectionDomainModel() { }

        public InspectionDomainModel(string streamId, bool isFailed, bool isCancelled, bool isSucceeded)
        {
            StreamId = streamId;
            IsFailed = isFailed;
            IsCancelled = isCancelled;
            IsSucceeded = isSucceeded;
        }
    }
}
