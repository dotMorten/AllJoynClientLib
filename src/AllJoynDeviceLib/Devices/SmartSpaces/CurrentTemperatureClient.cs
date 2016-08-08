using System.Threading.Tasks;
using DeviceProviders;

namespace AllJoynClientLib.Devices.SmartSpaces
{
    /// <summary>
    /// A SmartSpaces Current Temperature sensor
    /// </summary>
    /// <seealso cref="AllJoynClientLib.Devices.DeviceClient" />
    public class CurrentTemperatureClient : DeviceClient
    {
        private IInterface iface = null;

        internal CurrentTemperatureClient(DeviceProviders.IService service) : base(service)
        {
            iface = GetInterface("org.alljoyn.SmartSpaces.Environment.CurrentTemperature");
        }

        /// <summary>
        /// Current temperature expressed in Celsius.
        /// </summary>
        /// <returns>The current temperature expressed in Celsius.</returns>
        public Task<double> GetCurrentValueAsync()
        {
            return iface.GetPropertyAsync<double>("CurrentValue");
        }

        /// <summary>
        /// The precision of the CurrentValue property. i.e. the number of degrees Celsius the actual temperature must change before CurrentValue is updated.
        /// </summary>
        /// <returns>The current temperature expressed in Celsius.</returns>
        public Task<double> GetPrecisionAsync()
        {
            return iface.GetPropertyAsync<double>("Precision");
        }

        /// <summary>
        /// The minimum time between updates of the CurrentValue property in milliseconds.
        /// </summary>
        /// <returns>Minimum time between updates of the CurrentValue property in milliseconds.</returns>
        public Task<ushort> GetUpdateMinTimeAsync()
        {
            return iface.GetPropertyAsync<ushort>("UpdateMinTime");
        }
    }
}
