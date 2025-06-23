
using NectaDataTransfer.Shared.Interfaces.Sifa;
using NectaDataTransfer.Shared.Models.Sifa;
using MySql.Data.MySqlClient;
using SQLite;
using System.Data;
using NectaDataTransfer.Shared.Responses;
using System.Data.SqlClient;
using NectaDataTranferApp.Shared.Responses;

namespace NectaDataTransfer.Services.Sifa
{
    public class SifaService : ISifaService
    {

        private SQLiteAsyncConnection _connection;

        public SifaService()
        {

            SetUpDb();
        }

        private async void SetUpDb()
        {
            if (_connection == null)
            {

                //string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DataDB.db3");
                _connection = new SQLiteAsyncConnection(SifaConstantConn.DatabasePath, SifaConstantConn.Flags);
                _ = await _connection.CreateTableAsync<SifaMysqlDatabaseModel>().ConfigureAwait(false);
                _ = await _connection.CreateTableAsync<MysqlCentreModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<MysqlParticularModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<SifaParticularModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<MysqlGradeModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<MysqlSubjectModel>().ConfigureAwait(true);

            }

        }
        #region For Update Particular
        public async Task<List<ParticularModel>> GetParticularSqlite()
        {

            List<ParticularModel> ss = await _connection.QueryAsync<ParticularModel>($"Select * from {nameof(ParticularModel)} ");
            return ss.DistinctBy(x => x.SzCandidatesNumber).ToList();

        }

        public async Task<List<MysqlParticularModel>> GetMysqlParticularSqlite()
        {

            List<MysqlParticularModel> ss = await _connection.Table<MysqlParticularModel>().ToListAsync().ConfigureAwait(true);
            return ss.Distinct().ToList();

        }

        public async Task<List<SifaParticularModel>> GetParticularSqlitePlusCentreId()
        {

            List<SifaParticularModel> dt = await _connection.QueryAsync<SifaParticularModel>("select distinct mcm.Id centreid,szCandidatesNumber,pm.Etype ,pm.Premno ,pm.Ctype ,Fname ,Oname ,Sname ,Sex ,Dbirth ,case when Status=1 then Point else NULL end as Point ,case when Status=1 then Division else NULL end as Division ,Status,Qtvalue,Nrank  from ParticularModel pm JOIN MysqlCentreModel mcm ON pm.SzExamCentreNumber =mcm.SzExamCentreNumber left join QtvalueModel qm on qm.CandidateNumber =pm.SzCandidatesNumber");

            return dt;

        }

        public async Task<ReturnBooleanResponse> InsertParticularNotExist(string eyear, string candno, int etype, int CentreId, string Premno, string Ctype, string Fname, string Oname, string Sname, string Sex, string Dbirth, double Point, string Division, int Status, int Nrank, string _username)
        {
           
            var response = new ReturnBooleanResponse();
            List<MysqlCentreModel> lstResult = new();
            string username = _username;
            string name = "mysql";
            
                List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {

                try
                {
                    using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                    string sql = string.Format("INSERT INTO tbl_{0}_particulars (tbl_exam_centres_Id,szCandidatesNumber,tbl_exam_types_Id,szPersonalIdNumber,szCandidatesType,szFirstName,szOtherNames,szSurName,szSex,dtDateOfBirth,intPoints,szDivision,intStatus,nrank) SELECT {3},'{1}',{2},'{4}','{5}','{6}','{7}','{8}','{9}','{10}',{11},'{12}',{13},{14} WHERE NOT EXISTS (SELECT 1 FROM tbl_{0}_particulars WHERE szCandidatesNumber='{1}' AND tbl_exam_types_Id={2})", eyear, candno, etype, CentreId, Premno, Ctype, Fname, Oname, Sname, Sex, Dbirth, Point, Division, Status, Nrank);

                    MySqlCommand cmd = new(sql, con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    int k = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    con.Close();
                    if (k==1)
                    {
                        response.StatusCode = 200;
                        response.Message = "Add not exist successfully";
                        response.BooleanResponse = true;
                    }
                    else
                    {
                        response.StatusCode = 201;
                        response.Message = "Candidate exist!";
                        response.BooleanResponse = true;
                    }

                    

                }
                catch (AggregateException ae)
                {
                    response.StatusCode = 400;
                    response.Message = ae.Message.ToString();
                    response.BooleanResponse = false;

                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.Message = ex.Message.ToString();
                    response.BooleanResponse = false;

                }
            }
           
            return response;

        }

        public async Task<bool> UpdateParticularNames(string _sifaTable, string candno, int etype, string Fname, string Oname, string Sname, string Sex, string _username)
        {
            bool upok = false;
            List<MysqlCentreModel> lstResult = new();
            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                //try
                //{
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                string sql = string.Format("UPDATE {0} SET szFirstName='{3}',szOtherNames='{4}',szSurName='{5}',szSex='{6}' WHERE  szCandidatesNumber='{1}' AND tbl_exam_types_Id={2}", _sifaTable, candno, etype, Fname, Oname, Sname, Sex);

                MySqlCommand cmd = new(sql, con)
                {
                    CommandType = CommandType.Text
                };

                con.Open();

                _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                con.Close();
                upok = true;
                //}
                //catch (AggregateException)
                //{
                //    upok = false;
                //}
                //catch (Exception)
                //{
                //    upok = false;
                //}
            }
            return upok;
        }
        public async Task<bool> UpdateParticularFee(string _sifaTable, string candno, int etype, string _username)
        {
            bool upok = false;
            List<MysqlCentreModel> lstResult = new();
            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                try
                {
                    List<ParticularModel> ss = await _connection.Table<ParticularModel>().Where(v => v.SzCandidatesNumber == candno && v.Etype == etype).ToListAsync().ConfigureAwait(true);
                    var mylist = ss.Distinct().ToList();
                    foreach (var item in mylist)
                    {
                        using MySqlConnection con = new(Setting.DecryptionMe(checkconn));

                        string sql = string.Format("UPDATE {0} SET intPoints={3},szDivision='{4}',intStatus={5} WHERE  szCandidatesNumber='{1}' AND tbl_exam_types_Id={2}", _sifaTable, candno, etype, item.Point, item.Division, item.Status);

                        MySqlCommand cmd = new(sql, con)
                        {
                            CommandType = CommandType.Text
                        };

                        con.Open();

                        _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                        con.Close();
                        upok = true;
                    }
                }
                catch (AggregateException)
                {
                    upok = false;
                }
                catch (Exception)
                {
                    upok = false;
                }
            }
            return upok;
        }
        public async Task<ReturnBooleanResponse> UpdateParticular(string eyear, string candno, int etype, int CentreId, string Premno, string Ctype, string Fname, string Oname, string Sname, string Sex, string Dbirth, double Point, string Division, int Status, int Nrank, string _username)
        {

            var response = new ReturnBooleanResponse();
            List<MysqlCentreModel> lstResult = new();
            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                try
                {
                    using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                    string sql = string.Format("UPDATE tbl_{0}_particulars SET szPersonalIdNumber ='{4}',szCandidatesType='{5}',szFirstName='{6}',szOtherNames='{7}',szSurName='{8}',szSex='{9}',dtDateOfBirth='{10}',intPoints={11},szDivision='{12}',intStatus={13},nrank={14} WHERE  szCandidatesNumber='{1}' AND tbl_exam_types_Id={2}", eyear, candno, etype, CentreId, Premno, Ctype, Fname, Oname, Sname, Sex, Dbirth, Point, Division, Status, Nrank);

                    MySqlCommand cmd = new(sql, con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    con.Close();
                    response.StatusCode = 200;
                    response.Message = "Particular updated successfully";
                    response.BooleanResponse = true;

                }
                catch (AggregateException ae)
                {
                    response.StatusCode = 400;
                    response.Message = ae.Message.ToString();
                    response.BooleanResponse = false;

                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.Message = ex.Message.ToString();
                    response.BooleanResponse = false;

                }
            }

            return response;
        }

        #endregion

        #region For Update Result
        public async Task<List<ResultModel>> GetResultSqlite()
        {

            List<ResultModel> ss = await _connection.QueryAsync<ResultModel>($"Select * from {nameof(ResultModel)} ");
            return ss.DistinctBy(x => x.SzCandidatesNumber).ToList();

        }

        public async Task<List<SifaResultModel>> GetResultSqlitePlusParticularId()
        {

            List<SifaResultModel> dt = await _connection.QueryAsync<SifaResultModel>("select sm.Id SubjectId,pm.Id ParticualrId,gm.Id GradeId,NULL Status,rm.SzCandidatesNumber,rm.Etype,rm.Score,pm.ExamYear from ResultModel rm join MysqlParticularModel pm on rm.SzCandidatesNumber =pm.SzCandidatesNumber  and rm.Etype =pm.tbl_exam_types_Id  join MysqlSubjectModel sm on rm.Etype =sm.tbl_exam_types_Id  and rm.SubjectCode =sm.szCodeNumber join MysqlGradeModel gm on gm.tbl_exam_types_Id =rm.Etype and rm.Grade =gm.szGrade").ConfigureAwait(false);

            return dt;

        }

        public async Task<List<SifaResultModel>> GetResultSqlitePlusParticularIdUpdate()
        {

            List<SifaResultModel> dt = await _connection.QueryAsync<SifaResultModel>("select sm.Id SubjectId,pm.Id ParticualrId,gm.Id GradeId,NULL Status,rm.SzCandidatesNumber,rm.Etype,rm.Score,pm.ExamYear,rm.SubjectCode from ResultModel rm join MysqlParticularModel pm on rm.SzCandidatesNumber =pm.SzCandidatesNumber  and rm.Etype =pm.tbl_exam_types_Id left join MysqlSubjectModel sm on rm.Etype =sm.tbl_exam_types_Id  and rm.SubjectCode =sm.szCodeNumber join MysqlGradeModel gm on gm.tbl_exam_types_Id =rm.Etype and rm.Grade =gm.szGrade").ConfigureAwait(false);

            return dt;

        }

        public async Task<List<SifaResultModel>> GetResultSqlitePlusParticularIdYear()
        {

            List<SifaResultModel> dt = await _connection.QueryAsync<SifaResultModel>("select sm.Id SubjectId,pm.Id ParticualrId,gm.Id GradeId,NULL Status,rm.SzCandidatesNumber,rm.Etype,rm.Score,pm.ExamYear from ResultModel rm join MysqlParticularModel pm on rm.SzCandidatesNumber =pm.SzCandidatesNumber  and rm.Etype =pm.tbl_exam_types_Id  join MysqlSubjectModel sm on rm.Etype =sm.tbl_exam_types_Id and sm.ExamYear=pm.ExamYear and rm.SubjectCode =sm.szCodeNumber join MysqlGradeModel gm on gm.tbl_exam_types_Id =rm.Etype and rm.Grade =gm.szGrade and gm.ExamYear=pm.ExamYear").ConfigureAwait(false);

            return dt;

        }
        private async ValueTask<int> DeleteParticularLimit(string eyear, int etype, MySqlConnection cc)
        {
            int r;

            try
            {

                string sql = string.Format("DELETE FROM tbl_{0}_particulars WHERE tbl_exam_types_Id={1} LIMIT 10000", eyear, etype);

                using MySqlCommand cmd = new(sql, cc);

                r = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

                return r;
            }
            catch (AggregateException)
            {
                return r = 0;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        private async ValueTask<int> UpdateParticularLimit(string eyear, int etype, MySqlConnection cc)
        {
            int r;

            try
            {

                string sql = string.Format("UPDATE tbl_{0}_particulars SET intPoints = NULL where intPoints=-1 and tbl_exam_types_Id={1} LIMIT 10000", eyear, etype);

                using MySqlCommand cmd = new(sql, cc);

                r = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

                return r;
            }
            catch (AggregateException)
            {
                return r = 0;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public async Task<bool> UpdateParticularPoint(string eyear, int etype, string _username)
        {

            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                try
                {
                    using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                    con.Open();
                    while (true)
                    {
                        int affectedRows = await UpdateParticularLimit(eyear, etype, con);
                        if (affectedRows == 0)
                        {
                            break;
                        }
                    }
                    con.Close();
                }
                catch (AggregateException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<bool> DeleteParticularByTypeId(string eyear, int etype, string _username)
        {

            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                try
                {
                    using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                    con.Open();
                    while (true)
                    {
                        int affectedRows = await DeleteParticularLimit(eyear, etype, con);
                        if (affectedRows == 0)
                        {
                            break;
                        }
                    }
                    con.Close();
                }
                catch (AggregateException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        private async ValueTask<int> DeleteResultLimit(string eyear, int etype, MySqlConnection cc)
        {
            int r;

            try
            {

                string sql = string.Format("DELETE FROM tbl_{0}_results WHERE examtype={1} LIMIT 10000", eyear, etype);

                using MySqlCommand cmd = new(sql, cc);

                r = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

                return r;
            }
            catch (AggregateException)
            {
                return r = 0;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public async Task<bool> DeleteResultByTypeId(string eyear, int etype, string _username)
        {

            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                try
                {
                    using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                    con.Open();
                    while (true)
                    {
                        int affectedRows = await DeleteResultLimit(eyear, etype, con);
                        if (affectedRows == 0)
                        {
                            break;
                        }
                    }
                    con.Close();
                }
                catch (AggregateException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> DeleteResultByParticularId(string eyear, long particularid, int _examtype, string _szcandno, string _username)
        {

            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                try
                {
                    using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                    string sql = string.Format("DELETE FROM tbl_{0}_results WHERE examtype={2} and (tbl_candidates_particulars_Id={1} or Candno='{3}')", eyear, particularid, _examtype, _szcandno);

                    MySqlCommand cmd = new(sql, con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    con.Close();
                }
                catch (AggregateException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<ReturnBooleanResponse> InsertResult(string eyear, int subjectid, long particularid, int gradeid, int status, string candno, int etype, int score, string _username)
        {
            var response = new ReturnBooleanResponse();
            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                try
                {
                    using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                    string sql = string.Format("INSERT INTO tbl_{0}_results (tbl_exam_subjects_Id,tbl_candidates_particulars_Id,tbl_exam_grades_Id,Candno,examtype,score) select {1},{2},{3},'{4}',{5},{6}", eyear, subjectid, particularid, gradeid, candno, etype, score);

                    MySqlCommand cmd = new(sql, con)
                    {
                        CommandType = CommandType.Text
                    };

                    if (subjectid==0)
                    {
                        response.StatusCode = 401;
                        response.Message = "Subject Code does not exist on Sifa DB";
                        response.BooleanResponse = false;
                    }
                    else
                    {
                        con.Open();
                        _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                        con.Close();
                        response.StatusCode = 200;
                        response.Message = "Result updated successfully";
                        response.BooleanResponse = true;
                    }

                       

                }
                catch (AggregateException ae)
                {
                    response.StatusCode = 400;
                    response.Message = ae.Message.ToString();
                    response.BooleanResponse = false;

                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.Message = ex.Message.ToString();
                    response.BooleanResponse = false;

                }
            }

            return response;
        }

        #endregion



        #region For Update Centre
        public async Task<List<CentreModel>> GetCentreSqlite()
        {

            List<CentreModel> ss = await _connection.QueryAsync<CentreModel>($"Select * from {nameof(CentreModel)} ");
            return ss.ToList();

        }
        public async Task<List<CentreModel>> GetCentreSqliteNotSifa()
        {
            List<CentreModel> ss = await _connection.QueryAsync<CentreModel>("select c.SzExamCentreName ,c.SzExamCentreNumber  from CentreModel c left join MysqlCentreModel cm on c.SzExamCentreNumber =cm.SzExamCentreNumber where cm.SzExamCentreNumber is NULL");
            return ss.ToList();

        }
        public async Task<List<SubjectModel>> GetSubjectSqlite()
        {

            List<SubjectModel> ss = await _connection.QueryAsync<SubjectModel>($"Select * from {nameof(SubjectModel)} ");
            return ss.ToList();

        }
        public async Task<List<GradeModel>> GetGradeSqlite()
        {

            List<GradeModel> ss = await _connection.QueryAsync<GradeModel>($"Select * from {nameof(GradeModel)} ");
            return ss.ToList();

        }
        public async Task<List<TypeModel>> GetTypeSqlite()
        {

            List<TypeModel> ss = await _connection.QueryAsync<TypeModel>($"Select * from {nameof(TypeModel)} ");
            return ss.ToList();

        }
        public async Task<List<QtvalueModel>> GetQtvalueSqlite()
        {

            List<QtvalueModel> ss = await _connection.QueryAsync<QtvalueModel>($"Select * from {nameof(QtvalueModel)} ");
            return ss.ToList();

        }

        private IList<string> GetCentreInBatches(string eyear, IList<CentreModel> cmodel)
        {
            string insertSql = "INSERT INTO tbl_{0}_centres (szExamCentreNumber,szExamCentreName) SELECT '{1}','{2}'";
            //var valuesSql = "({0},'{1}', '{2}')";

            //StringBuilder sb = new();
            //sb.Append(insertSql);
            //sb.Append(",");
            //sb.Append(valuesSql);

            List<string> sqlsToExecute = new();
            //  var numberOfBatches = (int)Math.Ceiling((double)userNames.Count / batchSize);

            IEnumerable<string> valuesToInsert = cmodel.Select(u => string.Format(insertSql, eyear, u.SzExamCentreNumber, u.SzExamCentreName.Replace("'", "''")));
            sqlsToExecute.Add(string.Join(';', valuesToInsert));

            return sqlsToExecute;
        }

        private IList<string> GetResultInBatches(string eyear, IList<SifaResultModel> cmodel)
        {
            string insertSql = "INSERT INTO tbl_{0}_results (tbl_exam_subjects_Id,tbl_candidates_particulars_Id,tbl_exam_grades_Id,Candno,examtype,score) select {1},{2},{3},'{4}',{5},{6},'{7}'";

            List<string> sqlsToExecute = new();
            //  var numberOfBatches = (int)Math.Ceiling((double)userNames.Count / batchSize);

            IEnumerable<string> valuesToInsert = cmodel.Select(u => string.Format(insertSql, eyear, u.SubjectId, u.ParticualrId, u.GradeId, u.SzCandidatesNumber, u.Etype, u.Score, u.ExamYear));
            sqlsToExecute.Add(string.Join(';', valuesToInsert));

            return sqlsToExecute;
        }

        public async Task<bool> InsertCentreBulk(string eyear, IList<CentreModel> cmodel, string _username)
        {

            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                IList<string> sqls = GetCentreInBatches(eyear, cmodel);
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                con.Open();

                //try
                //{
                foreach (string sql in sqls)
                {
                    MySqlCommand cmd = new(sql, con)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 0
                    };
                    _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                //}
                //catch (AggregateException)
                //{
                //	return false;
                //}
                //catch (Exception ex)
                //{
                //	await Console.Out.WriteLineAsync(ex.ToString());
                //	return false;
                //}

                //await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                con.Close();
            }
            return true;
        }

        public async Task<bool> InsertParticularBulk(string eyear, IList<SifaParticularModel> cmodel, string _username)
        {

            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using (StreamWriter writer = new("pdata.csv", false))
                {

                    foreach (SifaParticularModel u in cmodel)
                    {
                        writer.WriteLine(string.Join(",", u.CentreId, u.SzCandidatesNumber, u.Etype, u.Premno, u.Ctype, u.Fname, u.Oname, u.Sname, u.Sex, u.Dbirth, u.Point, u.Division, u.Status, u.Qtvalue, u.Nrank));
                    }
                }
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                con.Open();
                //string terminator = "\r\n";
                string sQuery = string.Format("LOAD DATA LOCAL INFILE 'pdata.csv' INTO TABLE tbl_{0}_particulars FIELDS TERMINATED BY ','  (tbl_exam_centres_Id,szCandidatesNumber,tbl_exam_types_Id,szPersonalIdNumber,szCandidatesType,szFirstName,szOtherNames,szSurName,szSex,dtDateOfBirth,intPoints,szDivision,intStatus,qtvalue,nrank)", eyear);

                //try
                //{

                using (MySqlCommand cmd = new(sQuery, con))
                {

                    cmd.CommandTimeout = 0;
                    _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

                }

                //}
                //catch (AggregateException)
                //{
                //	return false;
                //}
                //catch (Exception ex)
                //{
                //	await Console.Out.WriteLineAsync(ex.ToString());
                //	return false;
                //}

                //await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                con.Close();
            }
            return true;
        }

        public async Task<bool> InsertResultBulk(string eyear, IList<SifaResultModel> cmodel, string _username)
        {

            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using (StreamWriter writer = new("rdata.csv", false))
                {

                    foreach (SifaResultModel u in cmodel)
                    {
                        writer.WriteLine(string.Join(",", u.ParticualrId, u.SubjectId, u.GradeId, u.SzCandidatesNumber, u.Etype, u.Score));
                    }
                }
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                con.Open();
                //string terminator = "\r\n";
                string sQuery = string.Format("LOAD DATA LOCAL INFILE 'rdata.csv' INTO TABLE tbl_{0}_results FIELDS TERMINATED BY ',' LINES TERMINATED BY '\n' (tbl_candidates_particulars_Id,tbl_exam_subjects_Id,tbl_exam_grades_Id,Candno,examtype,score)", eyear);

                //try
                //{

                using (MySqlCommand cmd = new(sQuery, con))
                {

                    cmd.CommandTimeout = 0;
                    _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

                }

                //}
                //catch (AggregateException)
                //{
                //	return false;
                //}
                //catch (Exception ex)
                //{
                //	await Console.Out.WriteLineAsync(ex.ToString());
                //	return false;
                //}

                //await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                con.Close();
            }
            return true;
        }

        public async Task<bool> InsertCentreNotExistBulk(string eyear, string centreno, string centrename, string _username)
        {

            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {

                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                con.Open();
                string sql;

                string tableName = eyear;
                // Define a whitelist of allowed characters for the table name
                string allowedChars = "abcdefghijklmnopqrstuvwxyz0213547698ABCDEFGHIJKLMNOPQRSTUVWXYZ_";
                // Remove any characters that are not in the whitelist
                tableName = new string(tableName.Where(c => allowedChars.Contains(c)).ToArray());

                try
                {
                    sql = string.Format("INSERT INTO tbl_{0}_centres (szExamCentreNumber,szExamCentreName) SELECT @1,@2", tableName);
                    MySqlCommand cmd = new(sql, con)
                    {
                        CommandType = CommandType.Text
                    };
                    _ = cmd.Parameters.AddWithValue("@1", centreno);
                    _ = cmd.Parameters.AddWithValue("@2", centrename);
                    //await con.ExecuteAsync(sql);
                    _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

                }
                catch (AggregateException)
                {
                    return false;
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.ToString());
                    return false;
                }

                //await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                con.Close();
            }
            return true;
        }

        public async Task<bool> InsertCentreNotExist(string eyear, string centreno, string centrename, string _username)
        {

            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                try
                {
                    using MySqlConnection con = new(Setting.DecryptionMe(checkconn));

                    string sql = string.Format("INSERT INTO tbl_{0}_centres (szExamCentreNumber,szExamCentreName) SELECT '{1}','{2}' WHERE NOT EXISTS (SELECT 1 FROM tbl_{0}_centres WHERE szExamCentreNumber='{1}')", eyear, centreno, centrename.Replace("'","''"));
                    MySqlCommand cmd = new(sql, con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();
                    //await con.ExecuteAsync(sql);
                    _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    con.Close();
                }
                catch (AggregateException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        public async Task<bool> InsertSubjectNotExist(string eyear, int etype, string scode, int subsidiary, string sname, string _username)
        {
            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                try
                {
                    using MySqlConnection con = new(Setting.DecryptionMe(checkconn));

                    string sql = string.Format("INSERT INTO tbl_{0}_exam_subjects (tbl_exam_types_Id,szCodeNumber,bitSubsidiary,szSubjectName) SELECT {1},'{2}',{3},'{4}' WHERE NOT EXISTS (SELECT 1 FROM tbl_{0}_exam_subjects WHERE tbl_exam_types_Id={1} and szCodeNumber='{2}')", eyear, etype, scode, subsidiary, sname);
                    MySqlCommand cmd = new(sql, con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();
                    _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    con.Close();
                }
                catch (AggregateException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> InsertGradeNotExist(string eyear, int etype, string grade, string remark, int gtype, int grank, int gpass, int credit, string _username)
        {
            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                try
                {
                    using MySqlConnection con = new(Setting.DecryptionMe(checkconn));

                    string sql = string.Format("INSERT INTO tbl_{0}_grades (tbl_exam_types_Id,szGrade,szRemarks,intRanking,blIsPass,blIsCredit) SELECT {1},'{2}','{3}',{4},{5},{6} WHERE NOT EXISTS (SELECT 1 FROM tbl_{0}_grades WHERE tbl_exam_types_Id={1} and szGrade='{2}')", eyear, etype, grade, remark, grank, gpass, credit);
                    MySqlCommand cmd = new(sql, con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();
                    _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    con.Close();
                }
                catch (AggregateException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<bool> InsertTypeNotExist(string eyear, int etype, string tname, string sname, string tcode, string _username)
        {

            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                try
                {
                    using MySqlConnection con = new(Setting.DecryptionMe(checkconn));

                    string sql = string.Format("INSERT INTO tbl_{0}_exam_types (Id,szExaminationTypeName,szShortName,szCode) SELECT {1},'{2}','{3}','{4}' WHERE NOT EXISTS (SELECT 1 FROM tbl_{0}_exam_types WHERE Id={1})", eyear, etype, tname, sname, sname);
                    MySqlCommand cmd = new(sql, con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();
                    _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    con.Close();
                }
                catch (AggregateException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
       

        public async Task<List<SifaMysqlDatabaseModel>> GetMysqlDatabases(string _username)
        {
            List<SifaMysqlDatabaseModel> lstDatabase = new();

            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                MySqlCommand cmd = new("SHOW DATABASES", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    SifaMysqlDatabaseModel db = new()
                    {
                        MysqlDatabase = rdr["Database"].ToString()
                    };
                    lstDatabase.Add(db);
                }
                con.Close();

            }

            return lstDatabase;
        }

        public async Task<ReturnDatabaseListResponse> GetAllMysqlCentre2(string eyear, string _username)
        {
            List<MysqlCentreModel> lstResult = new();
            var response = new ReturnDatabaseListResponse();
            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                try
                {
                    MySqlCommand cmd = new($"select  Id,szExamCentreNumber  from tbl_{eyear}_centres where szExamCentreNumber is not NULL GROUP BY szExamCentreNumber", con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MysqlCentreModel db = new()
                        {
                            Id = Convert.ToInt32(rdr["Id"]),
                            SzExamCentreNumber = rdr["szExamCentreNumber"].ToString()

                        };
                        lstResult.Add(db);
                        
                    }
                    response.StatusCode = 200;
                    response.Message = "Product added successfully";
                    response.MysqlCentreModelList = lstResult;
                    con.Close();
                }
                catch (AggregateException ex)
                {
                    response.StatusCode = 401;
                    response.Message = "Error while get database list: " + ex.Message;
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.Message = "Error while get database list: " + ex.Message;
                }
            }
            return response; 
        }
        public async Task<List<MysqlCentreModel>> GetAllMysqlCentre(string eyear, string _username)
        {
            List<MysqlCentreModel> lstResult = new();

            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                try
                {
                    MySqlCommand cmd = new($"select  Id,szExamCentreNumber  from tbl_{eyear}_centres where szExamCentreNumber is not NULL GROUP BY szExamCentreNumber", con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MysqlCentreModel db = new()
                        {
                            Id = Convert.ToInt32(rdr["Id"]),
                            SzExamCentreNumber = rdr["szExamCentreNumber"].ToString()

                        };
                        lstResult.Add(db);
                    }
                    con.Close();
                }
                catch (AggregateException ex)
                {
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                }
            }
            return lstResult;
        }
        public async Task<List<MysqlParticularModel>> GetAllMysqlParticularCand(string eyear, int etype, string candno, string _username)
        {
            List<MysqlParticularModel> lstResult = new();
            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));

                MySqlCommand cmd = new($"select Id,tbl_exam_types_Id,szCandidatesNumber from tbl_{eyear}_particulars where tbl_exam_types_Id={etype} and szCandidatesNumber='{candno}'", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    MysqlParticularModel db = new()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        tbl_exam_types_Id = Convert.ToInt32(rdr["tbl_exam_types_Id"]),
                        SzCandidatesNumber = rdr["szCandidatesNumber"].ToString(),
                        ExamYear = eyear
                    };
                    lstResult.Add(db);
                }
                con.Close();
            }
            return lstResult;
        }
        public async Task<List<MysqlParticularModel>> GetAllMysqlParticular(string eyear, int etype, string _username)
        {
            List<MysqlParticularModel> lstResult = new();
            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));

                MySqlCommand cmd = new($"select Id,tbl_exam_types_Id,szCandidatesNumber from tbl_{eyear}_particulars where tbl_exam_types_Id={etype}", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    MysqlParticularModel db = new()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        tbl_exam_types_Id = Convert.ToInt32(rdr["tbl_exam_types_Id"]),
                        SzCandidatesNumber = rdr["szCandidatesNumber"].ToString()
                    };
                    lstResult.Add(db);
                }
                con.Close();
            }
            return lstResult;
        }
        public async Task<List<MysqlGradeModel>> GetAllMysqlGrade(string eyear, string _username)
        {
            List<MysqlGradeModel> lstResult = new();
            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));

                MySqlCommand cmd = new($"select Id,tbl_exam_types_Id,szGrade from tbl_{eyear}_grades", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    MysqlGradeModel db = new()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        tbl_exam_types_Id = Convert.ToInt32(rdr["tbl_exam_types_Id"]),
                        szGrade = rdr["szGrade"].ToString(),
                        ExamYear = eyear

                    };
                    lstResult.Add(db);
                }
                con.Close();
            }
            return lstResult;
        }
        public async Task<List<MysqlSubjectModel>> GetAllMysqlSubject(string eyear, string _username)
        {
            List<MysqlSubjectModel> lstResult = new();
            string username = _username;
            string name = "mysql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));

                MySqlCommand cmd = new($"select Id,tbl_exam_types_Id,szCodeNumber from tbl_{eyear}_exam_subjects", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    MysqlSubjectModel db = new()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        tbl_exam_types_Id = Convert.ToInt32(rdr["tbl_exam_types_Id"]),
                        szCodeNumber = rdr["szCodeNumber"].ToString(),
                        ExamYear = eyear

                    };
                    lstResult.Add(db);
                }
                con.Close();
            }
            return lstResult;
        }

        public async Task<int> AddMysqlCentreAll(List<MysqlCentreModel> models)
        {
            //try
            //{
            int r = await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
            return r;

            //}
            //catch (Exception)
            //{
            //	return 0;
            //}

        }
        public async Task<int> AddMysqlSubjectAll(List<MysqlSubjectModel> models)
        {
            try
            {

                int r = await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
                return r;

            }
            catch (Exception)
            {
                return 0;
            }

        }

        public async Task<int> AddMysqlSubjectAllNotExist(List<MysqlSubjectModel> models)
        {

            try
            {
                int r = 0;
                foreach (var entity in models)
                {
                    // Check if the entity with the same primary key already exists
                    var existingEntity = await _connection.QueryAsync<MysqlSubjectModel>($"SELECT * FROM  {nameof(MysqlSubjectModel)} WHERE ExamYear = '{entity.ExamYear}' and szCodeNumber = '{entity.szCodeNumber}' and tbl_exam_types_Id={entity.tbl_exam_types_Id}").ConfigureAwait(false);

                    // If the entity doesn't exist, insert it
                    if (existingEntity == null || existingEntity.Count() == 0)
                    {
                        r = await _connection.InsertAsync(entity);

                    }

                }

                return r;


            }
            catch (Exception)
            {
                return 0;
            }

        }

        public async Task<int> AddMysqlGradeAllNotExist(List<MysqlGradeModel> models)
        {

            try
            {
                int r = 0;
                foreach (var entity in models)
                {
                    // Check if the entity with the same primary key already exists
                    var existingEntity = await _connection.QueryAsync<MysqlGradeModel>($"SELECT * FROM  {nameof(MysqlGradeModel)} WHERE ExamYear = '{entity.ExamYear}' and szGrade = '{entity.szGrade}' and tbl_exam_types_Id={entity.tbl_exam_types_Id}").ConfigureAwait(false);

                    // If the entity doesn't exist, insert it
                    if (existingEntity == null || existingEntity.Count() == 0)
                    {
                        r = await _connection.InsertAsync(entity);

                    }

                }

                return r;


            }
            catch (Exception)
            {
                return 0;
            }

        }
        public async Task<int> AddMysqlParticularAll(List<MysqlParticularModel> models)
        {
            try
            {
                int r = await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
                return r;

            }
            catch (Exception)
            {
                return 0;
            }

        }
        public async Task<int> AddMysqlGradeAll(List<MysqlGradeModel> models)
        {
            try
            {
                int r = await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
                return r;

            }
            catch (Exception)
            {
                return 0;
            }

        }
        public async Task<int> DeleteAllMysqlParticular<T>()
        {
            return await _connection.DeleteAllAsync<MysqlParticularModel>().ConfigureAwait(true);

        }

        public async Task<int> DeleteAllMysqlCentre<T>()
        {
            return await _connection.DeleteAllAsync<MysqlCentreModel>().ConfigureAwait(true);

        }

        public async Task<int> DeleteAllMysqlGrade<T>()
        {
            return await _connection.DeleteAllAsync<MysqlGradeModel>().ConfigureAwait(true);

        }

        public async Task<int> DeleteAllMysqlSubject<T>()
        {
            return await _connection.DeleteAllAsync<MysqlSubjectModel>().ConfigureAwait(true);

        }
    }
}
