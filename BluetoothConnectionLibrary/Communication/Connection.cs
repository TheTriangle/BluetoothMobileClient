using BluetoothConnectionLibrary.Services;
using BluetoothConnectionLibrary.Util;
using BluetoothConnectionLibrary.Utils;
using MvvmCross.Platform;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BluetoothConnectionLibrary.Communication
{
    public class Connection
    {
        string MAC, PIN;
        IBluetoothConnectionService bluetoothConnectionService;
        public delegate void ConnectionLost();
        public event ConnectionLost OnConnectionLost;
        public MessagingInterface Messages { get; set; }
        IDevice connectedDevice;

        public Connection(string MAC, string PIN)
        {
            if (PIN == null)
            {
                PIN = "";
            }
            this.MAC = MAC;
            this.PIN = PIN;
            var platform = CrossDeviceInfo.Current.Platform;
            switch (platform)
            {
                case Platform.iOS:
                    break;
                case Platform.Android:
                    bluetoothConnectionService = DependencyService.Get<IBluetoothConnectionService>();
                    break;
            }
        }

        public async Task<bool> Connect()
        {
            //var ble = Mvx.Resolve<IBluetoothLE>();
            //var adapter = Mvx.Resolve<IAdapter>();
            if (CrossBluetoothLE.Current == null)
            {
                Console.WriteLine("plugin null");
                return false;
            } 
            Plugin.BLE.CrossBluetoothLE.Current.StateChanged += async (o, e) =>
            {
                if (e.NewState == BluetoothState.On)
                {
                    var ble = CrossBluetoothLE.Current;
                    var adapter = CrossBluetoothLE.Current.Adapter;
                    Console.WriteLine("Ble state: " + ble.State);

                    Console.WriteLine("Mac to connect to: " + MAC);
                    var systemDevices = adapter.GetSystemConnectedOrPairedDevices();
                    Console.WriteLine("Devices:");
                    foreach (var device in systemDevices)
                    {
                        Console.WriteLine(device.Id);
                    }
                    Console.WriteLine("Starting connect to known device");
                    var parameters = new ConnectParameters(false, true);
                    CancellationTokenSource _cancellationTokenSource;
                    TimeSpan time = new TimeSpan(10, 0, 0);
                    _cancellationTokenSource = new CancellationTokenSource(time);

                    connectedDevice = await adapter.ConnectToKnownDeviceAsync(Guid.Parse("00000000-0000-0000-0000-" + MAC), parameters, _cancellationTokenSource.Token);
                    Console.WriteLine("Connected to device!");

                    var service = await connectedDevice.GetServiceAsync(Guid.Parse("00001800-0000-1000-8000-00805f9b34fb"));
                    var characteristic = await service.GetCharacteristicAsync(Guid.Parse("d8de624e-140f-4a22-8594-e2216b84a5f2"));
                    Messages = new MessagingInterface(characteristic);
                }
            };
            
            return true;
        }

        void RegisterConnectionLostListener()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    if (!IsConnected())
                    {
                        OnConnectionLost?.Invoke();
                        return;
                    }
                }
            }).Start();
        }

        public void Disconnect()
        {
            if (IsConnected()) connectedDevice.Dispose();
        }
        public bool IsConnected()
        {
            if (connectedDevice == null) return false;
            return connectedDevice.State == Plugin.BLE.Abstractions.DeviceState.Connected;
        }
    }
}
