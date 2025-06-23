using SQLite;

namespace NectaDataTransfer.Shared.Models
{
	public class TransferOptionModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string TransferOption { get; set; } // 1 for Bulk insert, 2 for update
		public string TransferOptionName { get; set; }
		public string Username { get; set; }
		public DateTime TrasferDate { get; set; }
	}
}


