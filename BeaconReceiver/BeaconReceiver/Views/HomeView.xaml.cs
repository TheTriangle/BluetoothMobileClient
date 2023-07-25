using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BluetoothMobileClient.Models;
using BluetoothMobileClient.ViewModels;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using UniversalBeacon.Library.Core.Entities;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace BluetoothMobileClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentPage
    {
        private readonly HomeViewModel _viewModel;
        private IAdapter _bluetoothAdapter;
        private ICharacteristic sendCharacteristic;
        private ICharacteristic receiveCharacteristic;

        public HomeView()
        {
            InitializeComponent();
            _viewModel = new HomeViewModel();
            BindingContext = _viewModel;
            CheckPermission();

            _bluetoothAdapter = CrossBluetoothLE.Current.Adapter;

            _bluetoothAdapter.DeviceDiscovered += _bluetoothAdapter_DeviceDiscovered;
            _bluetoothAdapter.DeviceConnected += _bluetoothAdapter_DeviceConnected;
        }

        private async void _bluetoothAdapter_DeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            try
            {
                await _bluetoothAdapter.ConnectToDeviceAsync(e.Device);
            } catch(Exception ex)
            {
                Console.WriteLine("Could not connect to device: " + e.Device.Id + ": " + ex.Message);
            }
        }

        private async void _bluetoothAdapter_DeviceConnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            try
            {
                var service = await e.Device.GetServiceAsync(Guid.Parse(edServiceID.Text));
                if (service != null)
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        btnSend.IsEnabled = true;
                    });
                    sendCharacteristic = await service.GetCharacteristicAsync(GattIdentifiers.GattCharacteristicSendId);
                    receiveCharacteristic = await service.GetCharacteristicAsync(GattIdentifiers.GattCharacteristicReceiveId);

                    await receiveCharacteristic.StartUpdatesAsync();

                    receiveCharacteristic.WriteAsync(UTF8Encoding.UTF8.GetBytes("01"));
                    if (receiveCharacteristic != null)
                    {
                        Console.WriteLine("register value update listener");
                        var descriptors = await receiveCharacteristic.GetDescriptorsAsync();

                        receiveCharacteristic.ValueUpdated += (o, args) =>
                        {
                            var receivedBytes = args.Characteristic.Value;
                            Console.WriteLine("Value updated: " + Encoding.UTF8.GetString(receivedBytes));
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                lblLog.Text += Encoding.UTF8.GetString(receivedBytes, 0, receivedBytes.Length) + Environment.NewLine;
                            });
                        };

                        await receiveCharacteristic.StartUpdatesAsync();
                    }

                }
            }
            catch
            {
            }
        }

        async Task<bool> CheckPermission()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

            if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    await DisplayAlert("Need location", "App needs location permission", "OK");
                }

                var status1 = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });

                var loca = status1.FirstOrDefault(x => x.Key == Permission.Location);
                if (loca.Value != null)
                    if (loca.Value == PermissionStatus.Granted) status = PermissionStatus.Granted;
            }

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Need location", "App need location permission", "OK");
                return false;
            }
            return true;
        }

        public async Task Init()
        {
            await _viewModel.RequestPermissions();
            btnConnect.Clicked += BtnConnect_Clicked;
            btnDisconnect.Clicked += BtnDisconnect_Clicked;
            btnSend.Clicked += BtnSend_Clicked;
            btnSend.IsEnabled = false;
        }

        private async void BtnSend_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sendCharacteristic != null)
                {
                    var bytes = await sendCharacteristic.WriteAsync(Encoding.ASCII.GetBytes($"{edMessage.Text}\r\n"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Device.BeginInvokeOnMainThread(() =>
                {
                    lblLog.Text += "Error sending comand to server." + Environment.NewLine;
                });
            }
        }

        private void BtnDisconnect_Clicked(object sender, EventArgs e)
        {
            _bluetoothAdapter.StopScanningForDevicesAsync();
        }

        private async void BtnConnect_Clicked(object sender, System.EventArgs e)
        {
            _bluetoothAdapter.StartScanningForDevicesAsync();
        }

        private void Connection_OnConnectionLost()
        {
            Device.BeginInvokeOnMainThread(() => { btnConnect.IsEnabled = true; });
            //Console.WriteLine("Button disabled");
        }
    }
}