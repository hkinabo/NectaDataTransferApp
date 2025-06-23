using NectaDataTranferApp.Shared.Responses;
using NectaDataTransfer.Shared.Models.Sifa;
using NectaDataTransfer.Shared.Responses;

namespace NectaDataTransfer.Shared.Interfaces.Sifa
{

    public interface ISifaService
    {
        Task<int> AddMysqlCentreAll(List<MysqlCentreModel> models);
        Task<int> AddMysqlGradeAll(List<MysqlGradeModel> models);
        Task<int> AddMysqlParticularAll(List<MysqlParticularModel> models);
        Task<int> AddMysqlSubjectAll(List<MysqlSubjectModel> models);
        Task<int> DeleteAllMysqlCentre<T>();
        Task<int> DeleteAllMysqlGrade<T>();
        Task<int> DeleteAllMysqlParticular<T>();
        Task<int> DeleteAllMysqlSubject<T>();
        Task<List<MysqlCentreModel>> GetAllMysqlCentre(string eyear, string _username);
        Task<List<MysqlGradeModel>> GetAllMysqlGrade(string eyear, string _username);

        Task<List<MysqlParticularModel>> GetAllMysqlParticular(string eyear, int etype, string _username);
        Task<List<MysqlSubjectModel>> GetAllMysqlSubject(string eyear, string _username);
        Task<List<CentreModel>> GetCentreSqlite();
        Task<List<ParticularModel>> GetParticularSqlite();
        Task<List<SifaMysqlDatabaseModel>> GetMysqlDatabases(string _username);
        Task<bool> InsertCentreNotExist(string eyear, string centreno, string centrename, string _username);

        Task<List<SifaParticularModel>> GetParticularSqlitePlusCentreId();

        Task<ReturnBooleanResponse> InsertParticularNotExist(string eyear, string candno, int etype, int CentreId, string Premno, string Ctype, string Fname, string Oname, string Sname, string Sex, string Dbirth, double Point, string Division, int Status, int Nrank, string _username);
        Task<List<MysqlParticularModel>> GetAllMysqlParticularCand(string eyear, int etype, string candno, string _username);
        Task<ReturnBooleanResponse> UpdateParticular(string eyear, string candno, int etype, int CentreId, string Premno, string Ctype, string Fname, string Oname, string Sname, string Sex, string Dbirth, double Point, string Division, int Status, int Nrank, string _username);

        Task<List<SifaResultModel>> GetResultSqlitePlusParticularId();
        Task<List<ResultModel>> GetResultSqlite();
        Task<bool> DeleteResultByParticularId(string eyear, long particularid, int _examtype, string _szcandno, string _username);
        Task<ReturnBooleanResponse> InsertResult(string eyear, int subjectid, long particularid, int gradeid, int status, string candno, int etype, int score, string _username);
        Task<List<SubjectModel>> GetSubjectSqlite();
        Task<List<GradeModel>> GetGradeSqlite();
        Task<List<TypeModel>> GetTypeSqlite();
        Task<List<QtvalueModel>> GetQtvalueSqlite();
        Task<bool> InsertSubjectNotExist(string eyear, int etype, string scode, int subsidiary, string sname, string _username);
        Task<bool> InsertGradeNotExist(string eyear, int etype, string grade, string remark, int gtype, int grank, int gpass, int credit, string _username);
        Task<bool> InsertTypeNotExist(string eyear, int etype, string tname, string sname, string tcode, string _username);

        Task<bool> InsertCentreNotExistBulk(string eyear, string centreno, string centrename, string _username);
        Task<bool> InsertCentreBulk(string eyear, IList<CentreModel> cmodel, string _username);
        Task<bool> DeleteResultByTypeId(string eyear, int etype, string _username);

        Task<bool> DeleteParticularByTypeId(string eyear, int etype, string _username);
        Task<bool> InsertParticularBulk(string eyear, IList<SifaParticularModel> cmodel, string _username);
        Task<bool> InsertResultBulk(string eyear, IList<SifaResultModel> cmodel, string _username);
        Task<bool> UpdateParticularPoint(string eyear, int etype, string _username);
        Task<List<CentreModel>> GetCentreSqliteNotSifa();
        Task<bool> UpdateParticularNames(string _sifaTable, string candno, int etype, string Fname, string Oname, string Sname, string Sex, string _username);
        Task<bool> UpdateParticularFee(string _sifaTable, string candno, int etype, string _username);

        Task<List<MysqlParticularModel>> GetMysqlParticularSqlite();
        Task<int> AddMysqlGradeAllNotExist(List<MysqlGradeModel> models);
        Task<int> AddMysqlSubjectAllNotExist(List<MysqlSubjectModel> models);
        Task<List<SifaResultModel>> GetResultSqlitePlusParticularIdYear();
        Task<ReturnDatabaseListResponse> GetAllMysqlCentre2(string eyear, string _username);
        Task<List<SifaResultModel>> GetResultSqlitePlusParticularIdUpdate();
    }
}
