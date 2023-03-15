using BeaconReceiver.iOS.Services;
using UIKit;

namespace BeaconReceiver.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            App.Init(new PermissionService());
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
