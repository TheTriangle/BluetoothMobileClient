using BluetoothMobileClient.iOS.Services;
using UIKit;

namespace BluetoothMobileClient.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            App.Init(new PermissionService(), new BluetoothConnectionService());
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
