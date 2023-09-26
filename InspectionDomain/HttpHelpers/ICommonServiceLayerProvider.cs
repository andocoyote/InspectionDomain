﻿using CommonFunctionality.Model;
using InspectionDomain.Providers.EstablishmentsProvider;

namespace HttpClientTest.HttpHelpers
{
    public interface ICommonServiceLayerProvider
    {
        Task<List<InspectionData>> GetInspections(List<EstablishmentsModel> establishmentsModels);
        Task<List<InspectionData>> GetInspections(string name, string city, string date);
    }
}