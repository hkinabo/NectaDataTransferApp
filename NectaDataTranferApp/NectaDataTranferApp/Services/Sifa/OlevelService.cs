using NectaDataTransfer.Shared.Interfaces.Sifa;
using NectaDataTransfer.Shared.Models.Sifa;
using NectaDataTransfer.Shared.Responses;
using SQLite;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace NectaDataTransfer.Services.Sifa
{
    public class OlevelService : IOlevelService
    {
        private SQLiteAsyncConnection _connection;

        public OlevelService()
        {
            SetUpDb();

        }

        private async void SetUpDb()
        {
            if (_connection == null)

            {
                _connection = new SQLiteAsyncConnection(SifaConstantConn.DatabasePath, SifaConstantConn.Flags);

                //await _connection.DropTableAsync<ParticularModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<ParticularModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<ResultModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<YearModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<CentreModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<GradeModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<QtvalueModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<TypeModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<SubjectModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<SifaNameModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<SifaNameBackupModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<SifaFeeBackupModel>().ConfigureAwait(true);

            }
        }
        public async Task<int> AddParticular(ParticularModel pModel)
        {
            return await _connection.InsertAsync(pModel).ConfigureAwait(true);
        }
        public async Task<int> UpdateParticular(ParticularModel pModel)
        {
            return await _connection.UpdateAsync(pModel).ConfigureAwait(true);
        }
        public async Task<int> DeleteParticular(ParticularModel pModel)
        {
            return await _connection.DeleteAsync(pModel).ConfigureAwait(true);
        }
        public async Task<int> UpdateParticularKey()
        {
            int upp = await _connection.ExecuteAsync($"UPDATE sqlite_sequence SET seq = 0 WHERE name = 'ParticularModel'").ConfigureAwait(true);

            //var upp = await _connection.QueryAsync<int>($"UPDATE sqlite_sequence SET seq = 0 WHERE name = {nameof(ParticularModel)}").ConfigureAwait(true);

            return upp;
        }
        public async Task<int> DeleteAllParticular<T>()
        {

            return await _connection.DeleteAllAsync<ParticularModel>().ConfigureAwait(true);

        }

        public async Task<int> AddParticularAll(List<ParticularModel> models)
        {
            return await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
        }
        public async Task<int> AddSifaNameAll(List<SifaNameModel> models)
        {
            return await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
        }
        public async Task<List<SifaNameModel>> GetAllSqliteSifaNames(string _username)
        {
            return await _connection.Table<SifaNameModel>().Where(SifaNameModel => SifaNameModel.UserName == _username).ToListAsync().ConfigureAwait(false);
        }
        public async Task<List<SifaNameBackupModel>> GetAllSifaNamesBackup(string _username)
        {
            return await _connection.Table<SifaNameBackupModel>().Where(SifaNameBackupModel => SifaNameBackupModel.UserName == _username).ToListAsync().ConfigureAwait(false);
        }
        public async Task<List<SifaFeeBackupModel>> GetAllSifaFeeBackup(string _username)
        {
            return await _connection.Table<SifaFeeBackupModel>().Where(SifaFeeBackupModel => SifaFeeBackupModel.UserName == _username).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<SifaSqlDatabaseModel>> GetSqlDatabases(string _username)
        {
            List<SifaSqlDatabaseModel> lstDatabase = new();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)

            {
                try
                {

               
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new("SELECT [name] nameDB FROM master.dbo.sysdatabases WHERE dbid > 4 and[name] <> 'ReportServer' and [name] <> 'ReportServerTempDB';", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    SifaSqlDatabaseModel db = new()
                    {
                        SqlDatabase = rdr["nameDB"].ToString()
                    };
                    lstDatabase.Add(db);
                }
                con.Close();
                }
                catch (Exception)
                {

                    throw;
                }

            }

            return lstDatabase;
        }

        public async Task<ReturnDatabaseListResponse> GetSqlDatabases2(string _username)
        {
            List<SifaSqlDatabaseModel> lstDatabase = new();
            var response = new ReturnDatabaseListResponse();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)

            {
                try
                {


                    using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                    SqlCommand cmd = new("SELECT [name] nameDB FROM master.dbo.sysdatabases WHERE dbid > 4 and[name] <> 'ReportServer' and [name] <> 'ReportServerTempDB';", con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SifaSqlDatabaseModel db = new()
                        {
                            SqlDatabase = rdr["nameDB"].ToString()
                        };
                        lstDatabase.Add(db);
                    }
                    response.StatusCode = 200;
                    response.Message = "Product added successfully";
                    response.SifaSqlDBModelList = lstDatabase;
                    con.Close();
                }
                catch (Exception ex)
                {

                    response.StatusCode = 500;
                    response.Message = "Error while get database list: " + ex.Message;
                }

            }

            return response;
        }

        public async Task<List<ParticularModel>> GetParticularByCandno(string candnumber, string _username)
        {
            List<ParticularModel> lstParticular = new();

            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new($"SELECT  szExamCentreNumber,szCandidatesNumber,premno,etype,fname,oname,sname,sex,ctype,dbirth,point,division,status,0 nrank FROM sifa_particular where SzCandidatesNumber LIKE '{candnumber}';", con)
                {
                    CommandType = CommandType.Text,
                    CommandTimeout=0
                    
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ParticularModel db = new()
                    {
                        SzExamCentreNumber = rdr["szExamCentreNumber"].ToString(),
                        SzCandidatesNumber = rdr["szCandidatesNumber"].ToString(),
                        Premno = rdr["premno"].ToString(),
                        Etype = Convert.ToInt32(rdr["etype"]),
                        Fname = rdr["fname"].ToString(),
                        Oname = rdr["oname"].ToString(),
                        Sname = rdr["sname"].ToString(),
                        Sex = rdr["sex"].ToString(),
                        Ctype = rdr["ctype"].ToString(),
                        Dbirth = rdr["dbirth"].ToString(),
                        Point = Convert.ToDouble(rdr["point"]),
                        Division = rdr["division"].ToString(),
                        Status = Convert.ToInt32(rdr["status"]),
                        Nrank = Convert.ToInt32(rdr["nrank"])
                    };

                    lstParticular.Add(db);
                }
                con.Close();

            }

            return lstParticular;
        }

        public async Task<List<ParticularModel>> GetParticularByCandnoTefis(string candnumber, string _username,string _eyear)
        {
            List<ParticularModel> lstParticular = new();

            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new($"SELECT  szExamCentreNumber,szCandidatesNumber,premno,etype,fname,oname,sname,sex,ctype,dbirth,point,division,status,0 nrank FROM sifa_particular where SzCandidatesNumber LIKE '{candnumber}' and eyear='{_eyear}' ;", con)
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 0

                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ParticularModel db = new()
                    {
                        SzExamCentreNumber = rdr["szExamCentreNumber"].ToString(),
                        SzCandidatesNumber = rdr["szCandidatesNumber"].ToString(),
                        Premno = rdr["premno"].ToString(),
                        Etype = Convert.ToInt32(rdr["etype"]),
                        Fname = rdr["fname"].ToString(),
                        Oname = rdr["oname"].ToString(),
                        Sname = rdr["sname"].ToString(),
                        Sex = rdr["sex"].ToString(),
                        Ctype = rdr["ctype"].ToString(),
                        Dbirth = rdr["dbirth"].ToString(),
                        Point = Convert.ToDouble(rdr["point"]),
                        Division = rdr["division"].ToString(),
                        Status = Convert.ToInt32(rdr["status"]),
                        Nrank = Convert.ToInt32(rdr["nrank"])
                    };

                    lstParticular.Add(db);
                }
                con.Close();

            }

            return lstParticular;
        }

        public async Task<List<ParticularModel>> GetParticularByCandnoFee(string candnumber, string _username, string _emisDB, int _etype)
        {
            List<ParticularModel> lstParticular = new();

            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;
            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                string sqlquerry = string.Format("SELECT  szExamCentreNumber, szCandidatesNumber, premno, etype, fname, oname, sname, sex, ctype, dbirth, point, division, status FROM {0}..sifa_particular where SzCandidatesNumber LIKE '{1}' and etype={2};", _emisDB, candnumber, _etype);
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new(sqlquerry, con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ParticularModel db = new()
                    {
                        SzExamCentreNumber = rdr["szExamCentreNumber"].ToString(),
                        SzCandidatesNumber = rdr["szCandidatesNumber"].ToString(),
                        Premno = rdr["premno"].ToString(),
                        Etype = Convert.ToInt32(rdr["etype"]),
                        Fname = rdr["fname"].ToString(),
                        Oname = rdr["oname"].ToString(),
                        Sname = rdr["sname"].ToString(),
                        Sex = rdr["sex"].ToString(),
                        Ctype = rdr["ctype"].ToString(),
                        Dbirth = rdr["dbirth"].ToString(),
                        Point = Convert.ToDouble(rdr["point"]),
                        Division = rdr["division"].ToString(),
                        Status = Convert.ToInt32(rdr["status"]),

                    };

                    lstParticular.Add(db);
                }
                con.Close();

            }

            return lstParticular;
        }

        public async Task UpdateSifaNameStore(string _etype, string _candnumber, int _mwaka, string _username)
        {
            List<ParticularModel> lstParticular = new();

            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new($"update maombi_marekebisho_DPM set [status]=4 where mtihani = '{_etype}' and mwaka={_mwaka} and cand_no='{_candnumber}';", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                cmd.ExecuteNonQuery();

                con.Close();

            }
        }
        public async Task UpdateSifaNameStoreFee(string _etype, string _candnumber, int _mwaka, string _username)
        {
            List<ParticularModel> lstParticular = new();

            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new($"update maombi_ada set [status]=4 where mtihani = '{_etype}' and mwaka={_mwaka} and namba='{_candnumber}';", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                cmd.ExecuteNonQuery();

                con.Close();

            }
        }

        public async Task<int> InsertSifaFeeBackup(string _etype, string _candnumber, int _mwaka, string _username)
        {
            int ff = 0;

            string myq = ($"Insert into {nameof(SifaFeeBackupModel)} (ExamName,ExamNumber,ExamYear,UserName) Values('{_etype}','{_candnumber}','{_mwaka}','{_username}')");

           ff = await _connection.ExecuteAsync(myq).ConfigureAwait(true);
            return ff;
        }

        public async Task<int> InsertSifaNameBackup(string _etype, string _candnumber, int _mwaka, string _username)
        {
            int ff = 0;
            string myq = ($"Insert into {nameof(SifaNameBackupModel)} (ExamName,ExamNumber,ExamYear,UserName) Values('{_etype}','{_candnumber}','{_mwaka}','{_username}')");

            ff = await _connection.ExecuteAsync(myq).ConfigureAwait(true);
            return ff;
        }
        public async Task<List<SifaNameModel>> GetAllSifaName(string _username)
        {
            List<SifaNameModel> lstParticular = new();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new($"SELECT  m.[mtihani],m.mwaka,m.[cand_no],m.[fname_new] fname,m.[mname_new] oname,m.[lname_new] sname,m.[sex_new] sex,m.[kuzaliwa_new] dbirth FROM [maombi_marekebisho_DPM] as m JOIN (select distinct mtihani,mwaka,cand_no, max(tarehe_ombi) dtare from maombi_marekebisho_DPM where [status]=2 group by mtihani,mwaka,cand_no ) as d on m.mwaka=d.mwaka and m.cand_no=d.cand_no and m.mtihani=d.mtihani and m.tarehe_ombi=d.dtare where m.[status] = 2;", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    SifaNameModel db = new()
                    {
                        ExamName = rdr["mtihani"].ToString(),
                        ExamYear = Convert.ToInt32(rdr["mwaka"]),
                        ExamNumber = rdr["cand_no"].ToString(),
                        Fname = rdr["fname"].ToString(),
                        Oname = rdr["oname"].ToString(),
                        Sname = rdr["sname"].ToString(),
                        Sex = rdr["sex"].ToString().Trim(),
                        Dbirth = rdr["dbirth"].ToString(),
                        ExamType = GetType(rdr["mtihani"].ToString()),
                        SifaTable = string.Format("tbl_{0}_particulars", rdr["mwaka"].ToString()),
                        UserName = _username
                    };

                    lstParticular.Add(db);
                }
                con.Close();

            }

            return lstParticular;
        }

        public async Task<List<SifaNameModel>> GetAllSifaNameFee(string _username)
        {
            List<SifaNameModel> lstParticular = new();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new($"SELECT m.exam etype,m.mtihani [mtihani],m.[mwaka],m.[namba] cand_no,m.jina fname FROM [maombi_ada] m join (select distinct exam,mwaka,namba,max(folio) dfolio from maombi_ada where status=2 group by exam,mwaka,namba) d on m.exam=d.exam and m.mwaka=d.mwaka and m.namba=d.namba and m.folio=d.dfolio where m.[status]=2;", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    SifaNameModel db = new()
                    {
                        ExamName = rdr["mtihani"].ToString(),
                        ExamYear = Convert.ToInt32(rdr["mwaka"]),
                        ExamNumber = rdr["cand_no"].ToString(),
                        Fname = rdr["fname"].ToString(),
                        ExamType = GetType(rdr["etype"].ToString()),
                        SifaTable = string.Format("tbl_{0}_particulars", rdr["mwaka"].ToString()),
                        EmisDB = string.Format("{1}{0}", rdr["mwaka"].ToString(), rdr["mtihani"].ToString()),
                        UserName = _username
                    };

                    lstParticular.Add(db);
                }
                con.Close();

            }

            return lstParticular;
        }

        private int GetType(string type)
        {
            int typeVal = 0;

            switch (type)
            {
                case "CSEE": typeVal = 1; break;
                case "ACSEE": typeVal = 2; break;
                case "FTNA": typeVal = 19; break;
                case "PSLE": typeVal = 7; break;
                case "GATCE": typeVal = 10; break;
                case "GATSCCE": typeVal = 12; break;
                case "DSEE": typeVal = 14; break;
                case "QT": typeVal = 6; break;
                case "SFNA": typeVal = 20; break;
            }


            return typeVal;
        }
        public async Task<List<ParticularModel>> GetAllParticular(string _username)
        {
            List<ParticularModel> lstParticular = new();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
               

                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new("SELECT szExamCentreNumber,szCandidatesNumber,premno,etype,fname,oname,sname,sex,ctype,dbirth,point,division,status,nrank FROM sifa_particular;", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                try
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                {
                    ParticularModel db = new()
                    {
                        SzExamCentreNumber = rdr["szExamCentreNumber"].ToString(),
                        SzCandidatesNumber = rdr["szCandidatesNumber"].ToString(),
                        Premno = rdr["premno"].ToString(),
                        Etype = Convert.ToInt32(rdr["etype"]),
                        Fname = rdr["fname"].ToString(),
                        Oname = rdr["oname"].ToString(),
                        Sname = rdr["sname"].ToString(),
                        Sex = rdr["sex"].ToString(),
                        Ctype = rdr["ctype"].ToString(),
                        Dbirth = rdr["dbirth"].ToString(),
                        Point = Convert.ToDouble(rdr["point"]),
                        Division = rdr["division"].ToString(),
                        Status = Convert.ToInt32(rdr["status"]),
                        Nrank = Convert.ToInt32(rdr["nrank"])
                    };

                    lstParticular.Add(db);
                }
                con.Close();

                }
                catch (Exception ex)
                {
                    con.Close();    
                    throw ex;
                }
            }

            return lstParticular;
        }



        public async Task<List<ResultModel>> GetResultByCandno(string candnumber, int etype, string _username)
        {
            List<ResultModel> lstResult = new();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new($"SELECT szCandidatesNumber,etype,subjectid,grade,score FROM sifa_result where SzCandidatesNumber='{candnumber}' and etype={etype};", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ResultModel db = new()
                    {
                        SzCandidatesNumber = rdr["szCandidatesNumber"].ToString(),
                        Etype = Convert.ToInt32(rdr["etype"]),
                        SubjectCode = rdr["subjectid"].ToString(),
                        Grade = rdr["grade"].ToString(),
                        Score = Convert.ToInt32(rdr["score"])
                    };

                    lstResult.Add(db);
                }
                con.Close();

            }

            return lstResult;
        }

        public async Task<List<ResultModel>> GetResultByCandnoTefis(string candnumber, int etype, string _username,string _eyear)
        {
            List<ResultModel> lstResult = new();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new($"SELECT szCandidatesNumber,etype,subjectid,grade,score FROM sifa_result where SzCandidatesNumber='{candnumber}' and etype={etype} and eyear='{_eyear}';", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ResultModel db = new()
                    {
                        SzCandidatesNumber = rdr["szCandidatesNumber"].ToString(),
                        Etype = Convert.ToInt32(rdr["etype"]),
                        SubjectCode = rdr["subjectid"].ToString(),
                        Grade = rdr["grade"].ToString(),
                        Score = Convert.ToInt32(rdr["score"])
                    };

                    lstResult.Add(db);
                }
                con.Close();

            }

            return lstResult;
        }
        public async Task<List<ResultModel>> GetResultByCandnoFee(string candnumber, int etype, string _username, string _emisDb)
        {
            List<ResultModel> lstResult = new();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                string sqlquerry = string.Format("SELECT szCandidatesNumber,etype,subjectid,grade,score FROM {0}..sifa_result where SzCandidatesNumber='{1}' and etype={2};", _emisDb, candnumber, etype);
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new(sqlquerry, con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ResultModel db = new()
                    {
                        SzCandidatesNumber = rdr["szCandidatesNumber"].ToString(),
                        Etype = Convert.ToInt32(rdr["etype"]),
                        SubjectCode = rdr["subjectid"].ToString(),
                        Grade = rdr["grade"].ToString(),
                        Score = Convert.ToInt32(rdr["score"])
                    };

                    lstResult.Add(db);
                }
                con.Close();

            }

            return lstResult;
        }

        public async Task<List<CentreModel>> GetCentreByCentreno(string centrenumber, string _username)
        {
            List<CentreModel> lstResult = new();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new($"SELECT szExamCentreNumber,szExamCentreName FROM sifa_centre where SzExamCentreNumber='{centrenumber}';", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CentreModel db = new()
                    {
                        SzExamCentreNumber = rdr["szExamCentreNumber"].ToString(),
                        SzExamCentreName = rdr["szExamCentreName"].ToString()

                    };

                    lstResult.Add(db);
                }
                con.Close();

            }

            return lstResult;
        }

        public async Task<List<ResultModel>> GetAllResult(string _username)
        {
            List<ResultModel> lstResult = new();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                try
                {

                    SqlCommand cmd = new("SELECT szCandidatesNumber,etype,subjectid,isnull(grade,'') grade,isnull(score,-1) score FROM sifa_result;", con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        ResultModel db = new()
                        {
                            SzCandidatesNumber = rdr["szCandidatesNumber"].ToString(),
                            Etype = Convert.ToInt32(rdr["etype"]),
                            SubjectCode = rdr["subjectid"].ToString(),
                            Grade = rdr["grade"].ToString(),
                            Score = (rdr["score"].ToString() == "-1") ? null : Convert.ToInt32(rdr["score"])
                        };

                        lstResult.Add(db);
                    }
                    con.Close();
                }
                catch (SqlException ex)
                {
                    // handle SQL exception
                    throw ex;
                }

            }

            return lstResult;
        }

        public async Task<int> AddYear(YearModel yModel)
        {
            return await _connection.InsertAsync(yModel).ConfigureAwait(true);
        }

        public async Task<int> DeleteParticularbyCandno<T>(string candno)
        {


            string itemIdToDelete = candno;
            Expression<Func<ParticularModel, bool>> whereCondition = ParticularModel => ParticularModel.SzCandidatesNumber == itemIdToDelete;

            // Delete the record(s) matching the WHERE clause
            var dresult = await _connection.Table<ParticularModel>().Where(whereCondition).DeleteAsync();
            return dresult;

        }
        public async Task<int> DeleteAllYear<T>(string _username)
        {


            string itemIdToDelete = _username;
            Expression<Func<YearModel, bool>> whereCondition = YearModel => YearModel.UserName == itemIdToDelete;

            // Delete the record(s) matching the WHERE clause
            var dresult = await _connection.Table<YearModel>().Where(whereCondition).DeleteAsync();
            return dresult;

        }

        public async Task<YearModel> GetYear(string _username)
        {
            List<YearModel> cname = await _connection.QueryAsync<YearModel>($"Select * from {nameof(YearModel)} where UserName='{_username}'");
            return cname.FirstOrDefault();

        }

        public async Task<YearModel> GetYearByName(string _y)
        {
            List<YearModel> cname = await _connection.QueryAsync<YearModel>($"Select * from {nameof(YearModel)} where YearSelected = '{_y}'");
            return cname.FirstOrDefault();
        }

        public async Task<List<CentreModel>> GetAllCentre(string _username)
        {
            List<CentreModel> lstResult = new();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new("SELECT distinct SzExamCentreName,szExamCentreNumber FROM sifa_centre;", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CentreModel db = new()
                    {
                        SzExamCentreName = rdr["SzExamCentreName"].ToString(),
                        SzExamCentreNumber = rdr["szExamCentreNumber"].ToString()

                    };

                    lstResult.Add(db);
                }
                con.Close();
            }
            return lstResult;
        }
        public async Task<List<GradeModel>> GetAllGrade(string _username)
        {
            List<GradeModel> lstResult = new();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new("SELECT typeid,gradename,remark, isnull(gradeid,0) gradeid,intrank,ispass,credit FROM sifa_grade;", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    GradeModel db = new()
                    {
                        TypeId = Convert.ToInt32(rdr["typeid"]),
                        GradeName = rdr["gradename"].ToString(),
                        Remark = rdr["remark"].ToString(),
                        GradeId = Convert.ToInt32(rdr["gradeid"]),
                        IntRank = Convert.ToInt32(rdr["intrank"]),
                        IsPass = Convert.ToInt32(rdr["ispass"]),
                        Credit = Convert.ToInt32(rdr["credit"])
                    };

                    lstResult.Add(db);
                }
                con.Close();
            }
            return lstResult;
        }

        public async Task<List<QtvalueModel>> GetAllQtvalue(string _username)
        {
            List<QtvalueModel> lstResult = new();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                try
                {
                    using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                    SqlCommand cmd = new("SELECT qtvalue,candno FROM sifa_qtvalue;", con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        QtvalueModel db = new()
                        {
                            Qtvalue = Convert.ToInt32(rdr["qtvalue"]),
                            CandidateNumber = rdr["candno"].ToString()

                        };

                        lstResult.Add(db);
                    }
                    con.Close();
                }
                catch (Exception)
                {

                }

            }
            return lstResult;
        }

        public async Task<List<TypeModel>> GetAllType(string _username)
        {
            List<TypeModel> lstResult = new();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new("SELECT typeid,typename,shortname,szcode FROM sifa_type;", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    TypeModel db = new()
                    {
                        TypeId = Convert.ToInt32(rdr["typeid"]),
                        TypeName = rdr["typename"].ToString(),
                        ShortName = rdr["shortname"].ToString(),
                        SzCode = rdr["szcode"].ToString(),
                    };

                    lstResult.Add(db);
                }
                con.Close();

            }

            return lstResult;
        }

        public async Task<List<SubjectModel>> GetAllSubject(string _username)
        {
            List<SubjectModel> lstResult = new();
            string username = _username;
            string name = "sql";
            List<SifaConnectionModel> conModelList = await _connection.QueryAsync<SifaConnectionModel>($"Select  * from {nameof(SifaConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                SqlCommand cmd = new("SELECT etype,subjectcode,subsidiary,subjectname FROM sifa_subject;", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    SubjectModel db = new()
                    {
                        Etype = Convert.ToInt32(rdr["etype"]),
                        SubjectCode = rdr["subjectcode"].ToString(),
                        Subsidiary = Convert.ToInt32(rdr["subsidiary"]),
                        SubjectName = rdr["subjectname"].ToString(),
                    };

                    lstResult.Add(db);
                }
                con.Close();

            }

            return lstResult;
        }

        public async Task<int> DeleteAllSifaName<T>()
        {
            return await _connection.DeleteAllAsync<SifaNameModel>().ConfigureAwait(true);

        }
        public async Task<int> DeleteAllSifaNameBackup<T>()
        {
            return await _connection.DeleteAllAsync<SifaNameBackupModel>().ConfigureAwait(true);

        }
        public async Task<int> DeleteAllSifaFeeBackup<T>()
        {
            return await _connection.DeleteAllAsync<SifaFeeBackupModel>().ConfigureAwait(true);

        }

        public async Task<int> DeleteAllResult<T>()
        {
            return await _connection.DeleteAllAsync<ResultModel>().ConfigureAwait(true);

        }

        public async Task<int> DeleteAllCentre<T>()
        {
            return await _connection.DeleteAllAsync<CentreModel>().ConfigureAwait(true);

        }

        public async Task<int> DeleteAllGrade<T>()
        {
            return await _connection.DeleteAllAsync<GradeModel>().ConfigureAwait(true);

        }

        public async Task<int> DeleteAllQtvalue<T>()
        {
            return await _connection.DeleteAllAsync<QtvalueModel>().ConfigureAwait(true);

        }

        public async Task<int> DeleteAllType<T>()
        {
            return await _connection.DeleteAllAsync<TypeModel>().ConfigureAwait(true);

        }

        public async Task<int> DeleteAllSubject<T>()
        {
            return await _connection.DeleteAllAsync<SubjectModel>().ConfigureAwait(true);

        }

        public async Task<int> AddResultAll(List<ResultModel> models)
        {
            int r = await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
            return r;
        }

        public async Task<int> AddCentreAll(List<CentreModel> models)
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

        public async Task<int> AddQtvalueAll(List<QtvalueModel> models)
        {
            int r = await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
            return r;
        }

        public async Task<int> AddGradeAll(List<GradeModel> models)
        {
            int r = await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
            return r;
        }

        public async Task<int> AddTypeAll(List<TypeModel> models)
        {
            int r = await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
            return r;
        }

        public async Task<int> AddSubjectAll(List<SubjectModel> models)
        {
            int r = await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
            return r;
        }

        public async Task<List<ParticularModel>> GetAllParticularSqlite()
        {

            List<ParticularModel> ss = await _connection.Table<ParticularModel>().OrderBy(v => v.SzCandidatesNumber).ToListAsync().ConfigureAwait(true);
            return ss.DistinctBy(x => x.SzCandidatesNumber).ToList();

        }

        //public async Task<List<CsvModel>> GetAllCsv()
        //{
        //	var ccc = await _connection.Table<CsvModel>().ToListAsync().ConfigureAwait(true);
        //	return ccc;

        //}
        //public async Task<List<SchoolModel>> GetAllSchool()
        //{

        //	var ss = await _connection.Table<SchoolModel>().OrderBy(v => v.SchoolCode).ToListAsync().ConfigureAwait(true);
        //	return ss.DistinctBy(x => x.SchoolCode).ToList();

        //}

        //public async Task<List<CsvModel>> GetAllSchoolDesti()
        //{
        //	var schools = await _connection.QueryAsync<CsvModel>($"Select distinct SchoolCode,SchoolName from {nameof(CsvModel)} where InUsajili=1").ConfigureAwait(false);
        //	return schools.ToList();
        //}

        //public async Task<List<CsvModel>> GetCsvDataMysql(int _classid, string _schoolno, string _regno)
        //{
        //	List<CsvModel> lstCsvData = new List<CsvModel>();

        //	string name = "mysql";
        //	var conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'").ConfigureAwait(false);

        //	var query2 = from element in conModelList
        //				 where element.Name == name
        //				 select element.ConnectionString;

        //	var checkconn = query2.FirstOrDefault();

        //	if (checkconn != null)
        //	{
        //		using (MySqlConnection con = new MySqlConnection(checkconn))
        //		{
        //			con.Open();
        //			MySqlCommand cmd = new MySqlCommand("TransferData", con);
        //			cmd.CommandType = CommandType.StoredProcedure;
        //			cmd.Parameters.AddWithValue("@classid", _classid);
        //			cmd.Parameters.AddWithValue("@regioncode", _regno.Replace("PS", ""));
        //			cmd.Parameters.AddWithValue("@schoolcode", _schoolno.Replace("PS", ""));
        //			MySqlDataReader rdr = cmd.ExecuteReader();
        //			while (rdr.Read())
        //			{
        //				CsvModel db = new CsvModel();
        //				db.RegionCode = rdr["regioncode"].ToString();
        //				db.RegionName = rdr["regionname"].ToString();
        //				db.DistrictCode = rdr["districtcode"].ToString();

        //				db.DistrictName = rdr["districtname"].ToString();

        //				db.SchoolCode = rdr["schoolcode"].ToString();

        //				db.SchoolName = rdr["schoolname"].ToString();

        //				db.IsEnglishMedium = Convert.ToInt32(rdr["isenglish"]);

        //				db.CandidateNumber = rdr["candidatenumber"].ToString();

        //				db.Name1 = rdr["fname"].ToString();

        //				db.Name2 = rdr["oname"].ToString();

        //				db.Name3 = rdr["sname"].ToString();

        //				db.Sex = rdr["sex"].ToString();

        //				db.BirthDate = rdr["bdate"].ToString();

        //				db.Vision = Convert.ToInt32(rdr["vision"]);

        //				db.PremNumber = rdr["premnumber"].ToString();

        //				db.SchoolOwner = Convert.ToInt32(rdr["schoolowner"]);

        //				db.ClassId = Convert.ToInt32(rdr["classid"]);

        //				lstCsvData.Add(db);
        //			}
        //			con.Close();
        //		}

        //	}

        //	return lstCsvData;
        //}

        //public async Task<List<CsvModel>> GetCsvDataMysql9999(int _classid, string _schoolno, string _regno)
        //{
        //	List<CsvModel> lstCsvData = new List<CsvModel>();

        //	string name = "mysql";
        //	var conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'").ConfigureAwait(false);

        //	var query2 = from element in conModelList
        //				 where element.Name == name
        //				 select element.ConnectionString;

        //	var checkconn = query2.FirstOrDefault();

        //	if (checkconn != null)
        //	{
        //		using (MySqlConnection con = new MySqlConnection(checkconn))
        //		{
        //			con.Open();
        //			MySqlCommand cmd = new MySqlCommand("TransferData", con);
        //			cmd.CommandType = CommandType.StoredProcedure;
        //			cmd.Parameters.AddWithValue("@classid", _classid);
        //			cmd.Parameters.AddWithValue("@regioncode", _regno.Replace("PS", ""));
        //			cmd.Parameters.AddWithValue("@schoolcode", _schoolno.Replace("PS", ""));
        //			MySqlDataReader rdr = cmd.ExecuteReader();
        //			while (rdr.Read())
        //			{
        //				CsvModel db = new CsvModel();
        //				db.RegionCode = rdr["regioncode"].ToString();
        //				db.RegionName = rdr["regionname"].ToString();
        //				db.DistrictCode = rdr["districtcode"].ToString();

        //				db.DistrictName = rdr["districtname"].ToString();

        //				db.SchoolCode = rdr["schoolcode"].ToString();

        //				db.SchoolName = rdr["schoolname"].ToString();

        //				db.IsEnglishMedium = Convert.ToInt32(rdr["isenglish"]);

        //				db.CandidateNumber = rdr["candidatenumber"].ToString();

        //				db.Name1 = rdr["fname"].ToString();

        //				db.Name2 = rdr["oname"].ToString();

        //				db.Name3 = rdr["sname"].ToString();

        //				db.Sex = rdr["sex"].ToString();

        //				db.BirthDate = rdr["bdate"].ToString();

        //				db.Vision = Convert.ToInt32(rdr["vision"]);

        //				db.PremNumber = rdr["premnumber"].ToString();

        //				db.SchoolOwner = Convert.ToInt32(rdr["schoolowner"]);

        //				db.ClassId = Convert.ToInt32(rdr["classid"]);

        //				lstCsvData.Add(db);
        //			}
        //			con.Close();
        //		}

        //	}

        //	return lstCsvData;
        //}

        //public async Task<List<CsvModel>> GetCsvDataMysqlSchool(int _classid, string _schoolno, string _regionno)
        //{
        //	List<CsvModel> lstCsvData = new List<CsvModel>();

        //	string name = "mysql";
        //	var conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'").ConfigureAwait(false);

        //	var query2 = from element in conModelList
        //				 where element.Name == name
        //				 select element.ConnectionString;

        //	var checkconn = query2.FirstOrDefault();

        //	if (checkconn != null)
        //	{
        //		using (MySqlConnection con = new MySqlConnection(checkconn))
        //		{
        //			con.Open();
        //			MySqlCommand cmd = new MySqlCommand("TransferData", con);
        //			cmd.CommandType = CommandType.StoredProcedure;
        //			cmd.Parameters.AddWithValue("@classid", _classid);
        //			cmd.Parameters.AddWithValue("@schoolcode", _schoolno.Replace("PS", ""));
        //			cmd.Parameters.AddWithValue("@regioncode", _regionno.Replace("PS", ""));
        //			MySqlDataReader rdr = cmd.ExecuteReader();
        //			while (rdr.Read())
        //			{
        //				CsvModel db = new CsvModel();
        //				db.RegionCode = rdr["regioncode"].ToString();
        //				db.RegionName = rdr["regionname"].ToString();
        //				db.DistrictCode = rdr["districtcode"].ToString();

        //				db.DistrictName = rdr["districtname"].ToString();

        //				db.SchoolCode = rdr["schoolcode"].ToString();

        //				db.SchoolName = rdr["schoolname"].ToString();

        //				db.IsEnglishMedium = Convert.ToInt32(rdr["isenglish"]);

        //				db.CandidateNumber = rdr["candidatenumber"].ToString();

        //				db.Name1 = rdr["fname"].ToString();

        //				db.Name2 = rdr["oname"].ToString();

        //				db.Name3 = rdr["sname"].ToString();

        //				db.Sex = rdr["sex"].ToString();

        //				db.BirthDate = rdr["bdate"].ToString();

        //				db.Vision = Convert.ToInt32(rdr["vision"]);

        //				db.PremNumber = rdr["premnumber"].ToString();

        //				db.SchoolOwner = Convert.ToInt32(rdr["schoolowner"]);

        //				db.ClassId = Convert.ToInt32(rdr["classid"]);

        //				lstCsvData.Add(db);
        //			}
        //			con.Close();
        //		}

        //	}

        //	return lstCsvData;
        //}

        //public async Task<CsvModel> GetCsvById(int id)
        //{
        //	var csv = await _connection.QueryAsync<CsvModel>($"Select * from {nameof(CsvModel)} where Id= {id}").ConfigureAwait(false);
        //	return csv.FirstOrDefault();
        //}

        //public async Task<CsvModel> GetClassIdFromCsv()
        //{
        //	var cname = await _connection.QueryAsync<CsvModel>($"Select distinct ClassId from {nameof(CsvModel)}").ConfigureAwait(false);
        //	return cname.FirstOrDefault();
        //}

        //public async Task<List<CsvModel>> GetRegionsFromCsv()
        //{
        //	var regions = await _connection.QueryAsync<CsvModel>($"Select distinct RegionCode,RegionName from {nameof(CsvModel)} ").ConfigureAwait(false);
        //	return regions.ToList();
        //}

        //public async Task<List<CsvModel>> GetRegionsFromCsvDesti()
        //{
        //	var regions = await _connection.QueryAsync<CsvModel>($"Select distinct RegionCode,RegionName from {nameof(CsvModel)} where InUsajili=1").ConfigureAwait(false);
        //	return regions.ToList();
        //}

        //public async Task<ClassModel> GetClassByName(string _class)
        //{
        //	var cname = await _connection.QueryAsync<ClassModel>($"Select * from {nameof(ClassModel)} where ClassName = '{_class}'");
        //	return cname.FirstOrDefault();
        //}

        //public async Task<ClassModel> GetClassById(int id)
        //{
        //	var cname = await _connection.QueryAsync<ClassModel>($"Select * from {nameof(ClassModel)} where ClassId = {id}").ConfigureAwait(false);
        //	return cname.FirstOrDefault();
        //}

        //public async Task<List<CsvModel>> GetCsvByRegion(int classid, string regioncode)
        //{
        //	var csv = await _connection.QueryAsync<CsvModel>($"Select * from {nameof(CsvModel)} where RegionCode= '{regioncode}' and ClassId = {classid}").ConfigureAwait(false);
        //	return csv.ToList();
        //}

        //public async Task<List<CsvModel>> GetCsvByRegionDestination(int classid, string regioncode)
        //{
        //	var csv = await _connection.QueryAsync<CsvModel>($"Select RegionCode, RegionName, DistrictCode, DistrictName, SchoolCode, SchoolName, IsEnglishMedium, CandidateNumber, Name1, Name2, Name3, Sex, BirthDate, Vision, PremNumber, SchoolOwner, ClassId from {nameof(CsvModel)} where RegionCode= '{regioncode}' and ClassId = {classid} and InUsajili=1 ").ConfigureAwait(false);
        //	return csv.ToList();
        //}

        //public async Task<List<CsvModel>> GetCsvBySchool(int classid, string schoolno)
        //{
        //	var csv = await _connection.QueryAsync<CsvModel>($"Select * from {nameof(CsvModel)} where SchoolCode= '{schoolno}' and ClassId = {classid}");
        //	return csv.ToList();
        //}

        //public async Task<int> UpdateCsv(CsvModel csvModel)
        //{
        //	return await _connection.UpdateAsync(csvModel);
        //}

        //public async Task<List<RegionModel>> GetRegions()
        //{
        //	List<RegionModel> lstregion = new List<RegionModel>();

        //	string name = "mysql";
        //	var conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'");

        //	var query2 = from element in conModelList
        //				 where element.Name == name
        //				 select element.ConnectionString;

        //	var checkconn = query2.FirstOrDefault();

        //	if (checkconn != null)
        //	{
        //		using (MySqlConnection con = new MySqlConnection(checkconn))
        //		{
        //			MySqlCommand cmd = new MySqlCommand("SELECT distinct RegionCode,RegionName FROM vwregion ", con);
        //			cmd.CommandType = CommandType.Text;
        //			con.Open();
        //			MySqlDataReader rdr = cmd.ExecuteReader();
        //			while (rdr.Read())
        //			{
        //				RegionModel db = new RegionModel();
        //				db.RegionCode = rdr["RegionCode"].ToString();
        //				db.RegionName = rdr["RegionName"].ToString();
        //				lstregion.Add(db);
        //			}
        //			con.Close();
        //		}
        //	}
        //	return lstregion;
        //}
        //public async Task<List<SchoolModel>> GetSchoolByCode(string schoolcode)
        //{
        //	List<SchoolModel> lschool = new List<SchoolModel>();

        //	string name = "mysql";
        //	var conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name = '{name}'");

        //	var query2 = from element in conModelList
        //				 where element.Name == name
        //				 select element.ConnectionString;

        //	var checkconn = query2.FirstOrDefault();

        //	if (checkconn != null)
        //	{
        //		using (MySqlConnection con = new MySqlConnection(checkconn))
        //		{
        //			MySqlCommand cmd = new MySqlCommand("SELECT distinct SchoolCode,SchoolNameVw FROM vwschool  where UPPER(SchoolCode) like @code", con);
        //			cmd.CommandType = CommandType.Text;
        //			con.Open();
        //			cmd.Parameters.AddWithValue("@code", schoolcode.ToUpper() + "%");
        //			MySqlDataReader rdr = cmd.ExecuteReader();
        //			while (rdr.Read())
        //			{
        //				SchoolModel db = new SchoolModel();
        //				db.SchoolCode = rdr["SchoolCode"].ToString();
        //				db.SchoolName = rdr["SchoolNameVw"].ToString();
        //				lschool.Add(db);
        //			}
        //			con.Close();
        //		}

        //	}

        //	return lschool;
        //}
        //public async Task<List<SchoolModel>> GetSchools()
        //{
        //	List<SchoolModel> lstregion = new List<SchoolModel>();

        //	string name = "mysql";
        //	var conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'").ConfigureAwait(false);

        //	var query2 = from element in conModelList
        //				 where element.Name == name
        //				 select element.ConnectionString;

        //	var checkconn = query2.FirstOrDefault();

        //	if (checkconn != null)
        //	{
        //		using (MySqlConnection con = new MySqlConnection(checkconn))
        //		{
        //			MySqlCommand cmd = new MySqlCommand("SELECT SchoolCode,SchoolNameVw FROM vwschool ", con);
        //			cmd.CommandType = CommandType.Text;
        //			con.Open();
        //			MySqlDataReader rdr = cmd.ExecuteReader();
        //			while (rdr.Read())
        //			{
        //				SchoolModel db = new SchoolModel();
        //				db.SchoolCode = rdr["SchoolCode"].ToString();
        //				db.SchoolName = rdr["SchoolNameVw"].ToString();
        //				lstregion.Add(db);
        //			}
        //			con.Close();
        //		}

        //	}
        //	return lstregion;
        //}
        //public async Task<List<ClassModel>> GetClasses()
        //{
        //	string name = "mysql";
        //	var conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'").ConfigureAwait(false);

        //	var query2 = from element in conModelList
        //				 where element.Name == name
        //				 select element.ConnectionString;

        //	var checkconn = query2.FirstOrDefault();

        //	List<ClassModel> lstclass = new List<ClassModel>();

        //	if (checkconn != null)
        //	{
        //		try
        //		{
        //			using (MySqlConnection con = new MySqlConnection(checkconn))
        //			{
        //				MySqlCommand cmd = new MySqlCommand("SELECT distinct ClassId,ClassName FROM vwclass ", con);
        //				cmd.CommandType = CommandType.Text;
        //				con.Open();
        //				MySqlDataReader rdr = cmd.ExecuteReader();
        //				while (rdr.Read())
        //				{
        //					ClassModel db = new ClassModel();
        //					db.ClassId = Convert.ToInt32(rdr["ClassId"]);
        //					db.ClassName = rdr["ClassName"].ToString();
        //					lstclass.Add(db);
        //				}
        //				con.Close();
        //			}
        //		}
        //		catch (Exception)
        //		{

        //		}

        //	}
        //	return lstclass;
        //}
    }
}
