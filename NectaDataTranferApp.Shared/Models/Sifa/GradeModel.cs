namespace NectaDataTransfer.Shared.Models.Sifa
{
	public class GradeModel
	{
		public int TypeId { get; set; }
		public string GradeName { get; set; }
		public string Remark { get; set; }
		public int GradeId { get; set; }

		public int IntRank { get; set; }

		public int IsPass { get; set; }
		public int Credit { get; set; }
	}
}
