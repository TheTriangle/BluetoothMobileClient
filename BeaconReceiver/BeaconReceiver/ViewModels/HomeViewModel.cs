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
using BluetoothConnectionLibrary.Communication;

namespace BluetoothMobileClient.ViewModels
{
    public class HomeViewModel
    {
        bool connectOnDiscover = false;
        string MAC;
        string PIN;
        public Connection connection;


        public async Task RequestPermissions()
        {
            RequestLocationPermission();
        }

        public bool InitializeConnection(string MAC, string PIN)
        {
            connection = new Connection(MAC, PIN);
            return connection.Connect();
        }

        public void Disconnect()
        {
            if (connection != null)
                connection.Disconnect();
        }

        private async Task RequestLocationPermission()
        {
            PermissionManager.RequestPermissions(() => {
                Console.WriteLine("Permissions received");
            });
        }
    }
}
