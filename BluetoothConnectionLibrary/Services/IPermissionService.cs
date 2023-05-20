using System;
using System.Collections.Generic;
using System.Text;

namespace BluetoothConnectionLibrary.Services
{
    public interface IPermissionService
    {
        bool HasPermissions();
        void RequestPermissions(Action bluetoothAction);
    }
}
