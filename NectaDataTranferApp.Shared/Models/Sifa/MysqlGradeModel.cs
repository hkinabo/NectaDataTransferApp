namespace NectaDataTransfer.Shared.Models.Sifa
{
    public class MysqlGradeModel
    {
        public int Id { get; set; }
        public int tbl_exam_types_Id { get; set; }
        public string szGrade { get; set; }
        public string ExamYear { get; set; } = string.Empty;
    }
}
