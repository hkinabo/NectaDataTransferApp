using NectaDataTransfer.Shared.Models.Sifa;



namespace NectaDataTransfer.Shared.Responses
{
    public class ReturnDatabaseResponse : BaseResponse
    {
        public SifaSqlDatabaseModel? SifaSqlDBModel { get; set; } 
    }
}
