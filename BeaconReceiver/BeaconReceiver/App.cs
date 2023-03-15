using BeaconReceiver.Services;
using BeaconReceiver.Views;
using Xamarin.Forms;

namespace BeaconReceiver
{
    public partial class App : Application
    {
        private readonly HomeView _viewInstance;
        public static IPermissionService permissionService;

        public App()
        {

            _viewInstance = new HomeView();
            MainPage = _viewInstance;
        }
        public static void Init(IPermissionService permissionServiceImpl)
        {
            App.permissionService = permissionServiceImpl;
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
