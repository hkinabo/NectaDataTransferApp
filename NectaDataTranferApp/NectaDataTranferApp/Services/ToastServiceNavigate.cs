using Microsoft.AspNetCore.Components;

namespace NectaDataTranferApp.Services
{
    public class ToastServiceNavigate
    {
        private readonly NavigationManager _navigationManager;

        public ToastServiceNavigate(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public event Action<string, string,int, bool,string> OnShow;
        public event Action OnHide;

        public void ShowToast(string message, string heading, bool navigate = false,int timeout=2000, string navigateTo = "")
        {
            OnShow?.Invoke(message, heading,timeout, navigate,navigateTo);
            if (!navigate && !string.IsNullOrEmpty(navigateTo))
            {
                Task.Delay(timeout).ContinueWith(_ => // Adjust delay as needed
                {
                    _navigationManager.NavigateTo(navigateTo);
                });
            }
        }

        public void HideToast()
        {
            OnHide?.Invoke();
        }
    }
}
