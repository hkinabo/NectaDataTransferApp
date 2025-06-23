using SQLite;

namespace NectaDataTransfer.Shared.Models
{
    public class CsvModel

    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }

        public string DistrictCode { get; set; }

        public string DistrictName { get; set; }

        public string SchoolCode { get; set; }

        public string CandidateNumber { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public string Name3 { get; set; }

        public string Sex { get; set; }

        public string BirthDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string PhoneNumber { get; set; }

        public int Vision { get; set; }

        public string PremNumber { get; set; }
        public string ReferenceNumber { get; set; }

        public string PsleYear  { get; set; }
        public string PsleNumber  { get; set; }
        public string JamiiNumber { get; set; }
        public string BirthCertificateNumber{ get; set; }
        public int IsRepeater{ get; set; }
        public string AdmissionNumber{ get; set; }
        public string AdmissionDate{ get; set; }
        public int TradeId { get; set; }
        public string Nationality { get; set; }

        public int ClassId { get; set; }

        public int CandidateId { get; set; }

        public int InUsajili { get; set; } = 0;
        public string Username { get; set; }

    }
}

