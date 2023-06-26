using BluetoothConnectionLibrary.Services;
using BluetoothConnectionLibrary.Util;
using BluetoothConnectionLibrary.Utils;
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

        public Connection(string MAC, string PIN)
        {
            if (PIN == null)
            {
                PIN = "";
            }
            this.MAC = MAC;
            this.PIN = PIN;
            bluetoothConnectionService = DependencyService.Get<IBluetoothConnectionService>();
            Console.WriteLine("Constructor service null: " + (bluetoothConnectionService == null));
        }

        public bool Connect()
        {
            Console.WriteLine("bluetoothConnectionService is null: " + (bluetoothConnectionService == null));
            if (bluetoothConnectionService == null) return false;
            bool success = bluetoothConnectionService.ConnectToDevice(ConnectionUtils.ParseMAC(MAC), PIN);
            RegisterConnectionLostListener();
            if (success)
            {
                Messages = new MessagingInterface(bluetoothConnectionService.InputStream, bluetoothConnectionService.OutputStream);
            }
            return success;
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
            bluetoothConnectionService.DisconnectDevice(MAC);
        }
        public bool IsConnected()
        {
            return bluetoothConnectionService.IsDeviceConnected(MAC);
        }
    }
}
