using CoreBluetooth;
using CoreFoundation;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using BluetoothMobileClient.Services;

namespace BluetoothMobileClient.iOS.Services
{
    public class PermissionService : IPermissionService
    {
        Action _bluetoothAction = null; //Optional, if you wanted to notify user that you have performed action (allow or deny) on the permission request dialog

        public bool HasPermissions()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
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