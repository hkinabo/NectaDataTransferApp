namespace NectaDataTransfer.Shared.Models.Sifa
{
    public class MysqlSubjectModel
    {
        public int Id { get; set; }
        public int tbl_exam_types_Id { get; set; }
        public string szCodeNumber { get; set; }
        public string ExamYear { get; set; } = string.Empty;


    }
}
