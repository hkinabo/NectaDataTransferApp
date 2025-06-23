namespace NectaDataTransfer.Shared.Models
{
	public class CsvModelSchool

	{
		public string RegionCode { get; set; }
		public string DistrictCode { get; set; }
		public string SchoolCode { get; set; }
		public string SchoolName { get; set; }

		public int IsEnglishMedium { get; set; }

		public int SchoolOwner { get; set; }
		public int DistanceFromDistrict { get; set; }
		public string NambaWizara { get; set; }
		public int ClassId { get; set; }

	}
}

