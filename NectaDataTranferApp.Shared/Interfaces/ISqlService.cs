
using NectaDataTransfer.Shared.Models;

using System.Data;

namespace NectaDataTransfer.Shared.Interfaces
{
	public interface ISqlService
	{
		Task<List<SqlModel>> GetAllSql();
		Task<int> AddSql(SqlModel sqlModel);
		Task<int> AddTransferLog(TransferLogModel tlog);
		Task<int> UpdateSql(SqlModel sqlModel);
		Task<int> DeleteSql(SqlModel sqlModel);
		Task<List<SqlDatabaseModel>> GetSqlDatabases(string _username);
		Task<List<TransferLogModel>> GetTransferLog(string tranferoption, int cid);
		// Task<bool> AddTransferLog(TransferLogModel tlog);
		Task<bool> RegistrationInsert(Dictionary<string, string> args, string _username);
		Task<bool> BulkInsert(DataTable dt, string _username);
		Task<bool> BulkInsertSubject(DataTable dt, string _username);
		Task<bool> BulkInsertSifa(DataTable dt, string _username);
		Task<bool> BulkInsertSchool(DataTable dt, string _username);
		Task<bool> UpdateParticularNames(string candno, int etype, string Fname, string Oname, string Sname, string Sex, string _username);
		//Task<int> AddHudumaSqlNames(HudumaSqlNameModel _hudumaName);
		//Task<int> DeleteHudumaSqlName(HudumaSqlNameModel _hudumasqlmodel);

		//Task<List<HudumaSqlNameModel>> GetAllHudumaSqlName();

		//Task<int> AddHudumaSqlNamesAll(List<HudumaSqlNameModel> models);
		//Task<List<HudumaSqlNameModel>> GetSqlNames(string candno, int etype, string _eyear, string _emisdb, int _nameid, string _username);
		//Task<List<HudumaSifaSqlNameModel>> GetSearchName(string searchname, string _etypename, int etype, string _eyear, string _emisdb, int _nameid, string _username);
		Task<int> DeleteSearchSifaSqlNames<T>();
		//Task<int> AddSearch(HudumaSifaSqlNameModel sifasqlnameModel);
		//Task<int> AddSearchList(List<HudumaSifaSqlNameModel> models);
		Task<bool> UpdateParticularNamesSifa(string candno, int etype, string Fname, string Oname, string Sname, string Sex, string _username);
		Task<int> DeleteSearchSifaSqlByNameID(int _nameId);
		Task<int> DeleteHudumaSqlNameByUsername(string _username);
        Task<bool> BulkInsertSubjectCA(DataTable dt, string _username);
    }

}