using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using BeaconReceiver.Services;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace BeaconReceiver.Droid.Services
{
    internal class PermissionService : IPermissionService
    {
        private bool hasPermission = false;
        public bool HasPermissions()
        {
            return ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                Android.Manifest.Permission.BluetoothScan) == Permission.Granted &&
                ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                Android.Manifest.Permission.AccessFineLocation) == Permission.Granted &&
                ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                Android.Manifest.Permission.AccessCoarseLocation) == Permission.Granted &&
                ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity,
                Android.Manifest.Permission.BluetoothConnect) == Permission.Granted;

        }

        public async void RequestPermissions(Action bluetoothAction)
        {
            var permissions = new string[] { Manifest.Permission.AccessFineLocation, 
                Manifest.Permission.AccessCoarseLocation, Manifest.Permission.BluetoothScan, 
                Manifest.Permission.BluetoothConnect, Manifest.Permission.Bluetooth,
                Manifest.Permission.BluetoothAdmin };
            ActivityCompat.RequestPermissions(CrossCurrentActivity.Current.Activity, permissions, 1);
            while (!HasPermissions())
            {
            }
            bluetoothAction?.Invoke();
        }

    }
}