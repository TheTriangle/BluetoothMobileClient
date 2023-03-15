# BeaconReceiver
Receiving bluetooth beacons of general, eddystone and IBeacon format

Use `IPermissionService` to request bluetooth and location permimssions from user. Don't forget to remind user to turn on their geolocation. Automatic check for that is coming in the next update

Once permissions are received, use `BeaconService` to run a service scanning for beacons. Use `BeaconService.Manager` to add listeners for new beacons appearing, use `BeaconService.Provider` to add listeners accepting advertisements appearing.
