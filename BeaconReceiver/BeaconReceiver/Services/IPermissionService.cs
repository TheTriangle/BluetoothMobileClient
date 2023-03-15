using System;
using System.Collections.Generic;
using System.Text;

namespace BeaconReceiver.Services
{
    public interface IPermissionService
    {
        bool HasPermissions();
        void RequestPermissions(Action bluetoothAction);
    }
}
