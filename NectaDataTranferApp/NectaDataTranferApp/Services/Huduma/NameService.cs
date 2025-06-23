using MySql.Data.MySqlClient;
using NectaDataTransfer.Shared.Interfaces.Huduma;
using NectaDataTransfer.Shared.Models;
using NectaDataTransfer.Shared.Models.Huduma;
using SQLite;
using System.Data;
//using static MudBlazor.CategoryTypes;
using System.Linq.Expressions;

namespace NectaDataTransfer.Services.Huduma
{
    public class NameService : INameService
	{

		private SQLiteAsyncConnection _connection;

		public NameService()
		{
			SetUpDb();
		}

		private async void SetUpDb()
		{
			if (_connection == null)
			{

				_connection = new SQLiteAsyncConnection(ConstantConn.DatabasePath, ConstantConn.Flags);
				//_ = await _connection.DropTableAsync<NameModel>().ConfigureAwait(true);

				_ = await _connection.CreateTableAsync<NameModel>().ConfigureAwait(false);

			}
		}
		public async Task<int> AddNames(NameModel nameM)
		{
			return await _connection.InsertAsync(nameM).ConfigureAwait(true);
		}
		//public async Task<int> UpdateByWhere(string _username)
		//{
		//    // Define the WHERE clause condition
		//    //int itemIdToUpdate = 123;
		//    //Expression<Func<Item, bool>> whereCondition = item => item.Id == itemIdToUpdate;

		//    //// Update the record(s) matching the WHERE clause
		//    //await dbConnection.Table<Item>().Where(whereCondition).UpdateAsync(new { Quantity = 10 });
		//}
		public async Task<int> DeleteNamesOfUser(string _username)
		{
			string itemIdToDelete = _username;
			Expression<Func<NameModel, bool>> whereCondition = NameModel => NameModel.UserName == itemIdToDelete;

			// Delete the record(s) matching the WHERE clause
			var dresult = await _connection.Table<NameModel>().Where(whereCondition).DeleteAsync();
			return dresult;
		}

		public async Task<List<NameModel>> GetAllSqliteNames(string _username)
		{
			return await _connection.Table<NameModel>().Where(NameModel => NameModel.UserName == _username).ToListAsync().ConfigureAwait(false);
		}

		public async Task<List<NameModel>> GetAllSqliteNamesAndSql(string _username)
		{
			//string query = "SELECT n.NameId,n.ApplicationId,n.Fname,n.Oname,n.Sname,n.Sex ,n.ExamType ,n.ExamName ,n.ExamYear ,n.EmisDB,s.Fname sfname ,s.Oname soname,s.Sname ssname,s.Sex ssex FROM NameModel n LEFT JOIN HudumaSqlNameModel s ON n.NameId = s.NameId ";

			List<NameModel> categories = await _connection.Table<NameModel>().Where(NameModel => NameModel.UserName == _username).ToListAsync();

			foreach (NameModel category in categories)
			{
				List<HudumaSqlNameModel> products = await _connection.Table<HudumaSqlNameModel>()
					.Where(p => p.NameId == category.NameId)
					.ToListAsync();

				//List<HudumaSifaSqlNameModel> sifaname = await _connection.Table<HudumaSifaSqlNameModel>()
				//	.Where(s => s.NameId == category.NameId)
				//	.ToListAsync();

				category.SqlNames = products;
				//category.SifaSqlNames = sifaname;
			}

			return categories;

		}

		public async Task<List<NameModel>> GetAllSqliteNamesAndSqlAndSifa(string _username)
		{
			//string query = "SELECT n.NameId,n.ApplicationId,n.Fname,n.Oname,n.Sname,n.Sex ,n.ExamType ,n.ExamName ,n.ExamYear ,n.EmisDB,s.Fname sfname ,s.Oname soname,s.Sname ssname,s.Sex ssex FROM NameModel n LEFT JOIN HudumaSqlNameModel s ON n.NameId = s.NameId Left join HudumaSifaSqlNameModel p ON n.NameId = p.NameId";

			List<NameModel> categories = await _connection.Table<NameModel>().Where(NameModel => NameModel.UserName == _username).ToListAsync();

			foreach (NameModel category in categories)
			{
				List<HudumaSqlNameModel> products = await _connection.Table<HudumaSqlNameModel>()
					.Where(p => p.NameId == category.NameId)
					.ToListAsync();

				foreach (HudumaSqlNameModel product in products)
				{
					List<HudumaSifaSqlNameModel> supplier = await _connection.Table<HudumaSifaSqlNameModel>()
				.Where(s => s.NameId == category.NameId)
				.ToListAsync();

					category.SifaSqlNames = supplier;
				}

				category.SqlNames = products;
				//category.SifaSqlNames = sifaname;
			}

			return categories;

		}

		public async Task<List<HudumaSqlNameModel>> GetAllSqliteSqlNames(string _username)
		{
			return await _connection.Table<HudumaSqlNameModel>().Where(NameModel => NameModel.UserName == _username).ToListAsync().ConfigureAwait(true);
		}
		public async Task<List<HudumaSifaSqlNameModel>> GetAllSqliteSifaSqlNames(string _username)
		{
			return await _connection.Table<HudumaSifaSqlNameModel>().Where(NameModel => NameModel.UserName == _username).ToListAsync().ConfigureAwait(true);
		}
		//public async Task<CAModelSchool> GetOptionById(int id)
		//{
		//    var toption = await _connection.QueryAsync<CAModelSchool>($"Select * from {nameof(CAModelSchool)} where Id=@_Id", new { _id = id }).ConfigureAwait(true);
		//    return toption.FirstOrDefault();
		//}

		//public async Task<CAModelSchool> GetOptionByUsername(string toption)
		//{
		//    var topt = await _connection.QueryAsync<CAModelSchool>($"Select * from {nameof(CAModelSchool)} where TransferOption='@_name'", new { _name = toption }).ConfigureAwait(true);
		//    return topt.FirstOrDefault();
		//}

		public async Task<int> UpdateNames(NameModel nameM)
		{
			return await _connection.UpdateAsync(nameM).ConfigureAwait(true);
		}
		public async Task<int> DeleteAllNames<T>()
		{
			return await _connection.DeleteAllAsync<NameModel>().ConfigureAwait(false);

		}
		public async Task<List<NameModel>> GetNames(string _username)
		{
			List<NameModel> lstCustomer = new();
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

				MySqlCommand cmd = new("select * from vwhudumanames", con)
				{
					CommandType = CommandType.Text
				};
				con.Open();
				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					NameModel na = new()
					{
						ApplicationId = Convert.ToInt32(rdr["appid"]),
						Fname = rdr["fname"].ToString(),
						Oname = rdr["oname"].ToString(),
						Sname = rdr["sname"].ToString(),
						Sex = rdr["sex"].ToString(),
						ExamType = Convert.ToInt32(rdr["examtype"]),
						ExamName = rdr["examname"].ToString(),
						ExamNumber = rdr["examnumber"].ToString(),
						ExamYear = rdr["examyear"].ToString(),
						EmisDB = rdr["emisdb"].ToString(),
						SifaTable = rdr["sifatable"].ToString(),
						UserName = username

					};

					lstCustomer.Add(na);
				}
				con.Close();

			}

			return lstCustomer;
		}

		public async Task<int> UpdateApplicationNames(string _username, int appid)
		{
			int upname = 0;
			List<NameModel> lstCustomer = new();
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

				MySqlCommand cmd = new($"update applications SET status_id=(SELECT id FROM statuses WHERE CODE='09') WHERE id={appid}", con)
				{
					CommandType = CommandType.Text
				};
				con.Open();
				upname = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				con.Close();

			}
			return upname;

		}
	}
}
