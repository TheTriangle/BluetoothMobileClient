﻿using BluetoothMobileClient;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;

[Register("AppDelegate")]
public class AppDelegate : FormsApplicationDelegate
{
    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        Xamarin.Forms.Forms.Init();

        LoadApplication(new App());

        return base.FinishedLaunching(application, launchOptions);
    }
}