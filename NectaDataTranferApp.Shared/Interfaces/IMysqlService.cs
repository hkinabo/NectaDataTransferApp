using NectaDataTransfer.Shared.Models;

namespace NectaDataTransfer.Shared.Interfaces
{

    public interface IMysqlService
    {
        Task<List<MysqlModel>> GetAllMysql();
        Task<MysqlModel> GetMysqlById(int id);

        Task<MysqlModel> GetMysqlByUsername(string uname);
        Task<int> AddMysql(MysqlModel mysqlModel);
        Task<int> UpdateMysql(MysqlModel mysqlModel);
        Task<int> DeleteMysql(MysqlModel mysqlModel);
        Task<List<MysqlModel>> GetAllMysqlADO(string _username);
        Task<List<MysqlDatabaseModel>> GetMysqlDatabases(string _username);

    }
}
