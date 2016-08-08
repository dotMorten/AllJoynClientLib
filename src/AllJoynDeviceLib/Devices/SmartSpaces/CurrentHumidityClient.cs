using System.Threading.Tasks;
using DeviceProviders;

namespace AllJoynClientLib.Devices.SmartSpaces
{
    /// <summary>
    /// A SmartSpaces Current Relative Humidity sensor
    /// </summary>
    /// <seealso cref="AllJoynClientLib.Devices.DeviceClient" />
    public class CurrentHumidityClient : DeviceClient
    {
        private IInterface iface = null;

        internal CurrentHumidityClient(DeviceProviders.IService service) : base(service)
        {
            iface = GetInterface("org.alljoyn.SmartSpaces.Environment.CurrentHumidity");
        }

        /// <summary>
        /// Current relative humidity value
        /// </summary>
        /// <returns>The current relative humidity expressed in percent.</returns>
        public Task<double> GetCurrentValueAsync()
        {
            return iface.GetPropertyAsync<double>("CurrentValue");
        }

        /// <summary>
        /// Gets the maximum value allowed for represented relative humidity
        /// </summary>
        /// <returns>Maximum value allowed for represented relative humidity.</returns>
        public Task<double> GetMaxValueAsync()
        {
            return iface.GetPropertyAsync<double>("MaxValue");
        }
    }
}
