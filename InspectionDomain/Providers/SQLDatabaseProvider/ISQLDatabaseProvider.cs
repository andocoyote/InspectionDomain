using CommonFunctionality.Model;

namespace InspectionDomain.Providers.SQLDatabaseProvider
{
    public interface ISQLDatabaseProvider
    {
        public string ConnectionString { get; set; }

        void WriteRecord(InspectionData inspectionData);
    }
}