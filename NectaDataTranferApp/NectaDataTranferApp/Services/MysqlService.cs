using MySql.Data.MySqlClient;
using NectaDataTransfer.Shared.Interfaces;
using NectaDataTransfer.Shared.Models;
using SQLite;
using System.Data;

namespace NectaDataTransfer.Services
{
    public class MysqlService : IMysqlService
    {

        private SQLiteAsyncConnection _connection;

        public MysqlService()
        {

            SetUpDb();
        }

        private async void SetUpDb()
        {
            if (_connection == null)
            {

                //string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DataDB.db3");
                _connection = new SQLiteAsyncConnection(ConstantConn.DatabasePath, ConstantConn.Flags);
                _ = await _connection.CreateTableAsync<MysqlModel>().ConfigureAwait(false);

            }

        }
        public async Task<int> AddMysql(MysqlModel mysqlModel)
        {
            return await _connection.InsertAsync(mysqlModel);

        }

        public async Task<int> DeleteMysql(MysqlModel mysqlModel)
        {
            return await _connection.DeleteAsync(mysqlModel);
        }

        public async Task<List<MysqlModel>> GetAllMysql()
        {
            return await _connection.Table<MysqlModel>().ToListAsync();
        }

        public async Task<MysqlModel> GetMysqlById(int id)
        {
            List<MysqlModel> expense = await _connection.QueryAsync<MysqlModel>($"Select * from {nameof(MysqlModel)} where Id= {id}");
            return expense.FirstOrDefault();
        }

        public async Task<int> UpdateMysql(MysqlModel mysqlModel)
        {
            return await _connection.UpdateAsync(mysqlModel);
        }

        public async Task<MysqlModel> GetMysqlByUsername(string uname)
        {
            List<MysqlModel> expense = await _connection.QueryAsync<MysqlModel>($"Select * from {nameof(MysqlModel)} where Username= {uname}").ConfigureAwait(false);
            return expense.FirstOrDefault();
        }
        public async Task<List<MysqlModel>> GetAllMysqlADO(string _username)
        {
            List<MysqlModel> lstCustomer = new();
            string username = _username;
            string name = "mysql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(false);

            IEnumerable<string> query2 = from element in conModelList
                                         where element.Name == name && element.Username == username
                                         select element.ConnectionString;

            string? checkconn = query2.FirstOrDefault();

            if (checkconn != null)
            {
                using MySqlConnection con = new(Setting.DecryptionMe(checkconn));

                MySqlCommand cmd = new("select * from candidates", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    MysqlModel Customer = new()
                    {
                        Id = Convert.ToInt32(rdr["CustomerID"]),
                        Username = rdr["Name"].ToString(),
                        Password = rdr["Gender"].ToString(),
                        Host = rdr["Country"].ToString()
                    };

                    lstCustomer.Add(Customer);
                }
                con.Close();

            }

            return lstCustomer;
        }
        public async Task<List<MysqlDatabaseModel>> GetMysqlDatabases(string _username)
        {
            List<MysqlDatabaseModel> lstDatabase = new();
            string username = _username;
            string name = "mysql";
            List<ConnectionModel> conModelList = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name= '{name}' and Username = '{username}'").ConfigureAwait(false);

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
                    MysqlDatabaseModel db = new()
                    {
                        MysqlDatabase = rdr["Database"].ToString()
                    };
                    lstDatabase.Add(db);
                }
                con.Close();

            }

            return lstDatabase;
        }
    }
}

