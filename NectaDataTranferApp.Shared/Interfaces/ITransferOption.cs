using NectaDataTransfer.Shared.Models;

namespace NectaDataTransfer.Shared.Interfaces
{
	public interface ITransferOption
	{

        Task<List<TransferOptionModel>> GetAllOption();
        Task<TransferOptionModel> GetOptionById(int id);

		Task<TransferOptionModel> GetOptionByUsername(string toption);
		Task<int> AddOption(TransferOptionModel TransferOptionModel);

       
        Task<int> UpdateOption(TransferOptionModel TransferOptionModel);
		Task<int> DeleteOption(TransferOptionModel TransferOptionModel);

		
        Task<int> DeleteAllOption<T>(string _username);
        Task<List<TransferOptionModel>> GetAllOption(string _username);
    }
}
