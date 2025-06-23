
using SQLite;

namespace NectaDataTransfer.Shared.Models
{
	public class SchoolModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string SchoolCode { get; set; }
		public string SchoolName { get; set; }
		public string Username { get; set; }
	}
}
