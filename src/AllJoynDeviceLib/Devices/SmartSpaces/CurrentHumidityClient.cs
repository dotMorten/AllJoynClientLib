using System.Threading.Tasks;
using DeviceProviders;
using System;

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

        private IProperty _currentValueProperty;

#pragma warning disable SA1300 // Code analyzer bug
        private event EventHandler<double> _currentValueChanged;
#pragma warning restore SA1300 // Code analyzer bug

        /// <summary>
        /// Raised when the current value has changed
        /// </summary>
        public event EventHandler<double> CurrentValueChanged
        {
            add
            {
                if (_currentValueProperty == null)
                {
                    _currentValueProperty = iface.GetProperty("CurrentValue");
                    _currentValueProperty.ValueChanged += CurrentTemperatureClient_ValueChanged;
                }

                _currentValueChanged += value;
            }

            remove
            {
                _currentValueChanged -= value;
                if (_currentValueChanged == null && _currentValueProperty != null)
                {
                    _currentValueProperty.ValueChanged -= CurrentTemperatureClient_ValueChanged;
                    _currentValueProperty = null;
                }
            }
        }

        private void CurrentTemperatureClient_ValueChanged(IProperty sender, object args)
        {
            _currentValueChanged?.Invoke(this, (double)args);
        }

    }
}
