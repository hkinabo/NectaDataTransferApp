
using SQLite;

namespace NectaDataTransfer.Shared.Models
{
	public class ConnectionModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? ConnectionString { get; set; }
		public string? Username { get; set; }
		public string? Pwd { get; set; }
		public string? Host { get; set; }
		public int Port { get; set; }
	}
}
