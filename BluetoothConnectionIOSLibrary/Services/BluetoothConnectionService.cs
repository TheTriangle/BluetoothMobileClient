using BluetoothConnectionAndroidLibrary.Services;
using BluetoothConnectionLibrary.Services;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(BluetoothConnectionService))]
namespace BluetoothConnectionAndroidLibrary.Services
{
    public class BluetoothConnectionService : IBluetoothConnectionService
    {
        Stream inputStream;
        Stream outputStream;
        public Stream InputStream { get { return inputStream; } }
        public Stream OutputStream { get { return outputStream; } }
        public bool ConnectToDevice(string MAC, string PIN)
        {
            throw new System.NotImplementedException();
        }

        public void DisconnectDevice(string MAC)
        {
            throw new System.NotImplementedException();
        }

        public bool IsDeviceConnected(string MAC)
        {
            throw new System.NotImplementedException();
        }
    }
}