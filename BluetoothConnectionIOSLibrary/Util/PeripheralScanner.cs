using CoreBluetooth;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace BluetoothConnectionIOSLibrary.Util
{
    public class PeripheralScanner
    {
        private readonly CBCentralManager manager;
        private List<CBPeripheral> foundPeripherals; // IPeripheral wraps CBPeripheral or BluetoothDevice

        public PeripheralScanner()
        {
            this.foundPeripherals = new List<CBPeripheral>();

            this.manager = new CBCentralManager();
            this.manager.DiscoveredPeripheral += this.discoveredPeripheral;
        }

        public async Task<List<CBPeripheral>> ScanForService(string serviceUuid)
        {
            return await this.ScanForService(serviceUuid, 100000);
        }

        public async Task<List<CBPeripheral>> ScanForService(string serviceUuid, int duration)
        {
            if (this.manager.IsScanning)
            {
                this.manager.StopScan();
            }
            this.manager.ScanForPeripherals(CBUUID.FromString(serviceUuid));
            await Task.Delay(duration);
            this.manager.StopScan();

            return this.foundPeripherals;
        }

        private void discoveredPeripheral(object sender, CBDiscoveredPeripheralEventArgs args)
        {
            CBPeripheral cbperipheral = args.Peripheral;
            bool isDiscovered = false;
            foreach (CBPeripheral peripheral in this.foundPeripherals)
            {
                if ((peripheral).Identifier == cbperipheral.Identifier)
                {
                    isDiscovered = true;
                    break;
                }
            }
            if (false == isDiscovered)
            {
                this.foundPeripherals.Add(cbperipheral);
            }
        }
    }
}