using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using OpenNETCF.IoC;
using UniversalBeacon.Library.Core.Entities;
using UniversalBeacon.Library.Core.Interfaces;
using Xamarin.Forms;

namespace BeaconReceiver.Models
{
    internal class BeaconService : IDisposable
    {
        private readonly BeaconManager _manager;
        private readonly IBluetoothPacketProvider _provider;

        public BeaconService()
        {
            // get the platform-specific provider
            _provider = RootWorkItem.Services.Get<IBluetoothPacketProvider>();

            if(null != _provider) {
                // create a beacon manager, giving it an invoker to marshal collection changes to the UI thread
                _manager = new BeaconManager(_provider, Device.BeginInvokeOnMainThread);
                _manager.Start();
                #if DEBUG
                _manager.BeaconAdded += _manager_BeaconAdded;
                _provider.AdvertisementPacketReceived += Provider_AdvertisementPacketReceived;
                #endif // DEBUG
            }
        }

        public void Dispose()
        {
            _manager?.Stop();
        }

        public ObservableCollection<Beacon> Beacons => _manager?.BluetoothBeacons;
        public IBluetoothPacketProvider Provider => _provider;

#if DEBUG
        void _manager_BeaconAdded(object sender, Beacon e)
        {
            Debug.WriteLine($"_manager_BeaconAdded {sender} Beacon {e}");
        }

        void Provider_AdvertisementPacketReceived(object sender, UniversalBeacon.Library.Core.Interop.BLEAdvertisementPacketArgs e)
        {
            Debug.WriteLine($"Provider_AdvertisementPacketReceived {sender} Beacon {e}");
        }
#endif // DEBUG
    }
}
