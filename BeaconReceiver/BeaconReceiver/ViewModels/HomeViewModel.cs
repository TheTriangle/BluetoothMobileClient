using OpenNETCF.IoC;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using UniversalBeacon.Library.Core.Entities;
using BeaconReceiver.Models;
using Xamarin.Essentials;
using System.Reflection;
using Xamarin.Forms.PlatformConfiguration;
using System.Text;

namespace BeaconReceiver.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BeaconService _service;
        public ObservableCollection<Beacon> Beacons => _service?.Beacons;
        private Beacon _selectedBeacon;
        
        public async Task RequestPermissions()
        {
            RequestLocationPermission();
        }

        private async Task RequestLocationPermission()
        {
            App.permissionService.RequestPermissions(() => { StartBeaconService(); });
            if (!App.permissionService.HasPermissions())
            {
                App.permissionService.RequestPermissions(() => {
                    StartBeaconService();
                });
            }
        }


        private void StartBeaconService()
        {
            _service = RootWorkItem.Services.Get<BeaconService>();
            if (_service == null)
            {
                _service = RootWorkItem.Services.AddNew<BeaconService>();
                if (_service.Beacons != null) _service.Beacons.CollectionChanged += Beacons_CollectionChanged;
                _service.Provider.AdvertisementPacketReceived += Provider_AdvertisementPacketReceived;
            }
        }

        void Provider_AdvertisementPacketReceived(object sender, UniversalBeacon.Library.Core.Interop.BLEAdvertisementPacketArgs e)
        {
            Debug.WriteLine("Advertisement: ");
            Debug.WriteLine("Manufacturer company id: ");
            Debug.WriteLine(e.Data.Advertisement.ManufacturerData[0].CompanyId);
            Debug.WriteLine("Data sections: " + e.Data.Advertisement.DataSections.Count);
            foreach (var section in e.Data.Advertisement.DataSections)
            {
                Debug.WriteLine(ByteArrayToString(section.Data));
            }
            Debug.WriteLine("Service UUIDs: " + e.Data.Advertisement.ServiceUuids.Count);
            foreach (var section in e.Data.Advertisement.ServiceUuids)
            {
                Debug.WriteLine(section.ToString());
            }
            Debug.WriteLine("end");
        }

        string ByteArrayToString(byte[] data)
        {
            StringBuilder hex = new StringBuilder(data.Length * 2);
            foreach (byte b in data)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private void Beacons_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine($"Beacons_CollectionChanged {sender} e {e}");
            foreach (var beacon in Beacons)
            {
                Debug.WriteLine("RSSI: " + beacon.Rssi + " beacon: ");
                foreach (var frame in beacon.BeaconFrames)
                {
                    Debug.WriteLine(ByteArrayToString(frame.Payload));
                }
            }
        }

        public Beacon SelectedBeacon
        {
            get => _selectedBeacon;
            set
            {
                _selectedBeacon = value;
                PropertyChanged.Fire(this, "SelectedBeacon");
            }
        }


    }
}
