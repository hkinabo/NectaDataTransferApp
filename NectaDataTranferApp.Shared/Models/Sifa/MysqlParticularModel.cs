namespace NectaDataTransfer.Shared.Models.Sifa
{
    public class MysqlParticularModel
    {
        public int Id { get; set; }
        public int tbl_exam_types_Id { get; set; }
        public string SzCandidatesNumber { get; set; } = null!;
        public string ExamYear { get; set; } = null!;
    }
}
