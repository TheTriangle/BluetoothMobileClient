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
using BluetoothConnectionAndroidLibrary.Services;
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
            App.Init();
            CrossCurrentActivity.Current.Init(this);
        }
    }
}