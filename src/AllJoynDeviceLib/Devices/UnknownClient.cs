namespace AllJoynClientLib.Devices
{
    /// <summary>
    /// Unknown Device.
    /// </summary>
    /// <seealso cref="DeviceManager.TrackUnknownDevices" />
    public class UnknownClient : DeviceClient
    {
        internal UnknownClient(DeviceProviders.IService service) : base(service)
        {
        }
    }
}
