using SQLite;
using SQLiteNetExtensions.Attributes;

namespace NectaDataTransfer.Shared.Models.Huduma
{
	public class NameModel
	{
		[PrimaryKey, AutoIncrement]
		public int NameId { get; set; }
		public int ApplicationId { get; set; }
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

		[OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]      // One to many relationship with Valuation
		public IList<HudumaSqlNameModel> SqlNames { get; set; }
		[OneToMany]
		public IList<HudumaSifaSqlNameModel> SifaSqlNames { get; set; }

	}
}
