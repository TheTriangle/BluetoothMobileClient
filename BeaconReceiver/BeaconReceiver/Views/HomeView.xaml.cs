using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using BluetoothMobileClient.ViewModels;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.Permissions.Abstractions;
using UniversalBeacon.Library.Core.Entities;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BluetoothMobileClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentPage
    {
        private readonly HomeViewModel _viewModel;
        public HomeView()
        {
            InitializeComponent();
            _viewModel = new HomeViewModel();
            SetMacAddress();
            BindingContext = _viewModel;
        }

        public void SetMacAddress()
        {
            var ni = NetworkInterface.GetAllNetworkInterfaces()
           .OrderBy(intf => intf.NetworkInterfaceType)
           .FirstOrDefault(intf => intf.OperationalStatus == OperationalStatus.Up
                 && (intf.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                     || intf.NetworkInterfaceType == NetworkInterfaceType.Ethernet));
            try
            {
                var hw = ni.GetPhysicalAddress();
                edMacAddress.Text = string.Join(":", (from ma in hw.GetAddressBytes() select ma.ToString("X2")).ToArray());
            } catch
            {
                edMacAddress.Text = "Your mac address is inaccessible";
            }
        }

        public async Task Init()
        {
            await _viewModel.RequestPermissions();
            btnConnect.Clicked += BtnConnect_Clicked;
            btnDisconnect.Clicked += BtnDisconnect_Clicked;
        }

        private void BtnDisconnect_Clicked(object sender, EventArgs e)
        {
            _viewModel.DisconnectDesktop(edDesktopMacAddress.Text);
        }

        private async void BtnConnect_Clicked(object sender, System.EventArgs e)
        {
            btnConnect.IsEnabled = false;
            bool connectionSuccessful = await _viewModel.ConnectToDesktop(edDesktopMacAddress.Text, edPIN.Text);
            if (!connectionSuccessful)
            {
                btnConnect.IsEnabled = true;
                return;
            }
            new Thread(() =>
            {
                _viewModel.Communicate(lblReceivedData);
                Device.BeginInvokeOnMainThread(() =>
                {
                    btnConnect.IsEnabled = true;
                });
            }).Start();
        }
    }
}