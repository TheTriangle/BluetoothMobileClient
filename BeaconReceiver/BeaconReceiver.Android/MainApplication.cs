using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BluetoothConnectionLibrary.Android.Services;
using Plugin.CurrentActivity;

namespace BluetoothMobileClient.Droid
{
    [Application(Debuggable = true)]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            App.Init(new PermissionService(), new BluetoothConnectionService());
            CrossCurrentActivity.Current.Init(this);
        }
    }
}