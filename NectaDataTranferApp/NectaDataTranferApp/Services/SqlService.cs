
using NectaDataTransfer.Shared.Interfaces;
using NectaDataTransfer.Shared.Models;
using SQLite;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace NectaDataTransfer.Services
{
    public class SqlService : ISqlService
    {
        private SQLiteAsyncConnection _connection;
        // private SqlConnection _sqlconnection;

        public SqlService()
        {
            SetUpDb();
        }

        private async void SetUpDb()
        {
            if (_connection == null)
            {
                //string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DataDB.db3");
                _connection = new SQLiteAsyncConnection(ConstantConn.DatabasePath, ConstantConn.Flags);
                _ = await _connection.CreateTableAsync<SqlModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<TransferLogModel>().ConfigureAwait(true);
                //_ = await _connection.DropTableAsync<HudumaSqlNameModel>().ConfigureAwait(true);

                //_ = await _connection.CreateTableAsync<HudumaSqlNameModel>().ConfigureAwait(true);
                //_ = await _connection.CreateTableAsync<HudumaSifaSqlNameModel>().ConfigureAwait(true);
            }
        }
        public async Task<int> AddSql(SqlModel sqlModel)
        {
            return await _connection.InsertAsync(sqlModel).ConfigureAwait(true);
        }
        //public async Task<int> AddHudumaSqlNames(HudumaSqlNameModel _hudumaName)
        //{
        //    return await _connection.InsertAsync(_hudumaName).ConfigureAwait(true);
        //}

        //public async Task<int> AddHudumaSqlNamesAll(List<HudumaSqlNameModel> models)
        //{
        //    int r = await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
        //    return r;
        //}
        public async Task<int> AddTransferLog(TransferLogModel tlog)
        {
            return await _connection.InsertAsync(tlog).ConfigureAwait(true);
        }

        //public async Task<int> AddSql(SqlModel sqlModel)
        //{
        //    //string checkconn = await SecureStorage.Default.GetAsync("sqlconn");
        //    //int response = 0;
        //    //if (checkconn != null)
        //    //{
        //    //    _sqlconnection=new SqlConnection(checkconn);
        //    //    response= await _sqlconnection.Insert(sqlModel);
        //    //}
        //    //return response;    
        //}

        public async Task<int> DeleteSql(SqlModel sqlModel)
        {
            return await _connection.DeleteAsync(sqlModel).ConfigureAwait(true);

        }
        //public async Task<int> DeleteHudumaSqlName(HudumaSqlNameModel _hudumasqlmodel)
        //{
        //    return await _connection.DeleteAsync(_hudumasqlmodel).ConfigureAwait(true);

        //}

        //public async Task<int> DeleteHudumaSqlNameByUsername(string _username)
        //{
        //    string itemIdToDelete = _username;
        //    Expression<Func<HudumaSqlNameModel, bool>> whereCondition = HudumaSqlNameModel => HudumaSqlNameModel.UserName == itemIdToDelete;

        //    // Delete the record(s) matching the WHERE clause
        //    var dresult = await _connection.Table<HudumaSqlNameModel>().Where(whereCondition).DeleteAsync();
        //    return dresult;
        //}
        //public async Task<List<HudumaSqlNameModel>> GetAllHudumaSqlName()
        //{
        //    return await _connection.Table<HudumaSqlNameModel>().ToListAsync().ConfigureAwait(true);
        //}
        public async Task<List<SqlModel>> GetAllSql()
        {
            return await _connection.Table<SqlModel>().ToListAsync().ConfigureAwait(true);
        }

        public async Task<List<TransferLogModel>> GetTransferLog(string tranferoption, int cid)
        {
            return await _connection.Table<TransferLogModel>().Where(x => x.ClassId == cid && x.TransferOption == tranferoption).ToListAsync().ConfigureAwait(true);
        }

        //public async Task<bool> AddTransferLog(TransferLogModel tlog)
        //{
        //	string name = "sql";
        //	var conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'");

        //	var query2 = from element in conModelList
        //				 where element.Name == name
        //				 select element.ConnectionString;

        //	var checkconn = query2.FirstOrDefault();

        //	if (checkconn != null)
        //	{
        //		using (SqlConnection con = new SqlConnection(checkconn))
        //		{
        //			const string query = "insert into Transfer_Log (TransferOption,ClassId,TotalRecord,UserName,TransferDate) values(@toption,@tclassid,@trecord,@tuser,@tdate)";
        //			SqlCommand cmd = new SqlCommand(query, con)
        //			{
        //				CommandType = CommandType.Text,
        //			};

        //			cmd.Parameters.AddWithValue("@toption", tlog.TransferOption);
        //			cmd.Parameters.AddWithValue("@tclassid", tlog.ClassId);
        //			cmd.Parameters.AddWithValue("@trecord", tlog.TotalRecord);
        //			cmd.Parameters.AddWithValue("@tuser", tlog.UserName);
        //			cmd.Parameters.AddWithValue("@tdate", tlog.TransferDate);

        //			con.Open();
        //			await cmd.ExecuteNonQueryAsync();

        //			con.Close();
        //			cmd.Dispose();
        //		}
        //	}
        //	return true;
        //}

        //public async Task<List<TransferLogModel>> GetTransferLog(string tranferoption, int cid)
        //{
        //	List<TransferLogModel> lstlog = new List<TransferLogModel>();

        //	string name = "sql";
        //	var conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'");

        //	var query2 = from element in conModelList
        //				 where element.Name == name
        //				 select element.ConnectionString;

        //	var checkconn = query2.FirstOrDefault();

        //	if (checkconn != null)
        //	{
        //		using (SqlConnection con = new SqlConnection(checkconn))
        //		{
        //			SqlCommand cmd = new SqlCommand("SELECT TransferOption,ClassId,TotalRecord FROM Transfer_Log WHERE TransferOption=@toption and ClassId=@cid", con);
        //			cmd.CommandType = CommandType.Text;
        //			cmd.Parameters.AddWithValue("@toption", tranferoption);
        //			cmd.Parameters.AddWithValue("@cid", cid);
        //			con.Open();
        //			SqlDataReader rdr = cmd.ExecuteReader();
        //			while (rdr.Read())
        //			{
        //				TransferLogModel tl = new TransferLogModel();
        //				tl.TransferOption = rdr["TransferOption"].ToString();
        //				tl.ClassId = Convert.ToInt32(rdr["ClassId"]);
        //				tl.TotalRecord = Convert.ToInt32(rdr["TotalRecord"]);
        //				lstlog.Add(tl);
        //			}
        //			con.Close();
        //		}

        //	}

        //	return lstlog;
        //}

        public async Task<List<SqlDatabaseModel>> GetSqlDatabases(string _username)
        {
            List<SqlDatabaseModel> lstDatabase = new();
            string username = _username;
            string name = "sql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
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
                    SqlDatabaseModel db = new()
                    {
                        SqlDatabase = rdr["nameDB"].ToString()
                    };

                    lstDatabase.Add(db);
                }
                con.Close();

            }

            return lstDatabase;
        }

        public async Task<int> UpdateSql(SqlModel sqlModel)
        {
            return await _connection.UpdateAsync(sqlModel).ConfigureAwait(true);
        }
        //public async Task<int> UpdateHudumaSqlName(HudumaSqlNameModel _hudumaname)
        //{
        //    return await _connection.UpdateAsync(_hudumaname).ConfigureAwait(true);
        //}

        public async Task<bool> RegistrationInsert(Dictionary<string, string> args, string _username)
        {
            int k = 0;
            string username = _username;
            string name = "sql";
            string queryid = "";

            foreach (KeyValuePair<string, string> entry in args)
            {
                if (entry.Value == "10")
                {
                    queryid = entry.Value;
                }
            }

            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {

                SqlConnection sqlconn = new(Setting.DecryptionMe(checkconn));
                try
                {
                    sqlconn.Open();
                    using SqlCommand sqlcmd = new("Transfer_InsertTableData") { CommandType = CommandType.StoredProcedure };
                    foreach (KeyValuePair<string, string> entry in args)
                    {
                        _ = sqlcmd.Parameters.AddWithValue(entry.Key, entry.Value);
                    }
                    sqlcmd.CommandTimeout = 0;
                    sqlcmd.Connection = sqlconn;

                     k = sqlcmd.ExecuteNonQuery();
                    sqlconn.Close();
                    
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
            return (k > 0 || queryid == "10" ) ? true : false;
        }

        public async Task<bool> BulkInsert(DataTable dt, string _username)
        {
            string username = _username;
            string name = "sql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection conn = new(Setting.DecryptionMe(checkconn));
                using SqlCommand command = new("", conn);

                conn.Open();
                //Bulk insert into temp table
                using SqlBulkCopy bulkcopy = new(conn);
                //bulkcopy.BulkCopyTimeout = 660;
                bulkcopy.DestinationTableName = "Transfer_CSVData";
                bulkcopy.ColumnMappings.Add("RegionCode", "RegionCode");
                bulkcopy.ColumnMappings.Add("RegionName", "RegionName");
                bulkcopy.ColumnMappings.Add("DistrictCode", "DistrictCode");
                bulkcopy.ColumnMappings.Add("DistrictName", "DistrictName");
                bulkcopy.ColumnMappings.Add("SchoolCode", "SchoolCode");
                bulkcopy.ColumnMappings.Add("CandidateNumber", "CandidateNumber");
                bulkcopy.ColumnMappings.Add("Name1", "Name1");
                bulkcopy.ColumnMappings.Add("Name2", "Name2");
                bulkcopy.ColumnMappings.Add("Name3", "Name3");
                bulkcopy.ColumnMappings.Add("Sex", "Sex");
                bulkcopy.ColumnMappings.Add("BirthDate", "BirthDate");
                bulkcopy.ColumnMappings.Add("Address1", "Address1");
                bulkcopy.ColumnMappings.Add("Address2", "Address2");
                bulkcopy.ColumnMappings.Add("Address3", "Address3");
                bulkcopy.ColumnMappings.Add("PhoneNumber", "PhoneNumber");
                bulkcopy.ColumnMappings.Add("Vision", "Vision");
                bulkcopy.ColumnMappings.Add("PremNumber", "PremNumber");
                bulkcopy.ColumnMappings.Add("ReferenceNumber", "ReferenceNumber");
                bulkcopy.ColumnMappings.Add("ClassId", "ClassId");
                bulkcopy.WriteToServer(dt);
                bulkcopy.Close();
            }
            return true;
        }
        public async Task<bool> BulkInsertSchool(DataTable dt, string _username)
        {
            string username = _username;
            string name = "sql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection conn = new(Setting.DecryptionMe(checkconn));
                using SqlCommand command = new("", conn);

                conn.Open();
                //Bulk insert into temp table
                using SqlBulkCopy bulkcopy = new(conn);
                //bulkcopy.BulkCopyTimeout = 660;
                bulkcopy.DestinationTableName = "Transfer_CSVDataSchool";
                bulkcopy.WriteToServer(dt);
                bulkcopy.Close();
            }
            return true;
        }
        public async Task<bool> BulkInsertSifa(DataTable dt, string _username)
        {
            string username = _username;
            string name = "sql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection conn = new(Setting.DecryptionMe(checkconn));
                using SqlCommand command = new("", conn);

                conn.Open();
                //Bulk insert into temp table
                using SqlBulkCopy bulkcopy = new(conn);
                //bulkcopy.BulkCopyTimeout = 660;
                bulkcopy.DestinationTableName = "Transfer_CSVDataSifa";
                bulkcopy.WriteToServer(dt);
                bulkcopy.Close();
            }
            return true;
        }
        public async Task<bool> BulkInsertSubject(DataTable dt, string _username)
        {
            string username = _username;
            string name = "sql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection conn = new(Setting.DecryptionMe(checkconn));
                using SqlCommand command = new("", conn);

                conn.Open();
                //Bulk insert into temp table
                using SqlBulkCopy bulkcopy = new(conn);
                //bulkcopy.BulkCopyTimeout = 660;
                bulkcopy.DestinationTableName = "Transfer_CSVDataSubject";
                bulkcopy.WriteToServer(dt);
                bulkcopy.Close();
            }
            return true;
        }
        public async Task<bool> BulkInsertSubjectCA(DataTable dt, string _username)
        {
            string username = _username;
            string name = "sql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using SqlConnection conn = new(Setting.DecryptionMe(checkconn));
                using SqlCommand command = new("", conn);

                conn.Open();
                //Bulk insert into temp table
                using SqlBulkCopy bulkcopy = new(conn);
                //bulkcopy.BulkCopyTimeout = 660;
                bulkcopy.DestinationTableName = "Transfer_CSVDataSubjectCA";
                bulkcopy.WriteToServer(dt);
                bulkcopy.Close();
            }
            return true;
        }
        public async Task<bool> UpdateParticularNames(string candno, int etype, string Fname, string Oname, string Sname, string Sex, string _username)
        {
            string? sqlquerry = null;
            bool upok = false;
            string username = _username;
            string name = "sql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                try
                {
                    using SqlConnection con = new(Setting.DecryptionMe(checkconn));
                    sqlquerry = etype is 7 or 6
                        ? string.Format("UPDATE tbl_prm_candidates_particulars SET szFirstName='{2}',szOtherNames='{3}',szSurName='{4}' WHERE  szCandidatesNumber='{0}' AND tbl_exam_types_Id={1}", candno, etype, Fname, Oname, Sname)
                        : etype == 19
                            ? string.Format("UPDATE Candidate SET FirstName='{2}',MiddleName='{3}',SurName='{4}' WHERE  CandidateNo='{0}'", candno, etype, Fname, Oname, Sname)
                            : etype == 20
                                                    ? string.Format("UPDATE Candidate SET FirstName='{2}',MiddleName='{3}',LastName='{4}' WHERE  CandidateNumber='{0}'", candno, etype, Fname, Oname, Sname)
                                                    : string.Format("UPDATE tbl_candidates_particulars SET szFirstName='{2}',szOtherNames='{3}',szSurName='{4}' WHERE  szCandidatesNumber='{0}' AND tbl_exam_types_Id={1}", candno, etype, Fname, Oname, Sname);

                    SqlCommand cmd = new(sqlquerry, con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    con.Close();
                    upok = true;
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

        public async Task<bool> UpdateParticularNamesSifa(string candno, int etype, string Fname, string Oname, string Sname, string Sex, string _username)
        {
            string? sqlquerry = null;
            bool upok = false;
            string username = _username;
            string name = "sifasql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}'and Username = '{username}'").ConfigureAwait(true);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                try
                {

                    using SqlConnection con = new(Setting.DecryptionMe(checkconn));

                    sqlquerry = etype is 7 or 6
                        ? string.Format("UPDATE tbl_prm_candidates_particulars SET szFirstName='{2}',szOtherNames='{3}',szSurName='{4}' WHERE  szCandidatesNumber='{0}' AND tbl_exam_types_Id={1}", candno, etype, Fname, Oname, Sname)
                        : etype == 19
                            ? string.Format("UPDATE Candidate SET FirstName='{2}',MiddleName='{3}',SurName='{4}' WHERE  CandidateNo='{0}'", candno, etype, Fname, Oname, Sname)
                            : etype == 20
                                                    ? string.Format("UPDATE Candidate SET FirstName='{2}',MiddleName='{3}',LastName='{4}' WHERE  CandidateNumber='{0}'", candno, etype, Fname, Oname, Sname)
                                                    : string.Format("UPDATE tbl_candidates_particulars SET szFirstName='{2}',szOtherNames='{3}',szSurName='{4}' WHERE  szCandidatesNumber='{0}' AND tbl_exam_types_Id={1}", candno, etype, Fname, Oname, Sname);

                    SqlCommand cmd = new(sqlquerry, con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    _ = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    con.Close();
                    upok = true;
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

        public Task<int> DeleteSearchSifaSqlNames<T>()
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSearchSifaSqlByNameID(int _nameId)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteHudumaSqlNameByUsername(string _username)
        {
            throw new NotImplementedException();
        }

        //public async Task<int> AddSearchList(List<HudumaSifaSqlNameModel> models)
        //{
        //    return await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
        //}
        //public async Task<int> AddSearch(HudumaSifaSqlNameModel sifasqlnameModel)
        //{
        //    return await _connection.InsertAsync(sifasqlnameModel).ConfigureAwait(true);
        //}
        //public async Task<int> DeleteSearchSifaSqlNames<T>()
        //{
        //    return await _connection.DeleteAllAsync<HudumaSifaSqlNameModel>().ConfigureAwait(true);
        //}
        //public async Task<int> DeleteSearchSifaSqlByNameID(int _nameId)
        //{

        //    List<HudumaSifaSqlNameModel> deletesifa = await _connection.QueryAsync<HudumaSifaSqlNameModel>($"DELETE from {nameof(HudumaSifaSqlNameModel)} where NameId= {_nameId}");
        //    return deletesifa.Count();

        //}

        //public async Task<List<HudumaSifaSqlNameModel>> GetSearchName(string searchname, string _etypename, int etype, string _eyear, string _emisdb, int _nameid, string _username)
        //{
        //    List<HudumaSifaSqlNameModel> lstSifaNames = new();
        //    string username = _username;
        //    string name = "sifasql";
        //    List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

        //    IEnumerable<string> query2 = from element in conModelList
        //                                 where element.Name == name && element.Username == username
        //                                 select element.ConnectionString;

        //    string? checkconn = query2.FirstOrDefault();

        //    if (checkconn != null)
        //    {
        //        using SqlConnection con = new(Setting.DecryptionMe(checkconn));
        //        SqlCommand cmd = new("SELECT fname,oname,sname,sex,etype examtype, szCandidatesNumber examnumber  FROM sifa_particular b where concat(b.fname,' ',isnull(b.oname,''),' ',b.sname) like @code", con)
        //        {
        //            CommandType = CommandType.Text
        //        };
        //        con.Open();
        //        _ = cmd.Parameters.AddWithValue("@code", "%" + searchname.ToUpper() + "%");
        //        SqlDataReader rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            HudumaSifaSqlNameModel db = new()
        //            {

        //                Fname = rdr["fname"].ToString(),
        //                Oname = rdr["oname"].ToString(),
        //                Sname = rdr["sname"].ToString(),
        //                Sex = rdr["sex"].ToString(),
        //                ExamType = Convert.ToInt32(rdr["examtype"]),
        //                ExamName = _etypename,
        //                ExamNumber = rdr["examnumber"].ToString(),
        //                ExamYear = _eyear,
        //                EmisDB = _emisdb,
        //                NameId = _nameid,
        //                //SifaTable = rdr["sifatable"].ToString(),
        //                UserName = username
        //            };
        //            lstSifaNames.Add(db);

        //        }
        //        con.Close();

        //    }

        //    return lstSifaNames;
        //}
        //public async Task<List<HudumaSqlNameModel>> GetSqlNames(string candno, int etype, string _eyear, string _emisdb, int _nameid, string _username)
        //{
        //    List<HudumaSqlNameModel> lstDatabase = new();
        //    string username = _username;
        //    string name = "sql";
        //    List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(true);

        //    IEnumerable<string> query2 = from element in conModelList
        //                                 where element.Name == name && element.Username == username
        //                                 select element.ConnectionString;

        //    string? checkconn = query2.FirstOrDefault();

        //    if (checkconn != null)
        //    {
        //        using SqlConnection con = new(Setting.DecryptionMe(checkconn));
        //        SqlCommand cmd = new($"SELECT  c.[tbl_exam_types_Id] examtype,t.szShortName examname,[szCandidatesNumber] examnumber,[szSurName] sname,[szFirstName] fname,[szOtherNames] oname,[szSex] sex FROM tbl_candidates_particulars c left join tbl_exam_types_vw t on c.tbl_exam_types_Id=t.Id WHERE szCandidatesNumber ='{candno}' and tbl_exam_types_Id = {etype}", con)
        //        {
        //            CommandType = CommandType.Text
        //        };
        //        try
        //        {

        //            con.Open();

        //            SqlDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                HudumaSqlNameModel db = new()
        //                {

        //                    Fname = rdr["fname"].ToString(),
        //                    Oname = rdr["oname"].ToString(),
        //                    Sname = rdr["sname"].ToString(),
        //                    Sex = rdr["sex"].ToString(),
        //                    ExamType = Convert.ToInt32(rdr["examtype"]),
        //                    ExamName = rdr["examname"].ToString(),
        //                    ExamNumber = rdr["examnumber"].ToString(),
        //                    ExamYear = _eyear,
        //                    EmisDB = _emisdb,
        //                    NameId = _nameid,
        //                    //SifaTable = rdr["sifatable"].ToString(),
        //                    UserName = username
        //                };

        //                lstDatabase.Add(db);

        //            }
        //        }
        //        catch (Exception)
        //        {

        //            con.Close();
        //        }
        //        con.Close();

        //    }

        //    return lstDatabase;
        //}
    }
}
