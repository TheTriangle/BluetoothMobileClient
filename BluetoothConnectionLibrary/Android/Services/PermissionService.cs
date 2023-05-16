using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using BluetoothMobileClient.Services;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace BluetoothMobileClient.Droid.Services
{
    internal class PermissionService : IPermissionService
    {
        public bool HasPermissions()
        {
            Log.Debug("BluetoothPermissions", "" + (ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity, 
                Android.Manifest.Permission.BluetoothScan) == Permission.Granted) + "" +
                (ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                Android.Manifest.Permission.AccessFineLocation) == Permission.Granted) + "" +
                (ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                Android.Manifest.Permission.AccessCoarseLocation) == Permission.Granted) + "" +
                (ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                Android.Manifest.Permission.BluetoothConnect) == Permission.Granted) + "" +
                //ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                //Android.Manifest.Permission.BluetoothPrivileged) == Permission.Granted &&
                (ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                Android.Manifest.Permission.BluetoothAdmin) == Permission.Granted));

            return //ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                //Android.Manifest.Permission.BluetoothScan) == Permission.Granted &&
                ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                Android.Manifest.Permission.AccessFineLocation) == Permission.Granted &&
                ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                Android.Manifest.Permission.AccessCoarseLocation) == Permission.Granted &&
                //ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                //Android.Manifest.Permission.BluetoothConnect) == Permission.Granted &&
                //ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                //Android.Manifest.Permission.BluetoothPrivileged) == Permission.Granted &&
                ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                Android.Manifest.Permission.BluetoothAdmin) == Permission.Granted;
        }

        public async void RequestPermissions(Action callbackAction)
        {
            var permissions = new string[] { Manifest.Permission.AccessFineLocation, 
                Manifest.Permission.AccessCoarseLocation, Manifest.Permission.BluetoothScan, 
                Manifest.Permission.BluetoothConnect, Manifest.Permission.Bluetooth,
                Manifest.Permission.BluetoothAdmin, Manifest.Permission.BluetoothPrivileged };
            ActivityCompat.RequestPermissions(CrossCurrentActivity.Current.Activity, permissions, 1);
            while (!HasPermissions())
            {
                Thread.Sleep(100);
            }
            callbackAction?.Invoke();
        }

    }
}