using BluetoothConnectionLibrary.Services;
using CoreBluetooth;
using System.Diagnostics;
using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using System.Linq;
using BluetoothConnectionLibrary.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(BluetoothConnectionService))]
namespace BluetoothConnectionLibrary.iOS.Services
{
    public class BluetoothConnectionService : IBluetoothConnectionService
    {
        private const int ConnectionTimeout = 10000;
        private readonly CBCentralManager manager = new CBCentralManager();

        public EventHandler<CBPeripheralEventArgs> DiscoveredDevice;
        public EventHandler StateChanged;

        public Stream InputStream { get; set; }

        public Stream OutputStream { get; set; }

        public BluetoothConnectionService()
        {
            //Console.WriteLine("Constructor^^^");
            this.manager.DiscoveredPeripheral += this.DiscoveredPeripheral;
            this.manager.UpdatedState += this.UpdatedState;
            _ = Scan(100000);
            ////Console.WriteLine("Constructorvvv");
        }

        public void Dispose()
        {
            //Console.WriteLine("Dispose^^^");
            this.manager.DiscoveredPeripheral -= this.DiscoveredPeripheral;
            this.manager.UpdatedState -= this.UpdatedState;
            this.StopScan();
            //Console.WriteLine("Disposevvv");
        }

        public async Task Scan(int scanDuration, string serviceUuid = null)
        {
            Debug.WriteLine("Scanning started");
            var uuids = string.IsNullOrEmpty(serviceUuid)
                ? new CBUUID[0]
                : new[] { CBUUID.FromString(serviceUuid) };
            this.manager.ScanForPeripherals(new CBUUID[0]);
            Debug.WriteLine("Awaiting scan");
            await Task.Delay(scanDuration);
            Debug.WriteLine("Stopping scan");
            this.StopScan();
            //Console.WriteLine("Scanvvv");
        }

        public void StopScan()
        {
            Debug.WriteLine("Stopping scan^^^");
            this.manager.StopScan();
            Debug.WriteLine("Stopping scanvvv");
        }

        public async Task ConnectTo(CBPeripheral peripheral)
        {
            //Console.WriteLine("Connectto^^^");
            var taskCompletion = new TaskCompletionSource<bool>();
            var task = taskCompletion.Task;
            EventHandler<CBPeripheralEventArgs> connectedHandler = (s, e) =>
            {
                if (e.Peripheral.Identifier?.ToString() == peripheral.Identifier?.ToString())
                {
                    taskCompletion.SetResult(true);
                }
            };

            try
            {
                this.manager.ConnectedPeripheral += connectedHandler;
                this.manager.ConnectPeripheral(peripheral);
                await this.WaitForTaskWithTimeout(task, ConnectionTimeout);
                Debug.WriteLine($"Bluetooth device connected = {peripheral.Name}");
            }
            finally
            {
                this.manager.ConnectedPeripheral -= connectedHandler;
            }
            //Console.WriteLine("Connecttovvv");
        }

        public void Disconnect(CBPeripheral peripheral)
        {
            //Console.WriteLine("Disconnect^^^");
            this.manager.CancelPeripheralConnection(peripheral);
            Debug.WriteLine($"Device {peripheral.Name} disconnected");
            //Console.WriteLine("Disconnectvvv");
        }

        public CBPeripheral[] GetConnectedDevices(string serviceUuid)
        {
            //Console.WriteLine("getconnectedooo");
            return this.manager.RetrieveConnectedPeripherals(new[] { CBUUID.FromString(serviceUuid) });
        }

        public async Task<CBService> GetService(CBPeripheral peripheral, string serviceUuid)
        {
            //Console.WriteLine("getservice^^^");
            var service = this.GetServiceIfDiscovered(peripheral, serviceUuid);
            if (service != null)
            {
                return service;
            }

            var taskCompletion = new TaskCompletionSource<bool>();
            var task = taskCompletion.Task;
            EventHandler<NSErrorEventArgs> handler = (s, e) =>
            {
                if (this.GetServiceIfDiscovered(peripheral, serviceUuid) != null)
                {
                    taskCompletion.SetResult(true);
                }
            };

            try
            {
                peripheral.DiscoveredService += handler;
                peripheral.DiscoverServices(new[] { CBUUID.FromString(serviceUuid) });
                await this.WaitForTaskWithTimeout(task, ConnectionTimeout);
                return this.GetServiceIfDiscovered(peripheral, serviceUuid);
            }
            finally
            {
                peripheral.DiscoveredService -= handler;
                //Console.WriteLine("getservicevvv");
            }
        }

        public CBService GetServiceIfDiscovered(CBPeripheral peripheral, string serviceUuid)
        {
            //Console.WriteLine("GetServiceifd");
            serviceUuid = serviceUuid.ToLowerInvariant();
            return peripheral.Services
                ?.FirstOrDefault(x => x.UUID?.Uuid?.ToLowerInvariant() == serviceUuid);
            //Console.WriteLine("GetServiceifd exited");
        }

        public async Task<CBCharacteristic[]> GetCharacteristics(CBPeripheral peripheral, CBService service, int scanTime)
        {
            peripheral.DiscoverCharacteristics(service);
            await Task.Delay(scanTime);
            return service.Characteristics;
        }

        public async Task<NSData> ReadValue(CBPeripheral peripheral, CBCharacteristic characteristic)
        {
            var taskCompletion = new TaskCompletionSource<bool>();
            var task = taskCompletion.Task;
            EventHandler<CBCharacteristicEventArgs> handler = (s, e) =>
            {
                if (e.Characteristic.UUID?.Uuid == characteristic.UUID?.Uuid)
                {
                    taskCompletion.SetResult(true);
                }
            };

            try
            {
                peripheral.UpdatedCharacterteristicValue += handler;
                peripheral.ReadValue(characteristic);
                await this.WaitForTaskWithTimeout(task, ConnectionTimeout);
                return characteristic.Value;
            }
            finally
            {
                peripheral.UpdatedCharacterteristicValue -= handler;
            }
        }

        public async Task<NSError> WriteValue(CBPeripheral peripheral, CBCharacteristic characteristic, NSData value)
        {
            var taskCompletion = new TaskCompletionSource<NSError>();
            var task = taskCompletion.Task;
            EventHandler<CBCharacteristicEventArgs> handler = (s, e) =>
            {
                if (e.Characteristic.UUID?.Uuid == characteristic.UUID?.Uuid)
                {
                    taskCompletion.SetResult(e.Error);
                }
            };

            try
            {
                peripheral.WroteCharacteristicValue += handler;
                peripheral.WriteValue(value, characteristic, CBCharacteristicWriteType.WithResponse);
                await this.WaitForTaskWithTimeout(task, ConnectionTimeout);
                return task.Result;
            }
            finally
            {
                peripheral.WroteCharacteristicValue -= handler;
            }
        }

        private void DiscoveredPeripheral(object sender, CBDiscoveredPeripheralEventArgs args)
        {
            Debug.WriteLine($"Discovered periferal!");
            var device = $"{args.Peripheral.Name} - {args.Peripheral.Identifier?.Description}";
            Debug.WriteLine($"Discovered {device}");
            this.DiscoveredDevice?.Invoke(sender, new CBPeripheralEventArgs(args.Peripheral));
        }

        private void UpdatedState(object sender, EventArgs args)
        {
            Debug.WriteLine($"Updated state!");
            Debug.WriteLine($"State = {this.manager.State}");
            this.StateChanged?.Invoke(sender, args);
        }

        private async Task WaitForTaskWithTimeout(Task task, int timeout)
        {
            //Console.WriteLine("WaitForTaskWithTimeout");
            await Task.WhenAny(task, Task.Delay(ConnectionTimeout));
            if (!task.IsCompleted)
            {
                throw new TimeoutException();
            }
            //Console.WriteLine("WaitForTaskWithTimeout exited");
        }

        public bool ConnectToDevice(string MAC, string PIN)
        {
            //throw new NotImplementedException();
            return false;
        }

        public void DisconnectDevice(string MAC)
        {
            //throw new NotImplementedException();
        }

        public bool IsDeviceConnected(string MAC)
        {
            //throw new NotImplementedException();
            return false;
        }
    }
}
