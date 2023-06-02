using BluetoothMobileClient.Views;
using Xamarin.Forms;
using BluetoothConnectionLibrary;
using BluetoothConnectionLibrary.Services;

namespace BluetoothMobileClient
{
    public partial class App : Application
    {
        private readonly HomeView _viewInstance;
        public App()
        {

            _viewInstance = new HomeView();
            MainPage = _viewInstance;
        }
        public static void Init()
        {
        }


        protected override async void OnStart()
        {
            // Handle when your app starts
            await _viewInstance.Init();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
