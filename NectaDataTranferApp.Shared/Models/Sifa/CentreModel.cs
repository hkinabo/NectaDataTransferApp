using SQLite;

namespace NectaDataTransfer.Shared.Models.Sifa
{
	public class CentreModel
	{
		public string SzExamCentreName { get; set; }
		[PrimaryKey]
		public string SzExamCentreNumber { get; set; }
	}
}
