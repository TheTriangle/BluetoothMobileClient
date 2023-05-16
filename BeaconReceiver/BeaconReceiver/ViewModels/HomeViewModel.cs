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

namespace BluetoothMobileClient.ViewModels
{
    public class HomeViewModel
    {
        bool connectOnDiscover = false;
        string MAC;
        string PIN;
        public async Task RequestPermissions()
        {
            RequestLocationPermission();
        }

        private async Task RequestLocationPermission()
        {
            App.permissionService.RequestPermissions(() => {
                Console.WriteLine("Permissions received");
            });
        }

        public async Task<bool> ConnectToDesktop(string MAC, string PIN)
        {
            Console.WriteLine("Connecting to " + ParseMAC(MAC));
            bool success = App.bluetoothConnectionService.ConnectToDevice(ParseMAC(MAC), PIN);
            return success;
        }

        public void DisconnectDesktop(string MAC)
        {
            App.bluetoothConnectionService.DisconnectDevice(MAC);
        }

        string ParseMAC(string givenMAC)
        {
            givenMAC = givenMAC.ToUpper();
            if (givenMAC.Length == 17) return givenMAC;
            return givenMAC.Substring(0, 2) + ':' + givenMAC.Substring(2, 2) + ':' + givenMAC.Substring(4, 2) + ':' + 
                   givenMAC.Substring(6, 2) + ':' + givenMAC.Substring(8, 2) + ':' + givenMAC.Substring(10, 2);
        }

        internal void Communicate(Label lblReceivedData)
        {
            Console.WriteLine("Start comm");
            try
            {
                while (App.bluetoothConnectionService.IsDeviceConnected(MAC))
                {
                    byte[] buffer = new byte[2048]; // read in chunks of 2KB
                    App.bluetoothConnectionService.InputStream.Read(buffer, 0, buffer.Length);
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            lblReceivedData.Text += System.Text.Encoding.UTF8.GetString(buffer) + "\n";
                        });
                    }
                    Task.Delay(1000).Wait();
                    byte[] message = System.Text.Encoding.UTF8.GetBytes("Hello from mobile!");
                    Console.WriteLine("Hello sent");

                    App.bluetoothConnectionService.OutputStream.Write(message, 0, message.Length);
                    App.bluetoothConnectionService.OutputStream.Flush();
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Connection lost");
            Device.BeginInvokeOnMainThread(() =>
            {
                lblReceivedData.Text += "\nConnection lost.";
            });
        }
    }
}
