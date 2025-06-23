using NectaDataTransfer.Shared.Interfaces.Sifa;
using NectaDataTransfer.Shared.Models.Sifa;
using SQLite;

namespace NectaDataTransfer.Services.Sifa
{
    public class SifaConnectionService : ISifaConnectionService
	{
		private SQLiteAsyncConnection _connection;

		public SifaConnectionService()
		{
			SetUpDb();
		}
		private async void SetUpDb()
		{
			if (_connection == null)

			{
				SQLiteConnectionString opt = new(SifaConstantConn.DatabasePath, SifaConstantConn.Flags, true, "Kin@12345", postKeyAction: e => { _ = e.Execute("PRAGMA cipher_compatibility=3"); });
				_connection = new SQLiteAsyncConnection(opt);

				_ = await _connection.CreateTableAsync<SifaConnectionModel>().ConfigureAwait(true);
			}
		}
		public async Task<int> AddConnection(SifaConnectionModel connectionModel)
		{
			return await _connection.InsertAsync(connectionModel).ConfigureAwait(true);

		}

		public async Task<int> DeleteConnection(SifaConnectionModel connectionModel)
		{
			return await _connection.DeleteAsync(connectionModel).ConfigureAwait(true);
		}

		public async Task<SifaConnectionModel> GetConnectionById(int id)
		{
			List<SifaConnectionModel> conn = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Id= {id}").ConfigureAwait(true);
			return conn.FirstOrDefault();
		}

		public async Task<int> UpdateConnection(SifaConnectionModel connectionModel)
		{
			return await _connection.UpdateAsync(connectionModel).ConfigureAwait(true);
		}

		//public async Task<List<SifaConnectionModel>> GetConnectionByName(string name)
		//{
		//	List<SifaConnectionModel> conn = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name = '{name}'").ConfigureAwait(true);
		//	return conn.ToList();
		//}
		public async Task<List<SifaConnectionModel>> GetConnectionByNameUsername(string name, string userName)
		{
			List<SifaConnectionModel> conn = await _connection.QueryAsync<SifaConnectionModel>($"Select * from {nameof(SifaConnectionModel)} where Name = '{name}' and Username = '{userName}' LIMIT 1").ConfigureAwait(true);
			return conn.ToList();
		}

		public async Task<List<SifaConnectionModel>> GetAllConnection()
		{
			return await _connection.Table<SifaConnectionModel>().ToListAsync().ConfigureAwait(true);
		}

	}
}
