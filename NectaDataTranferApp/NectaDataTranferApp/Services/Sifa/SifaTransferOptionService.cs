using NectaDataTransfer.Shared.Interfaces.Sifa;
using NectaDataTransfer.Shared.Models.Sifa;
using SQLite;

namespace NectaDataTransfer.Services.Sifa
{
    public class SifaTransferOptionService : ISifaTransferOption
    {

        private SQLiteAsyncConnection _connection;



        public SifaTransferOptionService()
        {
            SetUpDb();
        }

        private async void SetUpDb()
        {
            if (_connection == null)
            {

                _connection = new SQLiteAsyncConnection(SifaConstantConn.DatabasePath, SifaConstantConn.Flags);
                _ = await _connection.CreateTableAsync<SifaTransferOptionModel>().ConfigureAwait(true);
            }
        }
        public async Task<int> AddOption(SifaTransferOptionModel transferOptModel)
        {
            return await _connection.InsertAsync(transferOptModel).ConfigureAwait(true);
        }

        public async Task<int> DeleteOption(SifaTransferOptionModel transferOptModel)
        {
            return await _connection.DeleteAsync(transferOptModel).ConfigureAwait(true);
        }



        public async Task<List<SifaTransferOptionModel>> GetAllOption(string _username)
        {

            return await _connection.Table<SifaTransferOptionModel>().Where(x => x.Username == _username).ToListAsync().ConfigureAwait(true);
        }

        public async Task<SifaTransferOptionModel> GetOptionById(int id)
        {
            List<SifaTransferOptionModel> toption = await _connection.QueryAsync<SifaTransferOptionModel>($"Select * from {nameof(SifaTransferOptionModel)} where Id=@_Id", new { _id = id }).ConfigureAwait(true);
            return toption.FirstOrDefault();
        }

        public async Task<SifaTransferOptionModel> GetOptionByUsername(string _username)
        {
            List<SifaTransferOptionModel> topt = await _connection.QueryAsync<SifaTransferOptionModel>($"Select * from {nameof(SifaTransferOptionModel)} where Username='@_name'", new { _name = _username }).ConfigureAwait(true);
            return topt.FirstOrDefault();
        }

        public async Task<int> UpdateOption(SifaTransferOptionModel transferOptModel)
        {
            return await _connection.UpdateAsync(transferOptModel).ConfigureAwait(true);
        }
        public async Task<int> DeleteAllOption<T>(string _username)
        {
            // return await _connection.DeleteAllAsync<SifaTransferOptionModel>().ConfigureAwait(true);
            // Delete the record(s) matching the WHERE clause
            var dresult = await _connection.Table<SifaTransferOptionModel>().Where(x => x.Username == _username).DeleteAsync();
            return dresult;
        }
    }
}
