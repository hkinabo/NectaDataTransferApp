using Microsoft.JSInterop;

namespace DataTransferShared.Services
{
	public static class FileUtil
	{
		public static ValueTask<object> SaveAs(this IJSRuntime js, string filename, byte[] data)
		{
			return js.InvokeAsync<object>(
					   "saveAsFile",
					   filename,
					   Convert.ToBase64String(data));
		}
	}
}
