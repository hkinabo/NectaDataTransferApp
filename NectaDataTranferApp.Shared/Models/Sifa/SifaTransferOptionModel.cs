using SQLite;

namespace NectaDataTransfer.Shared.Models.Sifa
{
    public class SifaTransferOptionModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string TransferOption { get; set; } // 1 for Bulk insert, 2 for update
        public string TransferOptionName { get; set; } // First Insert, Update,Names Insert
        public string Username { get; set; }
    }
}
