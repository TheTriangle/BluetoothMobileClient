using BluetoothMobileClient.ViewModels;
using BluetoothMobileClient.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BluetoothMobileClient
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HomeView), typeof(HomeView));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//HomeView");
        }
    }
}
