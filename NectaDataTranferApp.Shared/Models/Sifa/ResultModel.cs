namespace NectaDataTransfer.Shared.Models.Sifa
{
	public class ResultModel
	{
		public int Etype { get; set; }
		public string SzCandidatesNumber { get; set; }

		public string SubjectCode { get; set; }
		public string? Grade { get; set; } = null;
		public int? Score { get; set; } = null;
	}
}
