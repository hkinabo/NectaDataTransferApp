


using NectaDataTransfer.Shared.Models;
using NectaDataTransfer.Shared.Models.CA;

namespace NectaDataTransfer.Shared.Interfaces
{
    public interface ICsvService
    {
        Task<List<CsvModel>> GetAllCsv();
        Task<List<SchoolModel>> GetAllSchool();
        Task<CsvModel> GetCsvById(int id);
        Task<CsvModel> GetClassIdFromCsv();
        Task<ClassModel> GetClassById(int id);
        Task<ClassModel> GetClassByName(string classname);

        Task<int> UpdateCsvInUsajili(int inusajili, string _regno);

        Task<int> AddCsv(CsvModel csvModel);
        Task<int> AddClass(ClassModel classModel);
        Task<int> AddSchool(SchoolModel schoolModel);
        Task<int> DeleteSchool(SchoolModel schoolModel);

        Task<int> AddCsvAll(List<CsvModel> models);
        Task<int> UpdateCsv(CsvModel csvModel);
        Task<int> UpdateSchool(SchoolModel schoolModel);
        Task<int> DeleteCsv(CsvModel csvModel);
        Task<int> DeleteCsvByClass(CsvModel csvModel, int classid);

        Task<int> DeleteAllCsv<T>();
        Task<int> DeleteAllCsvSubject<T>();
        Task<int> DeleteAllSchool<T>();
        Task<int> DeleteAllClass<T>();

        Task<List<CsvModel>> GetRegionsFromCsv();
        Task<List<CsvModel>> GetRegionsFromCsvDesti(int _inusajili);

        Task<List<CsvModel>> GetCsvByRegion(int classid, string regno);
        Task<List<CsvModel>> GetCsvByRegionDestination(int classid, string regioncode, int _inUsajili);
        Task<List<CsvModel>> GetCsvBySchool(int classid, string schoolno);
        Task<List<RegionModel>> GetRegions(string _username);
        Task<List<SchoolModel>> GetSchools(string _username);
        Task<List<SchoolModel>> GetSchoolByCode(string schoolcode, string _username);
        Task<List<ClassModel>> GetClasses(string _username);
        Task<int> SchoolUpdateCsvInUsajili(int inusajili, string _schoolno);
        Task<List<CsvModel>> GetAllSchoolDesti(int _inUsajili);
        //Task<ErrorModel[]> GetErrorReport();
        //Task<List<ErrorModel>> GetEReport();
        Task<List<CsvModel>> GetCsvDataMysql(int _classid, string _schoolno, string _regno, int querryid, string _username);
        Task<List<CsvModelSubject>> GetCsvDataMysqlSubject(int _classid, string _schoolno, string _regionno, int querryid, string _username);
        Task<int> AddCsvAllSubject(List<CsvModelSubject> models);
        Task<int> AddCsvAllSifa(List<SifaExamModel> models);
        Task<List<CsvModel>> GetAllSchoolDestiRegion(string regioncode);
        Task<List<SifaExamModel>> GetCsvDataMysqlSifa(int _classid, string _schoolno, string _regionno, int querryid, string _username);
        Task<int> DeleteAllCsvSchool<T>();
        Task<List<CsvModelSchool>> GetCsvDataMysqlSchool(int _classid, string _schoolno, string _regionno, int querryid, string _username);
        Task<int> AddCsvAllSchool(List<CsvModelSchool> models);
        Task<List<CsvModelSchool>> GetCsvSchoolByRegion(int classid, string regioncode);
        Task<List<CsvModelSchool>> GetCsvSchoolByRegionDestination(int classid, string regioncode);
        Task<List<SifaExamModel>> GetCsvSifaByRegionDestination(int classid, string regioncode);
        Task<List<CsvModelSubject>> GetCsvSubjectByRegionDestination(int classid, string regioncode);
        Task<int> DeleteAllCsvSifa<T>();
        Task<int> UpdateClassId(int _classid, string _username);
        Task<ClassModel> GetClassIdFromClassModel();
        Task<int> UpdateCandidateNumber(int _startnumber, string _username);
        Task<int> UpdateCandNumberSifaTable();
        Task<int> UpdateCandNumberSubjectTable();
        Task<List<RegStatusModel>> GetRegistrationStatus(int _classid, string _username);
        Task<List<CsvModelSchool>> GetCsvSchoolBySchoolDestination(int classid, string schoolcode);
        Task<int> DeleteAllCsvSubjectCA<T>();
        Task<int> AddCsvAllSubjectCA(List<CAModelSubject> models);
        Task<List<CAModelSubject>> GetCsvSubjectByRegionDestinationCA(int classid, string regioncode);
        Task<List<CAModelSubject>> GetCsvDataMysqlSubjectCA(int _classid, string _schoolno, string _regionno, int querryid, string _username);
        Task<List<SifaExamModel>> GetCsvSifaBySchoolDestination(int classid, string schoolcode);
        Task<List<CsvModelSubject>> GetCsvSubjectBySchoolDestination(int classid, string schoolcode);
        Task<List<CAModelSubject>> GetCsvSubjectBySchoolDestinationCA(int classid, string schoolcode);
    }
}