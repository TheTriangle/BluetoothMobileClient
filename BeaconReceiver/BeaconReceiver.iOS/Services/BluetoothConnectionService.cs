using Foundation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UIKit;

namespace BluetoothMobileClient.iOS.Services
{
    internal class BluetoothConnectionService// : IBluetoothConnectionService
    {
        public Stream InputStream => throw new NotImplementedException();

        public Stream OutputStream => throw new NotImplementedException();

        public event EventHandler<string> DeviceDiscovered;

        public bool ConnectToDevice(string MAC, string PIN)
        {
            throw new NotImplementedException();
        }

        public void DisconnectDevice(string MAC)
        {
            throw new NotImplementedException();
        }

        public bool IsDeviceConnected(string MAC)
        {
            throw new NotImplementedException();
        }
    }
}