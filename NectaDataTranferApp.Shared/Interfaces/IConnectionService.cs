using NectaDataTranferApp.Shared.Responses;
using NectaDataTransfer.Shared.Models;

namespace NectaDataTransfer.Shared.Interfaces
{
	public interface IConnectionService
	{
		Task<List<ConnectionModel>> GetAllConnection();
		Task<ConnectionModel> GetConnectionById(int id);

		Task<int> AddConnection(ConnectionModel connectionModel);
		Task<int> UpdateConnection(ConnectionModel connectionModel);
		Task<int> DeleteConnection(ConnectionModel connectionModel);
		Task<List<ConnectionModel>> GetConnectionByNameUsername(string name, string userName);
		Task<ReturnBooleanResponse> OpenConnMysql(string pconn);
		Task<ReturnBooleanResponse> OpenConnSql(string pconn);
		
    }
}
