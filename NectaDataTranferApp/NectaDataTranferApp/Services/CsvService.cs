
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using NectaDataTransfer.Shared.Interfaces;
using NectaDataTransfer.Shared.Models;
using SQLite;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using NectaDataTransfer.Shared.Models.CA;


namespace NectaDataTransfer.Services
{
    public class CsvService : ICsvService
    {
        private SQLiteAsyncConnection _connection;

        public CsvService()
        {
            SetUpDb();
        }

        private async void SetUpDb()
        {
            if (_connection == null)
            {
                //string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DataDB.db3");
                _connection = new SQLiteAsyncConnection(ConstantConn.DatabasePath, ConstantConn.Flags);
                //_ = await _connection.DropTableAsync<CsvModel>();
                _ = await _connection.CreateTableAsync<CsvModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<CsvModelSubject>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<CsvModelSchool>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<SifaExamModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<ClassModel>().ConfigureAwait(true);

                // await _connection.DropTableAsync<SchoolModel>();
                _ = await _connection.CreateTableAsync<SchoolModel>().ConfigureAwait(true);
                _ = await _connection.CreateTableAsync<CAModelSubject>().ConfigureAwait(true);

            }
        }
        public async Task<int> AddCsv(CsvModel csvModel)
        {
            return await _connection.InsertAsync(csvModel).ConfigureAwait(true);
        }

       

        public async Task<int> AddClass(ClassModel classModel)
        {
            return await _connection.InsertAsync(classModel).ConfigureAwait(true);
        }

        public async Task<int> AddSchool(SchoolModel schoolModel)
        {
            var existingRecord = await _connection.Table<SchoolModel>()
                                          .Where(s => s.SchoolCode == schoolModel.SchoolCode)
                                          .FirstOrDefaultAsync()
                                          .ConfigureAwait(false);
            if (existingRecord != null)
            {
                return 0;
            }
            else
            {
                return await _connection.InsertAsync(schoolModel).ConfigureAwait(true);
            }
        }
        public async Task<int> UpdateSchool(SchoolModel schoolModel)
        {
            return await _connection.UpdateAsync(schoolModel).ConfigureAwait(true);
        }

        public async Task<int> UpdateCsvInUsajili(int inusajili, string _regno)
        {
            List<CsvModel> csv = await _connection.QueryAsync<CsvModel>($"Update {nameof(CsvModel)} set InUsajili={inusajili} where RegionCode= '{_regno}'").ConfigureAwait(true);
            return csv.Count();
        }

        public async Task<int> UpdateCandNumberSifaTable()
        {
            int myresult = 0;
            string candno = "";
            try
            {
                string updateQuery1 = "UPDATE SifaExamModel SET CandidateNumber = c.CandidateNumber FROM (SELECT CandidateNumber, candidateid FROM CsvModel) AS c WHERE c.candidateid = SifaExamModel.candidateid and SifaExamModel.CandidateNumber=?;";


                myresult = await _connection.ExecuteAsync(updateQuery1, new object[] { candno });


                return myresult;
            }
            catch (Exception)
            {
                return myresult;
            }
        }
        public async Task<int> UpdateCandNumberSubjectTable()
        {


            int myresult = 0;
            string candno = "";
            try
            {
                string updateQuery1 = "UPDATE CsvModelSubject SET CandidateNumber = c.CandidateNumber FROM (SELECT CandidateNumber, candidateid FROM CsvModel) AS c WHERE c.candidateid = CsvModelSubject.candidateid and CsvModelSubject.CandidateNumber=?;";


                myresult = await _connection.ExecuteAsync(updateQuery1, new object[] { candno });


                return myresult;
            }
            catch (Exception)
            {
                return myresult;
            }
        }
        public async Task<int> SchoolUpdateCsvInUsajili(int inusajili, string _schoolno)
        {
            List<CsvModel> csv = await _connection.QueryAsync<CsvModel>($"Update {nameof(CsvModel)} set InUsajili={inusajili} where SchoolCode= '{_schoolno}'").ConfigureAwait(true);
            return csv.Count();
        }

        //public async Task<int> UpdateCsvCandidateNull(int inusajili, string _regno)
        //{
        //	var csv = await _connection.QueryAsync<CsvModel>($"Update {nameof(CsvModel)} set InUsajili={inusajili} where RegionCode= '{_regno}'");
        //	return csv.Count();
        //}

        public async Task<int> DeleteSchool(SchoolModel schoolModel)
        {
            return await _connection.DeleteAsync(schoolModel).ConfigureAwait(true);
        }
        public async Task<int> DeleteAllSchool<T>()
        {
            return await _connection.DeleteAllAsync<SchoolModel>().ConfigureAwait(true);
        }

        public async Task<int> AddCsvAll(List<CsvModel> models)
        {
            return await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
        }
        public async Task<int> AddCsvAllSubject(List<CsvModelSubject> models)
        {
            return await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
        }
        public async Task<int> AddCsvAllSubjectCA(List<CAModelSubject> models)
        {
            return await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
        }
        public async Task<int> AddCsvAllSifa(List<SifaExamModel> models)
        {
            return await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
        }
        public async Task<int> AddCsvAllSchool(List<CsvModelSchool> models)
        {
            return await _connection.InsertAllAsync(models, true).ConfigureAwait(true);
        }

        public async Task<int> DeleteCsv(CsvModel csvModel)
        {
            return await _connection.DeleteAsync(csvModel).ConfigureAwait(true);
        }

        public async Task<int> DeleteCsvByClass(CsvModel csvModel, int classid)
        {
            List<CsvModel> csv = await _connection.QueryAsync<CsvModel>($"Delete from {nameof(CsvModel)} where ClassId= {classid}");
            return csv.Count();
        }

        public async Task<int> DeleteAllCsv<T>()
        {
            return await _connection.DeleteAllAsync<CsvModel>().ConfigureAwait(false);

        }
        public async Task<int> DeleteAllCsvSubject<T>()
        {
            return await _connection.DeleteAllAsync<CsvModelSubject>().ConfigureAwait(false);

        }

        public async Task<int> DeleteAllCsvSubjectCA<T>()
        {
            return await _connection.DeleteAllAsync<CAModelSubject>().ConfigureAwait(false);

        }

        public async Task<int> DeleteAllCsvSchool<T>()
        {
            return await _connection.DeleteAllAsync<CsvModelSchool>().ConfigureAwait(false);

        }
        public async Task<int> DeleteAllCsvSifa<T>()
        {
            return await _connection.DeleteAllAsync<SifaExamModel>().ConfigureAwait(false);

        }
        public async Task<int> DeleteAllClass<T>()
        {
            return await _connection.DeleteAllAsync<ClassModel>().ConfigureAwait(false);

        }
        public async Task<List<CsvModel>> GetAllCsv()
        {
            List<CsvModel> ccc = await _connection.Table<CsvModel>().ToListAsync().ConfigureAwait(true);
            return ccc;

        }
        public async Task<List<SchoolModel>> GetAllSchool()
        {

            List<SchoolModel> ss = await _connection.Table<SchoolModel>().OrderBy(v => v.SchoolCode).ToListAsync().ConfigureAwait(true);
            return ss.DistinctBy(x => x.SchoolCode).ToList();

        }

        public async Task<List<CsvModel>> GetAllSchoolDesti(int _inUsajili)
        {
            List<CsvModel> schools = await _connection.QueryAsync<CsvModel>($"Select distinct SchoolCode from {nameof(CsvModel)} where InUsajili={_inUsajili}").ConfigureAwait(false);
            return schools.ToList();
        }

        public async Task<List<CsvModel>> GetAllSchoolDestiRegion(string regioncode)
        {
            List<CsvModel> schools = await _connection.QueryAsync<CsvModel>($"Select distinct SchoolCode from {nameof(CsvModel)} where cast(RegionCode as int)='{regioncode}'").ConfigureAwait(false);
            return schools.ToList();
        }

        public async Task<List<CsvModel>> GetCsvDataMysql(int _classid, string _schoolno, string _regno, int querryid, string _username)
        {
            List<CsvModel> lstCsvData = new();
            string username = _username;
            string name = "mysql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                con.Open();
                MySqlCommand cmd = new("TransferData", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _ = cmd.Parameters.AddWithValue("@classid", _classid);
                _ = cmd.Parameters.AddWithValue("@regioncode", _regno.Replace("PS", ""));
                _ = cmd.Parameters.AddWithValue("@schoolcode", _schoolno.Replace("PS", ""));
                _ = cmd.Parameters.AddWithValue("@querryid", querryid);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var bdd = GetFormatDate(rdr["bdate"].ToString());
                                       
                    CsvModel db = new()
                    {
                        RegionCode = rdr["regioncode"].ToString(),
                        RegionName = rdr["regionname"].ToString(),
                        DistrictCode = rdr["districtcode"].ToString(),

                        DistrictName = rdr["districtname"].ToString(),

                        SchoolCode = rdr["schoolcode"].ToString(),

                        CandidateNumber = rdr["candidatenumber"].ToString(),

                        Name1 = rdr["fname"].ToString(),

                        Name2 = rdr["oname"].ToString(),

                        Name3 = rdr["sname"].ToString(),

                        Sex = rdr["sex"].ToString(),

                        BirthDate = bdd.ToString(),

                        Address1 = rdr["address1"].ToString(),

                        Address2 = rdr["address2"].ToString(),

                        Address3 = rdr["address3"].ToString(),

                        PhoneNumber = rdr["phonenumber"].ToString(),

                        Vision = Convert.ToInt32(rdr["vision"]),

                        PremNumber = rdr["premnumber"].ToString(),

                        ReferenceNumber = rdr["referencenumber"].ToString(),

                        //PsleYear = rdr["psleyear"].ToString(),

                        //PsleNumber = rdr["pslenumber"].ToString(),

                        //JamiiNumber = rdr["jamiinumber"].ToString(),

                        //BirthCertificateNumber = rdr["bcnnumber"].ToString(),

                        //IsRepeater = rdr["isrepeater"] != DBNull.Value ? Convert.ToInt32(rdr["isrepeater"]) : 0 ,

                        //AdmissionNumber = rdr["admissionnumber"].ToString(),

                        //AdmissionDate = rdr["admissiondate"].ToString(),

                        //TradeId = rdr["tradeid"] != DBNull.Value ? Convert.ToInt32(rdr["tradeid"]) : 0 ,

                        //Nationality = rdr["nationality"].ToString(),


                        ClassId = Convert.ToInt32(rdr["classid"]),
                        CandidateId = Convert.ToInt32(rdr["candid"]),

                        Username = username
                    };

                    lstCsvData.Add(db);
                   
                }

                con.Close();

            }

            return lstCsvData;
        }
        public string GetFormatDate(string? dbdate)
        {
            string dateoutput = "";
            DateTime date;

            if (!string.IsNullOrEmpty(dbdate))
            {

                // Define possible date formats
                string[] formats = { "dd/MM/yyyy", "MMMM dd, yyyy", "dd/MM/yyyy HH:mm:ss", "yyyy-MM-dd", "yyyy/MM/dd", "MMM dd, yyyy", "dd MMMM yyyy", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-dd HH:mm:ss", "HH:mm:ss", "d MMM yyyy", "dddd, dd MMMM yyyy" };
                // Try to parse the date with any of the formats
                if (DateTime.TryParseExact(dbdate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    // Format the date to "yyyy-MM-dd"
                    string formattedDate = date.ToString("yyyy-MM-dd");
                    // Console.WriteLine(formattedDate);
                    dateoutput = formattedDate;
                }
                else
                {
                    dateoutput = "";
                }
            }
            else
            {
                dateoutput = "";
            }

            return dateoutput;
        }

        public async Task<List<CsvModelSubject>> GetCsvDataMysqlSubject(int _classid, string _schoolno, string _regionno, int querryid, string _username)
        {
            List<CsvModelSubject> lstCsvData = new();

            string username = _username;
            string name = "mysql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                try
                {
                    con.Open();
                    MySqlCommand cmd = new("TransferData", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    _ = cmd.Parameters.AddWithValue("@classid", _classid);
                    _ = cmd.Parameters.AddWithValue("@schoolcode", _schoolno.Replace("PS", ""));
                    _ = cmd.Parameters.AddWithValue("@regioncode", _regionno.Replace("PS", ""));
                    _ = cmd.Parameters.AddWithValue("@querryid", querryid);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        CsvModelSubject db = new()
                        {
                            Username = username,
                            SubjectCode = rdr["subjectcode"].ToString(),
                            RegionCode = rdr["regioncode"].ToString(),

                            SchoolCode = rdr["schoolcode"].ToString(),

                            CandidateNumber = rdr["candidatenumber"].ToString(),

                            ClassId = Convert.ToInt32(rdr["classid"]),
                            CandidateId = rdr["candid"] != DBNull.Value ? Convert.ToInt32(rdr["candid"]) : 0,

                            Combi = rdr["combi"].ToString()
                        };

                        lstCsvData.Add(db);
                    }
                    con.Close();
                }
                catch (Exception)
                {
                    con.Close();
                    throw;
                }

            }

            return lstCsvData;
        }

        public async Task<List<CAModelSubject>> GetCsvDataMysqlSubjectCA(int _classid, string _schoolno, string _regionno, int querryid, string _username)
        {
            List<CAModelSubject> lstCsvData = new();

            string username = _username;
            string name = "mysql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                try
                {
                    con.Open();
                    MySqlCommand cmd = new("TransferData", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    _ = cmd.Parameters.AddWithValue("@classid", _classid);
                    _ = cmd.Parameters.AddWithValue("@schoolcode", _schoolno.Replace("PS", ""));
                    _ = cmd.Parameters.AddWithValue("@regioncode", _regionno.Replace("PS", ""));
                    _ = cmd.Parameters.AddWithValue("@querryid", querryid);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        CAModelSubject db = new()
                        {
                            RegionCode = rdr["regioncode"].ToString(),
                            SubjectCode = rdr["subjectcode"].ToString(),
                            SchoolCode = rdr["schoolcode"].ToString(),
                            CandidateNumber = rdr["candidatenumber"].ToString(),
                            Y1T1 = rdr["Y1T1"] != DBNull.Value ? Convert.ToDouble(rdr["Y1T1"]) : null,
                            Y1T2 = rdr["Y1T2"] != DBNull.Value ? Convert.ToDouble(rdr["Y1T2"]) : null,
                            Y2T1 = rdr["Y2T1"] != DBNull.Value ? Convert.ToDouble(rdr["Y2T1"]) : null,
                            Project = rdr["Project"] != DBNull.Value ? Convert.ToDouble(rdr["Project"]) : null,
                            ClassId = Convert.ToInt32(rdr["classid"]),
                            Username = username
                        };

                        lstCsvData.Add(db);
                    }
                    con.Close();
                }
                catch (Exception)
                {
                    con.Close();
                    throw;
                }

            }

            return lstCsvData;
        }
        public async Task<List<SifaExamModel>> GetCsvDataMysqlSifa(int _classid, string _schoolno, string _regionno, int querryid, string _username)
        {
            List<SifaExamModel> lstCsvData = new();

            string username = _username;
            string name = "mysql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                try
                {
                    con.Open();
                    MySqlCommand cmd = new("TransferData", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    _ = cmd.Parameters.AddWithValue("@classid", _classid);
                    _ = cmd.Parameters.AddWithValue("@schoolcode", _schoolno.Replace("PS", ""));
                    _ = cmd.Parameters.AddWithValue("@regioncode", _regionno.Replace("PS", ""));
                    _ = cmd.Parameters.AddWithValue("@querryid", querryid);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SifaExamModel db = new()
                        {
                            RegionCode = rdr["regioncode"].ToString(),

                            SchoolCode = rdr["schoolcode"].ToString(),

                            CandidateNumber = rdr["candidatenumber"].ToString(),
                            SifaNumber = rdr["sifanumber"].ToString(),
                            SifaType = rdr["sifatype"].ToString(),
                            SifaYear = rdr["sifayear"] != DBNull.Value ? Convert.ToInt32(rdr["sifayear"]) : 0,
                            ClassId = rdr["classid"] != DBNull.Value ? Convert.ToInt32(rdr["classid"]) : 0,
                            CandidateId = rdr["candid"] != DBNull.Value ? Convert.ToInt32(rdr["candid"]) : 0,

                        };

                        lstCsvData.Add(db);
                    }
                    con.Close();
                }
                catch (Exception)
                {
                    con.Close();
                    throw;
                }

            }

            return lstCsvData;
        }

        public async Task<List<CsvModelSchool>> GetCsvDataMysqlSchool(int _classid, string _schoolno, string _regionno, int querryid, string _username)
        {
            List<CsvModelSchool> lstCsvData = new();

            string username = _username;
            string name = "mysql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                try
                {
                    con.Open();
                    MySqlCommand cmd = new("TransferData", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    _ = cmd.Parameters.AddWithValue("@classid", _classid);
                    _ = cmd.Parameters.AddWithValue("@schoolcode", _schoolno.Replace("PS", ""));
                    _ = cmd.Parameters.AddWithValue("@regioncode", _regionno.Replace("PS", ""));
                    _ = cmd.Parameters.AddWithValue("@querryid", querryid);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        CsvModelSchool db = new()
                        {
                            RegionCode = rdr["regioncode"].ToString(),
                            DistrictCode = rdr["districtcode"].ToString(),
                            SchoolCode = rdr["schoolcode"].ToString(),
                            SchoolName = rdr["schoolname"].ToString(),
                            IsEnglishMedium = Convert.ToInt32(rdr["isenglish"]),
                            SchoolOwner = Convert.ToInt32(rdr["schoolowner"]),
                            DistanceFromDistrict = Convert.ToInt32(rdr["distancedistrict"]),
                            NambaWizara = rdr["nambawizara"].ToString(),
                            ClassId = Convert.ToInt32(rdr["classid"])
                        };
                        lstCsvData.Add(db);
                    }
                    con.Close();
                }
                catch (Exception)
                {
                    con.Close();
                    throw;
                }

            }

            return lstCsvData;
        }

        public async Task<CsvModel> GetCsvById(int id)
        {
            List<CsvModel> csv = await _connection.QueryAsync<CsvModel>($"Select * from {nameof(CsvModel)} where Id= {id}").ConfigureAwait(false);
            return csv.FirstOrDefault();
        }

        public async Task<CsvModel> GetClassIdFromCsv()
        {
            List<CsvModel> cname = await _connection.QueryAsync<CsvModel>($"Select distinct ClassId from {nameof(CsvModel)}").ConfigureAwait(false);
            return cname.FirstOrDefault();
        }

        public async Task<ClassModel> GetClassIdFromClassModel()
        {
            List<ClassModel> id = await _connection.QueryAsync<ClassModel>($"Select distinct ClassId from {nameof(ClassModel)} where Status=1").ConfigureAwait(false);
            return id.FirstOrDefault();
        }

        public async Task<List<CsvModel>> GetRegionsFromCsv()
        {
            List<CsvModel> regions = await _connection.QueryAsync<CsvModel>($"Select distinct RegionCode,RegionName from {nameof(CsvModel)} ").ConfigureAwait(false);
            return regions.ToList();
        }

        public async Task<List<CsvModel>> GetRegionsFromCsvDesti(int _inusajili)
        {
            List<CsvModel> regions = await _connection.QueryAsync<CsvModel>($"Select distinct RegionCode,RegionName from {nameof(CsvModel)} where InUsajili={_inusajili}").ConfigureAwait(false);
            return regions.ToList();
        }

        public async Task<ClassModel> GetClassByName(string _class)
        {
            List<ClassModel> cname = await _connection.QueryAsync<ClassModel>($"Select * from {nameof(ClassModel)} where ClassName = '{_class}'");
            return cname.FirstOrDefault();
        }

        public async Task<ClassModel> GetClassById(int id)
        {
            List<ClassModel> cname = await _connection.QueryAsync<ClassModel>($"Select * from {nameof(ClassModel)} where ClassId = {id}").ConfigureAwait(false);
            return cname.FirstOrDefault();
        }

        public async Task<List<CsvModel>> GetCsvByRegion(int classid, string regioncode)
        {
            List<CsvModel> csv = await _connection.QueryAsync<CsvModel>($"Select * from {nameof(CsvModel)} where RegionCode= '{regioncode}' and ClassId = {classid}").ConfigureAwait(false);
            return csv.ToList();
        }
        public async Task<List<CsvModelSchool>> GetCsvSchoolByRegion(int classid, string regioncode)
        {
            List<CsvModelSchool> csv = await _connection.QueryAsync<CsvModelSchool>($"Select * from {nameof(CsvModelSchool)} where RegionCode= '{regioncode}' and ClassId = {classid}").ConfigureAwait(false);
            return csv.ToList();
        }

        public async Task<List<CsvModel>> GetCsvByRegionDestination(int classid, string regioncode, int _inUsajili)
        {
            List<CsvModel> csv = await _connection.QueryAsync<CsvModel>($"Select RegionCode, RegionName, DistrictCode, DistrictName, SchoolCode, CandidateNumber, Name1, Name2, Name3, Sex, BirthDate,Address1,Address2,Address3,PhoneNumber, Vision, PremNumber,ClassId,ReferenceNumber from {nameof(CsvModel)} where RegionCode= '{regioncode}' and ClassId = {classid} and InUsajili={_inUsajili} ").ConfigureAwait(false);
            return csv.ToList();
        }

        public async Task<List<CsvModelSchool>> GetCsvSchoolByRegionDestination(int classid, string regioncode)
        {
            List<CsvModelSchool> csv = await _connection.QueryAsync<CsvModelSchool>($"Select RegionCode,DistrictCode,SchoolCode,SchoolName,IsEnglishMedium,SchoolOwner,DistanceFromDistrict,NambaWizara from {nameof(CsvModelSchool)} where RegionCode= '{regioncode}' and ClassId = {classid}").ConfigureAwait(false);
            return csv.ToList();
        }

        public async Task<List<CsvModelSchool>> GetCsvSchoolBySchoolDestination(int classid, string schoolcode)
        {
            List<CsvModelSchool> csv = await _connection.QueryAsync<CsvModelSchool>($"Select RegionCode,DistrictCode,SchoolCode,SchoolName,IsEnglishMedium,SchoolOwner,DistanceFromDistrict,NambaWizara from {nameof(CsvModelSchool)} where SchoolCode= '{schoolcode}' and ClassId = {classid}").ConfigureAwait(false);
            return csv.ToList();
        }

        public async Task<List<SifaExamModel>> GetCsvSifaByRegionDestination(int classid, string regioncode)
        {
            List<SifaExamModel> csv = await _connection.QueryAsync<SifaExamModel>($"Select RegionCode,SchoolCode,CandidateNumber,SifaNumber,SifaType,SifaYear,ClassId from {nameof(SifaExamModel)} where RegionCode= '{regioncode}' and ClassId = {classid}").ConfigureAwait(false);
            return csv.ToList();
        }

        public async Task<List<SifaExamModel>> GetCsvSifaBySchoolDestination(int classid, string schoolcode)
        {
            List<SifaExamModel> csv = await _connection.QueryAsync<SifaExamModel>($"Select RegionCode,SchoolCode,CandidateNumber,SifaNumber,SifaType,SifaYear,ClassId from {nameof(SifaExamModel)} where SchoolCode= '{schoolcode}' and ClassId = {classid}").ConfigureAwait(false);
            return csv.ToList();
        }

        public async Task<List<CsvModelSubject>> GetCsvSubjectByRegionDestination(int classid, string regioncode)
        {
            List<CsvModelSubject> csv = await _connection.QueryAsync<CsvModelSubject>($"Select SubjectCode,RegionCode,SchoolCode,CandidateNumber,ClassId,Combi from {nameof(CsvModelSubject)} where RegionCode= '{regioncode}' and ClassId = {classid}").ConfigureAwait(false);
            return csv.ToList();
        }
        public async Task<List<CsvModelSubject>> GetCsvSubjectBySchoolDestination(int classid, string schoolcode)
        {
            List<CsvModelSubject> csv = await _connection.QueryAsync<CsvModelSubject>($"Select SubjectCode,RegionCode,SchoolCode,CandidateNumber,ClassId,Combi from {nameof(CsvModelSubject)} where SchoolCode= '{schoolcode}' and ClassId = {classid}").ConfigureAwait(false);
            return csv.ToList();
        }
        public async Task<List<CAModelSubject>> GetCsvSubjectByRegionDestinationCA(int classid, string regioncode)
        {
            List<CAModelSubject> csv = await _connection.QueryAsync<CAModelSubject>($"Select SubjectCode,RegionCode,SchoolCode,CandidateNumber,Y1T1,Y1T2,Y2T1,Project,ClassId from {nameof(CAModelSubject)} where RegionCode= '{regioncode}' and ClassId = {classid}").ConfigureAwait(false);
            return csv.ToList();
        }
        public async Task<List<CAModelSubject>> GetCsvSubjectBySchoolDestinationCA(int classid, string schoolcode)
        {
            List<CAModelSubject> csv = await _connection.QueryAsync<CAModelSubject>($"Select SubjectCode,RegionCode,SchoolCode,CandidateNumber,Y1T1,Y1T2,Y2T1,Project,ClassId from {nameof(CAModelSubject)} where SchoolCode= '{schoolcode}' and ClassId = {classid}").ConfigureAwait(false);
            return csv.ToList();
        }
        public async Task<List<CsvModel>> GetCsvBySchool(int classid, string schoolno)
        {
            List<CsvModel> csv = await _connection.QueryAsync<CsvModel>($"Select * from {nameof(CsvModel)} where SchoolCode= '{schoolno}' and ClassId = {classid}");
            return csv.ToList();
        }

        public async Task<int> UpdateCsv(CsvModel csvModel)
        {
            return await _connection.UpdateAsync(csvModel);
        }

        public async Task<List<RegionModel>> GetRegions(string _username)
        {
            List<RegionModel> lstregion = new();

            string username = _username;
            string name = "mysql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                MySqlCommand cmd = new("SELECT distinct RegionCode,RegionName FROM vwregion ", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    RegionModel db = new()
                    {
                        RegionCode = rdr["RegionCode"].ToString(),
                        RegionName = rdr["RegionName"].ToString()
                    };
                    lstregion.Add(db);
                }
                con.Close();
            }
            return lstregion;
        }
        public async Task<List<SchoolModel>> GetSchoolByCode(string schoolcode, string _username)
        {
            List<SchoolModel> lschool = new();

            string username = _username;
            string name = "mysql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                var scode = string.Format("{0}%", schoolcode.ToUpper());
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                MySqlCommand cmd = new($"SELECT distinct SchoolCode,SchoolNameVw FROM vwschool  where UPPER(SchoolCode) like '{scode}'", con)
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 0
                };
                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    SchoolModel db = new()
                    {
                        SchoolCode = rdr["SchoolCode"].ToString(),
                        SchoolName = rdr["SchoolNameVw"].ToString()
                    };
                    lschool.Add(db);
                }
                con.Close();

            }

            return lschool;
        }
        public async Task<List<SchoolModel>> GetSchools(string _username)
        {
            List<SchoolModel> lstregion = new();

            string username = _username;
            string name = "mysql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                MySqlCommand cmd = new("SELECT SchoolCode,SchoolNameVw FROM vwschool ", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    SchoolModel db = new()
                    {
                        SchoolCode = rdr["SchoolCode"].ToString(),
                        SchoolName = rdr["SchoolNameVw"].ToString()
                    };
                    lstregion.Add(db);
                }
                con.Close();

            }
            return lstregion;
        }

        public async Task<int> UpdateClassId(int _classid, string _username)
        {
            int myresult = 0;
            try
            {
                // Define parameters
                // var parameters = new { NewValue = 1, ConditionValue1 = _classid, ConditionValue2 = _username };
                // Your update query goes here
                string updateQuery = "UPDATE ClassModel SET Status = ? where ClassId=? and Username=?";
                var parameters = new { @NewValue = _classid };
                myresult = await _connection.ExecuteAsync(updateQuery, new object[] { 1, _classid, _username });
                return myresult;
            }
            catch (Exception)
            {
                return myresult;
            }



        }

        public async Task<int> UpdateCandidateNumber(int _startnumber, string _username)
        {
            int myresult = 0;
            try
            {
                string updateQuery1 = "DROP TABLE IF EXISTS CandTable";
                string updateQuery2 = "create temporary table CandTable as select id,  (SchoolCode || '-' || printf('%04d',(? + ROW_NUMBER() OVER (PARTITION BY ClassId,SchoolCode ORDER BY Sex,Name1, Name2, Name3)))) candno from CsvModel cm where Username=? and CandidateNumber ='' ";
                // Your update query goes here
                string updateQuery = "UPDATE CsvModel SET CandidateNumber = (SELECT candno  FROM CandTable  AS sub WHERE sub.Id = CsvModel.Id) where CandidateNumber='';";

                _ = await _connection.ExecuteAsync(updateQuery1, new object[] { _startnumber, _username });
                _ = await _connection.ExecuteAsync(updateQuery2, new object[] { _startnumber, _username });

                myresult = await _connection.ExecuteAsync(updateQuery, new object[] { _startnumber, _username });
                return myresult;
            }
            catch (Exception)
            {
                return myresult;
            }
        }
        public async Task<List<ClassModel>> GetClasses(string _username)
        {
            string username = _username;
            string name = "mysql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            List<ClassModel> lstclass = new();

            if (checkconn != null)
            {
                //try
                //{
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                MySqlCommand cmd = new("SELECT distinct ClassId,ClassName FROM vwclass ", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ClassModel db = new()
                    {
                        ClassId = Convert.ToInt32(rdr["ClassId"]),
                        ClassName = rdr["ClassName"].ToString(),
                        Username = username
                    };
                    lstclass.Add(db);
                }
                con.Close();
                //}
                //catch (Exception)
                //{

                //}

            }
            return lstclass;
        }
        public async Task<List<RegStatusModel>> GetRegistrationStatus(int _classid, string _username)
        {
            string username = _username;
            string name = "mysql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}' and Username='{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            List<RegStatusModel> lstregstatus = new();

            if (checkconn != null)
            {
                try
                {
                    using MySqlConnection con = new(Setting.DecryptionMe(checkconn));
                    MySqlCommand cmd = new($"SELECT distinct ClassId,RegStatus,Comments FROM vwregistrationstatus WHERE ClassId={_classid} limit 1 ", con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        RegStatusModel db = new()
                        {
                            ClassId = Convert.ToInt32(rdr["ClassId"]),
                            Comments = rdr["Comments"].ToString(),
                            RegStatus = Convert.ToInt32(rdr["RegStatus"]),
                            Username = _username
                        };
                        lstregstatus.Add(db);
                    }
                    con.Close();
                }
                catch (Exception ex)
                {

                }

            }
            return lstregstatus;
        }
    }
}
