using SQLite;
using System.ComponentModel.DataAnnotations;

namespace NectaDataTransfer.Shared.Models
{
	public class MysqlModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[Required]

		public string Host { get; set; }

		[Required]
		public int Port { get; set; }

		//[Required]
		//[StringLength(30, ErrorMessage = "Username must be at least 2 characters long.", MinimumLength = 2)]
		public string Username { get; set; }
		//[Required]
		//[StringLength(30, ErrorMessage = "Password must be at least 4 characters long.", MinimumLength = 4)]
		public string Password { get; set; }

		public string SourceDatabase { get; set; }

	}
}
