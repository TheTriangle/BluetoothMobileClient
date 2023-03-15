using BeaconReceiver.ViewModels;
using BeaconReceiver.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BeaconReceiver
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
