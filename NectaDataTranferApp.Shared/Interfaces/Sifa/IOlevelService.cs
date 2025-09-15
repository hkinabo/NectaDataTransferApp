using NectaDataTransfer.Shared.Models.Sifa;
using NectaDataTransfer.Shared.Responses;


namespace NectaDataTransfer.Shared.Interfaces.Sifa
{
    public interface IOlevelService
    {
        Task<int> AddParticular(ParticularModel pModel);
        Task<int> UpdateParticular(ParticularModel pModel);
        Task<int> DeleteParticular(ParticularModel pModel);
        Task<int> DeleteAllParticular<T>();
        Task<int> DeleteAllResult<T>();
        Task<int> DeleteAllCentre<T>();
        Task<int> DeleteAllGrade<T>();
        Task<int> DeleteAllQtvalue<T>();
        Task<int> DeleteAllType<T>();
        Task<int> DeleteAllSubject<T>();
        Task<int> AddParticularAll(List<ParticularModel> models);
        Task<int> AddResultAll(List<ResultModel> models);
        Task<int> AddCentreAll(List<CentreModel> models);
        Task<int> AddQtvalueAll(List<QtvalueModel> models);
        Task<int> AddGradeAll(List<GradeModel> models);
        Task<int> AddTypeAll(List<TypeModel> models);
        Task<int> AddSubjectAll(List<SubjectModel> models);
        Task<List<SifaSqlDatabaseModel>> GetSqlDatabases(string _username);
        Task<ReturnDatabaseListResponse> GetSqlDatabases2(string _username);


        Task<List<ParticularModel>> GetParticularByCandno(string candnumber, string _username);
        Task<List<ParticularModel>> GetAllParticular(string _username);
        Task<List<ParticularModel>> GetAllParticularSqlite();

        Task<List<ResultModel>> GetResultByCandno(string candnumber, int etype, string _username);
        Task<List<ResultModel>> GetAllResult(string _username);
        Task<List<CentreModel>> GetAllCentre(string _username);
        Task<List<GradeModel>> GetAllGrade(string _username);
        Task<List<QtvalueModel>> GetAllQtvalue(string _username);
        Task<List<TypeModel>> GetAllType(string _username);
        Task<List<SubjectModel>> GetAllSubject(string _username);
        Task<YearModel> GetYearByName(string _class);
        Task<int> DeleteAllYear<T>(string _username);
        Task<int> AddYear(YearModel yModel);
        Task<List<CentreModel>> GetCentreByCentreno(string centrenumber, string _username);
        Task<YearModel> GetYear(string _username);

        Task<int> UpdateParticularKey();
        Task<int> DeleteAllSifaName<T>();
        Task<List<SifaNameModel>> GetAllSifaName(string _username);
        Task<int> AddSifaNameAll(List<SifaNameModel> models);
        Task<List<SifaNameModel>> GetAllSqliteSifaNames(string _username);
        Task UpdateSifaNameStore(string _etype, string _candnumber, int _mwaka, string _username);
        Task<List<SifaNameModel>> GetAllSifaNameFee(string _username);
        Task<List<ParticularModel>> GetParticularByCandnoFee(string candnumber, string _username, string _emisDB, int _etype);
        Task<List<ResultModel>> GetResultByCandnoFee(string candnumber, int etype, string _username, string _emisDb);
        Task UpdateSifaNameStoreFee(string _etype, string _candnumber, int _mwaka, string _username);
        Task<int> DeleteParticularbyCandno<T>(string candno);
        Task<int> DeleteAllSifaNameBackup<T>();
        Task<int> DeleteAllSifaFeeBackup<T>();
        Task<int> InsertSifaNameBackup(string _etype, string _candnumber, int _mwaka, string _username);
        Task<int> InsertSifaFeeBackup(string _etype, string _candnumber, int _mwaka, string _username);
        Task<List<SifaNameBackupModel>> GetAllSifaNamesBackup(string _username);
        Task<List<SifaFeeBackupModel>> GetAllSifaFeeBackup(string _username);
        Task<List<ParticularModel>> GetParticularByCandnoTefis(string candnumber, string _username, string _eyear);
        Task<List<ResultModel>> GetResultByCandnoTefis(string candnumber, int etype, string _username, string _eyear);
        Task<int> DeleteAllSifaName2<T>();
        Task<int> DeleteAllSifaName3(List<SifaNameModel> list);
    }
}
