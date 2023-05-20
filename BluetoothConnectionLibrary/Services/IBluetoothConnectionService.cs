using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace BluetoothConnectionLibrary.Services
{
    public interface IBluetoothConnectionService
    {
        bool ConnectToDevice(string MAC, string PIN);
        void DisconnectDevice(string MAC);
        bool IsDeviceConnected(string MAC);
        Stream InputStream { get; }
        Stream OutputStream { get; }
    }
}
