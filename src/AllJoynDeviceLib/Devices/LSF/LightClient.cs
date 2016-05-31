using DeviceProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllJoynClientLib.Devices
{
    public partial class LightClient : SwitchClient
    {
        private IInterface lampState = null;

        internal LightClient(DeviceProviders.IService service) : base(service)
        {
            lampState = GetInterface("org.allseen.LSF.LampState");
            lampDetails = GetInterface("org.allseen.LSF.LampDetails");
        }

        protected override void OnDeviceLost()
        {
            if(_lampStateChangedSignal != null)
                _lampStateChangedSignal.SignalRaised -= LampStateChangedSignal_SignalRaised;
            base.OnDeviceLost();
        }

        public override Task<bool> GetOnOffAsync()
        {
            return lampState.GetPropertyAsync<bool>("OnOff");
        }

        public override Task SetOnOffAsync(bool isOn)
        {
            return lampState.SetPropertyAsync("OnOff", isOn);
        }

        private const uint MaxValue = uint.MaxValue - 1;

        /// <summary>
        /// Brightness as a value between 0 and 1
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetBrightnessAsync()
        {
            uint b = await lampState.GetPropertyAsync<uint>("Brightness").ConfigureAwait(false);
            return b / (double)MaxValue;
        }

        /// <summary>
        /// Brightness as a value between 0 and 1
        /// </summary>
        public Task SetBrightnessAsync(double brightness)
        {
            if (brightness < 0) brightness = 0;
            else if (brightness > 1) brightness = 1;
            return lampState.SetPropertyAsync("Brightness", (uint)(brightness * MaxValue));
        }

        /// <summary>
        /// Saturation as a value between 0 and 1
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetSaturationAsync()
        {
            uint s = await lampState.GetPropertyAsync<uint>("Saturation").ConfigureAwait(false);
            return s / (double)MaxValue;
        }

        /// <summary>
        /// Saturation as a value between 0 and 1
        /// </summary>
        public Task SetSaturationAsync(double saturation)
        {
            if (saturation < 0) saturation = 0;
            else if (saturation > 1) saturation = 1;
            return lampState.SetPropertyAsync("Saturation", (uint)(saturation * MaxValue));
        }

        /// <summary>
        /// Hue as a value between 0 and 360
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetHueAsync()
        {
            uint s = await lampState.GetPropertyAsync<uint>("Hue").ConfigureAwait(false);
            return s / (double)MaxValue * 360d;
        }

        /// <summary>
        /// Hue as a value between 0 and 360
        /// </summary>
        public Task SetHueAsync(double Hue)
        {
            if (Hue < 0) Hue = 0;
            else if (Hue > 360) Hue = 360;
            return lampState.SetPropertyAsync("Hue", (uint)(Hue / 360d * MaxValue));
        }

        public Task SetColorAsync(Windows.UI.Color color)
        {
            return SetColorAsync(color.R, color.G, color.B, TimeSpan.Zero);
        }

        public Task SetColorAsync(byte red, byte green, byte blue)
        {
            return SetColorAsync(red, green, blue, TimeSpan.Zero);
        }

        public async Task SetColorAsync(byte red, byte green, byte blue, TimeSpan transitionTime)
        {
            var h = GetHue(red, green, blue);
            var s = GetSaturation(red, green, blue);
            var b = GetBrightness(red, green, blue);

            if (await GetHasEffectsAsync().ConfigureAwait(false))
            {
                await TransitionLampState(TimeSpan.Zero, h, s, b, null);
            }
            else
            {
                await Task.WhenAll(new Task[] {
                    SetHueAsync(h),
                    SetSaturationAsync(s),
                    SetBrightnessAsync(b)
                }).ConfigureAwait(false);
            }
        }

        public Task TransitionLampState(TimeSpan transitionTime, double? hue, double? saturation, double? brightness, double? temperature)
        {
            var newState = new Dictionary<object, object>();
            if (hue.HasValue) newState.Add("Hue", (UInt32)(hue.Value / 360 * MaxValue));
            if (saturation.HasValue) newState.Add("Saturation", (UInt32)(saturation.Value * MaxValue));
            if (brightness.HasValue) newState.Add("Brightness", (UInt32)(brightness.Value * MaxValue));
            if (temperature.HasValue) newState.Add("ColorTemp", ((UInt32)(temperature - 1000) / 19000 * MaxValue));
            return lampState.InvokeMethodAsync("TransitionLampState", 0UL, newState.ToList(), (UInt32)transitionTime.TotalMilliseconds);
        }

        /// <summary>
        /// Temperature in Kelvin
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetTemperatureAsync()
        {
            // In AllJoyn 0 means 1000K. uint32_max-1 means 20000K.

            uint t = await lampState.GetPropertyAsync<uint>("ColorTemp").ConfigureAwait(false);
            return t / (double)MaxValue * 19000 + 1000;
        }

        /// <summary>
        /// Temperature in Kelvin
        /// </summary>
        public Task SetTemperatureAsync(double temperature)
        {
            if (temperature < 1000) temperature = 1000;
            else if (temperature > 20000) temperature = 20000;
            return lampState.SetPropertyAsync("ColorTemp", (uint)((temperature - 1000) / 19000 * MaxValue));
        }

        private EventHandler _LampStateChanged;
        private ISignal _lampStateChangedSignal;

        public event EventHandler LampStateChanged
        {
            add
            {
                if (_LampStateChanged == null)
                {
                    if(_lampStateChangedSignal == null)
                    {
                        _lampStateChangedSignal = lampState.GetSignal(nameof(LampStateChanged));
                    }
                    if(_lampStateChangedSignal != null)
                    {
                        _lampStateChangedSignal.SignalRaised += LampStateChangedSignal_SignalRaised;
                    }
                }
                _LampStateChanged += value;
            }
            remove
            {
                _LampStateChanged -= value;
                if(_LampStateChanged == null && _lampStateChangedSignal != null)
                {
                    _lampStateChangedSignal.SignalRaised -= LampStateChangedSignal_SignalRaised;
                }
            }
        }

        private void LampStateChangedSignal_SignalRaised(ISignal sender, IList<object> args)
        {
            _LampStateChanged?.Invoke(this, EventArgs.Empty);
        }

        private static float GetHue(byte R, byte G, byte B)
        {
            if (R == G && G == B)
                return 0; // 0 makes as good an UNDEFINED value as any

            float r = (float)R / 255.0f;
            float g = (float)G / 255.0f;
            float b = (float)B / 255.0f;

            float max, min;
            float delta;
            float hue = 0.0f;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            delta = max - min;

            if (r == max)
            {
                hue = (g - b) / delta;
            }
            else if (g == max)
            {
                hue = 2 + (b - r) / delta;
            }
            else if (b == max)
            {
                hue = 4 + (r - g) / delta;
            }
            hue *= 60;

            if (hue < 0.0f)
            {
                hue += 360.0f;
            }
            return hue;
        }
        private static float GetSaturation(byte R, byte G, byte B)
        {
            float r = (float)R / 255.0f;
            float g = (float)G / 255.0f;
            float b = (float)B / 255.0f;

            float max, min;
            float l, s = 0;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            // if max == min, then there is no color and
            // the saturation is zero.
            //
            if (max != min)
            {
                l = (max + min) / 2;

                if (l <= .5)
                {
                    s = (max - min) / (max + min);
                }
                else
                {
                    s = (max - min) / (2 - max - min);
                }
            }
            return s;
        }
        private static float GetBrightness(byte R, byte G, byte B)
        {
            float r = (float)R / 255.0f;
            float g = (float)G / 255.0f;
            float b = (float)B / 255.0f;

            float max, min;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            return (max + min) / 2;
        }
    }
}
