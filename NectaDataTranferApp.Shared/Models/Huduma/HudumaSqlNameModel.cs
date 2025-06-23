using SQLite;

namespace NectaDataTransfer.Shared.Models.Huduma
{
	public class HudumaSqlNameModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Fname { get; set; }
		public string Oname { get; set; }

		public string Sname { get; set; }
		public string Sex { get; set; }
		public int ExamType { get; set; }
		public string ExamName { get; set; }
		public string ExamNumber { get; set; }
		public string ExamYear { get; set; }
		public string EmisDB { get; set; }
		public string SifaTable { get; set; }
		public string UserName { get; set; }

		public int NameId { get; set; }

	}
}
