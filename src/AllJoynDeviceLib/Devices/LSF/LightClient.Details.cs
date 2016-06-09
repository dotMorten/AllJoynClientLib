using System;
using System.Threading.Tasks;
using DeviceProviders;

namespace AllJoynClientLib.Devices
{
    /// <summary>
    /// Lighting Service Framework lamp client
    /// </summary>
    public partial class LightClient
    {
        private IInterface lampDetails = null;

        // We cache the tasks since these values shouldn't change
        private Task<bool> hasEffects;
        private Task<bool> _isDimmable;
        private Task<bool> _isColor;
        private Task<bool> _isVariableColorTemp;
        private Task<uint> _minTemperature;
        private Task<uint> _maxTemperature;

        /// <summary>
        /// Gets a value indicating whether this light supports effects
        /// </summary>
        /// <returns>True if it supports effects</returns>
        public Task<bool> GetHasEffectsAsync()
        {
            if (hasEffects == null)
            {
                if (lampDetails == null)
                {
                    hasEffects = Task.FromResult(false);
                }
                else
                {
                    hasEffects = lampDetails.GetPropertyAsync<bool>("HasEffects");
                }
            }

            return hasEffects;
        }

        /// <summary>
        /// Gets a value indicating whether this light supports dimming
        /// </summary>
        /// <returns>True if it supports dimming</returns>
        public Task<bool> GetIsDimmableAsync()
        {
            if (_isDimmable == null)
            {
                if (lampDetails == null)
                {
                    _isDimmable = Task.FromResult(true);
                }
                else
                {
                    _isDimmable = lampDetails.GetPropertyAsync<bool>("Dimmable");
                }
            }

            return _isDimmable;
        }

        /// <summary>
        /// Gets a value indicating whether this light supports colors
        /// </summary>
        /// <returns>True if it supports colors</returns>
        public Task<bool> GetIsColorSupportedAsync()
        {
            if (_isColor == null)
            {
                if (lampDetails == null)
                {
                    _isColor = Task.FromResult(true);
                }
                else
                {
                    _isColor = lampDetails.GetPropertyAsync<bool>("Color");
                }
            }

            return _isColor;
        }

        /// <summary>
        /// Gets a value indicating whether this light supports changing color temperature
        /// </summary>
        /// <returns>True if it supports changing color temperature</returns>
        public Task<bool> GetIsVariableColorTempAsync()
        {
            if (_isVariableColorTemp == null)
            {
                if (lampDetails == null)
                {
                    _isVariableColorTemp = Task.FromResult(true);
                }
                else
                {
                    _isVariableColorTemp = lampDetails.GetPropertyAsync<bool>("VariableColorTemp");
                }
            }

            return _isVariableColorTemp;
        }

        /// <summary>
        /// Gets the warmest color temperature this bulb supports
        /// </summary>
        /// <returns>Minimum temperature</returns>
        public Task<uint> GetMinTemperatureAsync()
        {
            if (_minTemperature == null)
            {
                if (lampDetails == null)
                {
                    _minTemperature = Task.FromResult<uint>(2500);
                }
                else
                {
                    _minTemperature = lampDetails.GetPropertyAsync<uint>("MinTemperature");
                }
            }

            return _minTemperature;
        }

        /// <summary>
        /// Gets the coldest color temperature this bulb supports
        /// </summary>
        /// <returns>Maximum temperature</returns>
        public Task<uint> GetMaxTemperatureAsync()
        {
            if (_maxTemperature == null)
            {
                if (lampDetails == null)
                {
                    _maxTemperature = Task.FromResult<uint>(9000);
                }
                else
                {
                    _maxTemperature = lampDetails.GetPropertyAsync<uint>("MaxTemperature");
                }
            }

            return _maxTemperature;
        }
    }
}
