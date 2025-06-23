using SQLite;

namespace NectaDataTransfer.Shared.Models.Sifa
{
	public class MysqlCentreModel
	{
		public int Id { get; set; }
		[PrimaryKey]
		public string SzExamCentreNumber { get; set; }
	}
}
