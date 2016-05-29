using DeviceProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllJoynClientLib.Devices.LSF
{
    public partial class LightClient 
    {
        private IInterface lampDetails = null;

        //We cache the tasks since these values shouldn't change
        private Task<bool> hasEffects;
        private Task<bool> _isDimmable;
        private Task<bool> _isColor;
        private Task<bool> _isVariableColorTemp;
        private Task<UInt32> _minTemperature;
        private Task<UInt32> _maxTemperature;

        public Task<bool> GetHasEffectsAsync()
        {
            if(hasEffects == null)
            {
                if (lampDetails == null)
                    hasEffects = Task.FromResult(false);
                else
                    hasEffects = lampDetails.GetPropertyAsync<bool>("HasEffects");
            }
            return hasEffects;
        }

        public Task<bool> GetIsDimmableAsync()
        {
            if (_isDimmable == null)
            {
                if (lampDetails == null)
                    _isDimmable = Task.FromResult(true);
                else
                    _isDimmable = lampDetails.GetPropertyAsync<bool>("Dimmable");
            }
            return _isDimmable;
        }

        public Task<bool> GetIsColorAsync()
        {
            if (_isColor == null)
            {
                if (lampDetails == null)
                    _isColor = Task.FromResult(true);
                else
                    _isColor = lampDetails.GetPropertyAsync<bool>("Color");
            }
            return _isColor;
        }
        public Task<bool> GetIsVariableColorTempAsync()
        {
            if (_isVariableColorTemp == null)
            {
                if (lampDetails == null)
                    _isVariableColorTemp = Task.FromResult(true);
                else
                    _isVariableColorTemp = lampDetails.GetPropertyAsync<bool>("VariableColorTemp");
            }
            return _isVariableColorTemp;
        }

        public Task<UInt32> GetMinTemperatureAsync()
        {
            if (_minTemperature == null)
            {
                if (lampDetails == null)
                    _minTemperature = Task.FromResult<UInt32>(2500);
                else
                    _minTemperature = lampDetails.GetPropertyAsync<UInt32>("MinTemperature");
            }
            return _minTemperature;
        }
        public Task<UInt32> GetMaxTemperatureAsync()
        {
            if (_maxTemperature == null)
            {
                if (lampDetails == null)
                    _maxTemperature = Task.FromResult<UInt32>(9000);
                else
                    _maxTemperature = lampDetails.GetPropertyAsync<UInt32>("MaxTemperature");
            }
            return _maxTemperature;
        }
    }
}
