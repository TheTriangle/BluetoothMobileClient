using BluetoothConnectionLibrary.Services;
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

        void RegisterConnectionLostListener()
        {
            //Console.WriteLine("RegisterConnectionLostListener ^^^");
            new Thread(() =>
            {
                //Console.WriteLine("NewThreadStart ^^^");
                Thread.CurrentThread.IsBackground = true;
                //Console.WriteLine("NewThreadStart vvv");
                while (true)
                {
                    if (!IsConnected())
                    {
                        //Console.WriteLine("Connection lost ^^^");
                        OnConnectionLost?.Invoke();
                        //Console.WriteLine("Connection lost vvv");
                        return;
                    }
                }
            }).Start();
            //Console.WriteLine("RegisterConnectionLostListener vvv");
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
