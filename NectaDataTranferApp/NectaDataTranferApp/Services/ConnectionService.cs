using MySql.Data.MySqlClient;
using NectaDataTranferApp.Shared.Responses;
using NectaDataTransfer.Shared.Interfaces;
using NectaDataTransfer.Shared.Models;
using NectaDataTransfer.Shared.Responses;
using SQLite;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace NectaDataTransfer.Services
{
    public class ConnectionService : IConnectionService
	{
		private SQLiteAsyncConnection _connection;

		public ConnectionService()
		{
			SetUpDb();
		}
		private async void SetUpDb()
		{
			if (_connection == null)
			{
				_connection = new SQLiteAsyncConnection(ConstantConn.DatabasePath, ConstantConn.Flags);
				await _connection.CreateTableAsync<ConnectionModel>().ConfigureAwait(true);
			}
		}
		public async Task<ReturnBooleanResponse> OpenConnMysql(string pconn)
		{
			MySqlConnection connection = new MySqlConnection(pconn);
            var response = new ReturnBooleanResponse();
            try
			{
				connection.Open();
                response.StatusCode = 200;
                response.Message = "Connection successfully";
                response.BooleanResponse = true;
                return response;

			}

            catch (MySqlException MysqlEx)
            {
                response.StatusCode = MysqlEx.Number;
                response.Message = MysqlEx.Message.ToString();
                response.BooleanResponse = false;
                return response;
            }
            catch (InvalidOperationException invalidOpEx)
            {
                response.StatusCode = 400;
                response.Message = invalidOpEx.Message.ToString();
                response.BooleanResponse = false;
                return response;

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message.ToString();
                response.BooleanResponse = false;
                return response;
            }

        }
		public async Task<ReturnBooleanResponse> OpenConnSql(string pconn)
		{
			SqlConnection connection = new SqlConnection(pconn);
            var response = new ReturnBooleanResponse();
			try
			{
				connection.Open();
				response.StatusCode = 200;
				response.Message = "Connection successfully";
				response.BooleanResponse = true;
				return response;
			}
			catch (SqlException sqlEx)
			{
				response.StatusCode = sqlEx.ErrorCode;
				response.Message = sqlEx.Message.ToString();
				response.BooleanResponse = false;
				return response;
			}
			catch (InvalidOperationException invalidOpEx)
			{
                response.StatusCode = 400;
                response.Message = invalidOpEx.Message.ToString();
                response.BooleanResponse = false;
                return response;

            }
			catch (Exception ex)
			{
				response.StatusCode = 500;
				response.Message = ex.Message.ToString();
				response.BooleanResponse = false;
				return response;
			}

		}
		public async Task<int> AddConnection(ConnectionModel connectionModel)
		{
			return await _connection.InsertAsync(connectionModel).ConfigureAwait(true);

		}

		public async Task<int> DeleteConnection(ConnectionModel connectionModel)
		{
			return await _connection.DeleteAsync(connectionModel).ConfigureAwait(true);
		}

		public async Task<ConnectionModel> GetConnectionById(int id)
		{
			var conn = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Id= {id}").ConfigureAwait(true);
			return conn.FirstOrDefault();
		}

		public async Task<int> UpdateConnection(ConnectionModel connectionModel)
		{
			return await _connection.UpdateAsync(connectionModel).ConfigureAwait(true);
		}

		//public async Task<List<ConnectionModel>> GetConnectionByName(string name)
		//{
		//    var conn = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name = '{name}'").ConfigureAwait(true);
		//    return conn.ToList();
		//}

		public async Task<List<ConnectionModel>> GetConnectionByNameUsername(string name, string userName)
		{
			var conn = await _connection.QueryAsync<ConnectionModel>($"Select * from {nameof(ConnectionModel)} where Name = '{name}' and Username ='{userName}'").ConfigureAwait(true);
			return conn.ToList();
		}

		public async Task<List<ConnectionModel>> GetAllConnection()
		{
			return await _connection.Table<ConnectionModel>().ToListAsync().ConfigureAwait(true);
		}

	}
}
