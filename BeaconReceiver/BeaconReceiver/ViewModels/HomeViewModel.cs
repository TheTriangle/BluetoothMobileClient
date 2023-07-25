using OpenNETCF.IoC;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using UniversalBeacon.Library.Core.Entities;
using Xamarin.Essentials;
using System.Reflection;
using Xamarin.Forms.PlatformConfiguration;
using System.Text;
using Xamarin.Forms;
using System.IO;
using BluetoothConnectionLibrary.Permissions;

namespace BluetoothMobileClient.ViewModels
{
    public class HomeViewModel
    {
        public async Task RequestPermissions()
        {
            RequestLocationPermission();
        }


        private async Task RequestLocationPermission()
        {
            Console.WriteLine("Requesting permissions");
            PermissionManager.RequestPermissions(() => {
                Console.WriteLine("Permissions received");
            });
            Console.WriteLine("Requesting permissions method exit");
        }
    }
}
