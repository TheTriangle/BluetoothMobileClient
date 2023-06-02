using Android.Bluetooth;
using Android.Util;
using BluetoothConnectionAndroidLibrary.Services;
using BluetoothConnectionLibrary.Services;
using Java.Util;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(BluetoothConnectionService))]
namespace BluetoothConnectionAndroidLibrary.Services
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