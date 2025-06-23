namespace NectaDataTransfer.Shared.Models
{
    public class SifaExamModel
    {
        public string RegionCode { get; set; }
        public string SchoolCode { get; set; }
        public string CandidateNumber { get; set; }
        public string SifaNumber { get; set; }
        public string SifaType { get; set; }
        public int SifaYear { get; set; }
        public int ClassId { get; set; }
        public int CandidateId { get; set; }
    }
}
