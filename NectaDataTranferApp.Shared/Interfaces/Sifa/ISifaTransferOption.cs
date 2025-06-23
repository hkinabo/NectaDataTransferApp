using NectaDataTransfer.Shared.Models.Sifa;

namespace NectaDataTransfer.Shared.Interfaces.Sifa
{
    public interface ISifaTransferOption
    {
        Task<List<SifaTransferOptionModel>> GetAllOption(string _username);
        Task<SifaTransferOptionModel> GetOptionById(int id);

        Task<SifaTransferOptionModel> GetOptionByUsername(string _username);
        Task<int> AddOption(SifaTransferOptionModel TransferOptionModel);
        Task<int> UpdateOption(SifaTransferOptionModel TransferOptionModel);
        Task<int> DeleteOption(SifaTransferOptionModel TransferOptionModel);

        Task<int> DeleteAllOption<T>(string _username);


    }
}
