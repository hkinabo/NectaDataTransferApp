using NectaDataTransfer.Shared.Models.Huduma;

namespace NectaDataTransfer.Shared.Interfaces.Huduma
{
    public interface INameService
    {
        Task<List<NameModel>> GetNames(string _username);
        Task<int> DeleteAllNames<T>();
        Task<List<NameModel>> GetAllSqliteNames(string _username);
        Task<int> AddNames(NameModel nameM);
        Task<int> UpdateNames(NameModel nameM);
        Task<List<HudumaSqlNameModel>> GetAllSqliteSqlNames(string _username);
        Task<List<HudumaSifaSqlNameModel>> GetAllSqliteSifaSqlNames(string _username);

        Task<List<NameModel>> GetAllSqliteNamesAndSql(string _username);
        Task<List<NameModel>> GetAllSqliteNamesAndSqlAndSifa(string _username);
        Task<int> DeleteNamesOfUser(string _username);
        Task<int> UpdateApplicationNames(string _username, int appid);
    }
}
