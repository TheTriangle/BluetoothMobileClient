using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.LocalBroadcastManager.Content;
using BluetoothMobileClient.Services;
using Java.IO;
using Java.Lang.Reflect;
using Java.Lang;
using Java.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Object = Java.Lang.Object;

namespace BluetoothMobileClient.Droid.Services
{
    public class BluetoothConnectionService : IBluetoothConnectionService
    {
        BluetoothSocket desktopSocket;
        Stream inputStream;
        Stream outputStream;
        public Stream InputStream { get { return inputStream; } }
        public Stream OutputStream { get { return outputStream; } }
        public bool ConnectToDevice(string MAC, string PIN)
        {
            Log.Debug("Connecting to device: ", MAC);
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            if (!adapter.IsEnabled)
            {
                adapter.Enable();
            }
            try
            {
                BluetoothDevice desktopDevice = adapter.GetRemoteDevice(MAC);

                desktopSocket = desktopDevice.CreateInsecureRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));

                desktopDevice.SetPin(System.Text.Encoding.UTF8.GetBytes(PIN));

                desktopSocket.Connect();
                try
                {
                    inputStream = desktopSocket.InputStream;
                    outputStream = desktopSocket.OutputStream;
                }
                catch (Java.Lang.Exception ex)
                {
                    Log.Error("Bluetooth connection error", "stream");
                    ex.PrintStackTrace();
                    return false;
                }
            }
            catch (Java.Lang.Exception ex)
            {
                Log.Error("Bluetooth connection error", "outer");
                ex.PrintStackTrace();
                return false;
            }
            return true;
        }

        public void DisconnectDevice(string MAC)
        {
            if (inputStream == null ||  outputStream == null || desktopSocket == null) return;
            inputStream.Close();
            outputStream.Close();
            desktopSocket.Close();
        }

        public bool IsDeviceConnected(string MAC)
        {
            return desktopSocket.IsConnected;
        }
    }
}