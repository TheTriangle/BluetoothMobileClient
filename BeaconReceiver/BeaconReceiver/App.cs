using BluetoothMobileClient.Views;
using Xamarin.Forms;
using BluetoothConnectionLibrary;
using BluetoothConnectionLibrary.Services;

namespace BluetoothMobileClient
{
    public partial class App : Application
    {
        private readonly HomeView _viewInstance;
        public static IPermissionService permissionService;
        public static IBluetoothConnectionService bluetoothConnectionService;
        public App()
        {

            _viewInstance = new HomeView();
            MainPage = _viewInstance;
        }
        public static void Init(IPermissionService permissionServiceImpl, IBluetoothConnectionService connectionServiceImpl)
        {
            App.permissionService = permissionServiceImpl;
            App.bluetoothConnectionService = connectionServiceImpl;
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
