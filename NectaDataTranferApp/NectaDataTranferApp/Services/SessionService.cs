using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using NectaDataTransfer.Shared.Models;

namespace NectaDataTransfer.Services
{
    public class SessionService
	{
		private readonly ProtectedSessionStorage _sessionStorage;

		public SessionService(ProtectedSessionStorage storage)
		{
			_sessionStorage = storage;
		}

		public async Task<UserDto> GetUserSession(string mykey)
		{
			try
			{
				ProtectedBrowserStorageResult<UserDto> value = await _sessionStorage.GetAsync<UserDto>(mykey);
				return value.Value;
			}
			catch (Exception)
			{
				return null;
			}

		}

		public async Task SetUserSession(string mykey, UserDto value)
		{
			await _sessionStorage.SetAsync(mykey, value);
		}
		public async Task DeleteUserSession(string mykey)
		{
			try
			{
				await _sessionStorage.DeleteAsync(mykey);
			}
			catch (Exception)
			{


			}

		}
	}
}
