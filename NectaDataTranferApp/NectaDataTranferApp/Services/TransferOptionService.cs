
using NectaDataTransfer.Shared.Interfaces;
using NectaDataTransfer.Shared.Models;
using SQLite;

namespace NectaDataTransfer.Services
{
    public class TransferOptionService : ITransferOption
	{

		private SQLiteAsyncConnection _connection;

		public TransferOptionService()
		{
			SetUpDb();
		}

		private async void SetUpDb()
		{
			if (_connection == null)
			{
				//  string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DataDB.db3");
				_connection = new SQLiteAsyncConnection(ConstantConn.DatabasePath, ConstantConn.Flags);
				_ = await _connection.CreateTableAsync<TransferOptionModel>().ConfigureAwait(true);
			}
		}
		public async Task<int> AddOption(TransferOptionModel transferOptModel)
		{
			return await _connection.InsertAsync(transferOptModel).ConfigureAwait(true);
		}

        public async Task<List<TransferOptionModel>> GetAllOption()
        {
            return await _connection.Table<TransferOptionModel>().ToListAsync().ConfigureAwait(true);
        }

        public async Task<int> DeleteOption(TransferOptionModel transferOptModel)
		{
			return await _connection.DeleteAsync(transferOptModel).ConfigureAwait(true);
		}

		public async Task<List<TransferOptionModel>> GetAllOption(string _username)
		{
            return await _connection.Table<TransferOptionModel>().Where(x => x.Username == _username).ToListAsync().ConfigureAwait(true);
        }

		public async Task<TransferOptionModel> GetOptionById(int id)
		{
			List<TransferOptionModel> toption = await _connection.QueryAsync<TransferOptionModel>($"Select * from {nameof(TransferOptionModel)} where Id=@_Id", new { _id = id }).ConfigureAwait(true);
			return toption.FirstOrDefault();
		}

		public async Task<TransferOptionModel> GetOptionByUsername(string toption)
		{
			List<TransferOptionModel> topt = await _connection.QueryAsync<TransferOptionModel>($"Select * from {nameof(TransferOptionModel)} where TransferOption='@_name'", new { _name = toption }).ConfigureAwait(true);
			return topt.FirstOrDefault();
		}

		public async Task<int> UpdateOption(TransferOptionModel transferOptModel)
		{
			return await _connection.UpdateAsync(transferOptModel).ConfigureAwait(true);
		}
	

        public async Task<int> DeleteAllOption<T>(string _username)
        {
            // return await _connection.DeleteAllAsync<SifaTransferOptionModel>().ConfigureAwait(true);
            // Delete the record(s) matching the WHERE clause
            var dresult = await _connection.Table<TransferOptionModel>().Where(x => x.Username == _username).DeleteAsync();
            return dresult;
        }
    }
}
