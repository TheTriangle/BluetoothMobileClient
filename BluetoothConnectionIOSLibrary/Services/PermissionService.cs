
using BluetoothConnectionIOSLibrary.Services;
using BluetoothConnectionLibrary.Services;
using CoreBluetooth;
using CoreFoundation;
using System;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(PermissionService))]
namespace BluetoothConnectionIOSLibrary.Services
{
    public class PermissionService : IPermissionService
    {
        Action _bluetoothAction = null;

        public PermissionService()
        {
            Console.WriteLine("permission service created");
        }

        public bool HasPermissions()
        {
            Console.WriteLine("Permission: ");
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                Console.WriteLine(CBCentralManager.Authorization == CBManagerAuthorization.AllowedAlways);
                return CBCentralManager.Authorization == CBManagerAuthorization.AllowedAlways;
            }
            else
            {
                return true;
            }
        }

        public void RequestPermissions(Action bluetoothAction)
        {
            _bluetoothAction = bluetoothAction;
            var myDelegate = new PermissionCBCentralManager(this);
            var centralManger = new CBCentralManager(myDelegate, DispatchQueue.MainQueue, new CBCentralInitOptions() { ShowPowerAlert = false });
        }

        internal void CurrentUpdatedState(CBCentralManager central)
        {
            _bluetoothAction?.Invoke();
        }

        public class PermissionCBCentralManager : CBCentralManagerDelegate
        {
            PermissionService permissionService = null;

            public PermissionCBCentralManager(PermissionService controller)
            {
                permissionService = controller;
            }

            public override void UpdatedState(CBCentralManager central)
            {
                permissionService.CurrentUpdatedState(central);
            }
        }
    }
}