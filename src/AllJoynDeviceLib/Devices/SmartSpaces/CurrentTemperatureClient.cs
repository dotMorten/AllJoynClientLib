using System;
using System.ComponentModel;
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

        private IProperty _currentValueProperty;

        private event EventHandler<double> _currentValueChanged;

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
