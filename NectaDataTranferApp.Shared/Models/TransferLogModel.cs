using SQLite;

namespace NectaDataTransfer.Shared.Models
{
	public class TransferLogModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string TransferOption { get; set; }

		public int ClassId { get; set; }

		public int TotalRecord { get; set; }

		public string UserName { get; set; }
		public DateTime TransferDate { get; set; }

	}
}
