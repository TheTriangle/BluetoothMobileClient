using Plugin.DeviceInfo.Abstractions;
using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Text;
using BluetoothConnectionLibrary.Services;
using Xamarin.Forms;

namespace BluetoothConnectionLibrary.Permissions
{
    public class PermissionManager
    {
        static IPermissionService permissionService;
        public static bool HasPermissions()
        {
            return permissionService.HasPermissions();
        }
        
        public static void RequestPermissions(Action bluetoothAction)
        {
            permissionService.RequestPermissions(bluetoothAction);
        }

        static PermissionManager() 
        {
            var platform = CrossDeviceInfo.Current.Platform;
            switch (platform)
            {
                case Platform.iOS:
                    break;
                case Platform.Android:
                    permissionService = DependencyService.Get<IPermissionService>();
                    break;
            }
        }
    }
}
