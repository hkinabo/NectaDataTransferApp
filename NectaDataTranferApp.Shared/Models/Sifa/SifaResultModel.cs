namespace NectaDataTransfer.Shared.Models.Sifa
{
    public class SifaResultModel
    {
        public int SubjectId { get; set; }
        public long ParticualrId { get; set; }
        public int GradeId { get; set; }

        public int Status { get; set; }
        public string SzCandidatesNumber { get; set; }
        public int Etype { get; set; }
        public int Score { get; set; }

        public string ExamYear { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
    }
}
