using NectaDataTransfer.Shared.Models.Sifa;

namespace NectaDataTransfer.Shared.Interfaces.Sifa
{
	public interface ISifaConnectionService
	{
		Task<List<SifaConnectionModel>> GetAllConnection();
		Task<SifaConnectionModel> GetConnectionById(int id);

		Task<int> AddConnection(SifaConnectionModel connectionModel);
		Task<int> UpdateConnection(SifaConnectionModel connectionModel);
		Task<int> DeleteConnection(SifaConnectionModel connectionModel);
		Task<List<SifaConnectionModel>> GetConnectionByNameUsername(string name, string userName);
	}
}
