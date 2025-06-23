
using NectaDataTransfer.Shared.Models.Sifa;



namespace NectaDataTransfer.Shared.Responses
{
    public class ReturnDatabaseListResponse : BaseResponse
    {
        public List<SifaSqlDatabaseModel>? SifaSqlDBModelList { get; set; }
        public List<MysqlCentreModel>? MysqlCentreModelList { get; set; }
    }
}
