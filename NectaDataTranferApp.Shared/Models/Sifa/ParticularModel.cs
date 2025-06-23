using SQLite;

namespace NectaDataTransfer.Shared.Models.Sifa
{
	public class ParticularModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string SzExamCentreNumber { get; set; } = null!;
		public string SzCandidatesNumber { get; set; } = null!;

		public string? Premno { get; set; }
		public int Etype { get; set; }

		public string? Fname { get; set; }
		public string? Oname { get; set; }
		public string? Sname { get; set; }
		public string? Sex { get; set; }

		public string? Ctype { get; set; }
		public string? Dbirth { get; set; }

		public double Point { get; set; }

		public string? Division { get; set; }
		public int Status { get; set; }

		public int Nrank { get; set; }

	}
}
