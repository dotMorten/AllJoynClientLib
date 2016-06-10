using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceProviders;

namespace AllJoynClientLib.Devices
{
    public partial class LightClient : SwitchClient
    {
        private const uint MaxValue = uint.MaxValue - 1;

        private IInterface lampState = null;

        internal LightClient(DeviceProviders.IService service)
            : base(service)
        {
            lampState = GetInterface("org.allseen.LSF.LampState");
            lampDetails = GetInterface("org.allseen.LSF.LampDetails");
        }

        /// <summary>
        /// Called when the device was lost from the network
        /// </summary>
        /// <remarks>Use this clean up.</remarks>
        protected sealed override void OnDeviceLost()
        {
            if (_lampStateChangedSignal != null)
            {
                _lampStateChangedSignal.SignalRaised -= LampStateChangedSignal_SignalRaised;
            }

            base.OnDeviceLost();
        }

        /// <summary>
        /// Gets a value indicating whether this light is on or off
        /// </summary>
        /// <returns>True if on</returns>
        public override Task<bool> GetOnOffAsync()
        {
            return lampState.GetPropertyAsync<bool>("OnOff");
        }

        /// <summary>
        /// Sets the on/off status of the lamp.
        /// </summary>
        /// <param name="isOn">On if set to <c>true</c> otherwise off.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public override Task SetOnOffAsync(bool isOn)
        {
            return lampState.SetPropertyAsync("OnOff", isOn);
        }

        /// <summary>
        /// Gets the current brightness as a value between 0 and 1
        /// </summary>
        /// <returns>The brightness</returns>
        public async Task<double> GetBrightnessAsync()
        {
            uint b = await lampState.GetPropertyAsync<uint>("Brightness").ConfigureAwait(false);
            return b / (double)MaxValue;
        }

        /// <summary>
        /// Brightness as a value between 0 and 1
        /// </summary>
        /// <param name="brightness">The brightness as a value between 0 (off) and 1 (full brightness).</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task SetBrightnessAsync(double brightness)
        {
            if (brightness < 0)
            {
                brightness = 0;
            }
            else if (brightness > 1)
            {
                brightness = 1;
            }

            return lampState.SetPropertyAsync("Brightness", (uint)(brightness * MaxValue));
        }

        /// <summary>
        /// Gets the saturation as a value between 0 (no color) and 1 (fully saturated)
        /// </summary>
        /// <returns>The current saturation</returns>
        public async Task<double> GetSaturationAsync()
        {
            uint s = await lampState.GetPropertyAsync<uint>("Saturation").ConfigureAwait(false);
            return s / (double)MaxValue;
        }

        /// <summary>
        /// Sets the saturation on the light
        /// </summary>
        /// <param name="saturation">The saturation as a value between 0 (no color) and 1 (fully saturated)</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task SetSaturationAsync(double saturation)
        {
            if (saturation < 0)
            {
                saturation = 0;
            }
            else if (saturation > 1)
            {
                saturation = 1;
            }

            return lampState.SetPropertyAsync("Saturation", (uint)(saturation * MaxValue));
        }

        /// <summary>
        /// Hue as a value between 0 and 360
        /// </summary>
        /// <returns>The current hue</returns>
        public async Task<double> GetHueAsync()
        {
            uint s = await lampState.GetPropertyAsync<uint>("Hue").ConfigureAwait(false);
            return s / (double)MaxValue * 360d;
        }

        /// <summary>
        /// Hue as a value between 0 and 360
        /// </summary>
        /// <param name="hue">The hue as a value between 0 and 360.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task SetHueAsync(double hue)
        {
            if (hue < 0)
            {
                hue = 0;
            }
            else if (hue > 360)
            {
                hue = 360;
            }

            return lampState.SetPropertyAsync("Hue", (uint)(hue / 360d * MaxValue));
        }

        /// <summary>
        /// Sets the color of the lamp.
        /// </summary>
        /// <param name="color">Color to apply to the bulb</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task SetColorAsync(Windows.UI.Color color)
        {
            return SetColorAsync(color.R, color.G, color.B, TimeSpan.Zero);
        }

        /// <summary>
        /// Sets the color of the lamp.
        /// </summary>
        /// <param name="red">Red component</param>
        /// <param name="green">Green component</param>
        /// <param name="blue">Blue component</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task SetColorAsync(byte red, byte green, byte blue)
        {
            return SetColorAsync(red, green, blue, TimeSpan.Zero);
        }

        /// <summary>
        /// Sets the color of the lamp over the specified transition time.
        /// </summary>
        /// <param name="red">Red component</param>
        /// <param name="green">Green component</param>
        /// <param name="blue">Blue component</param>
        /// <param name="transitionTime">Length of the transition</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SetColorAsync(byte red, byte green, byte blue, TimeSpan transitionTime)
        {
            var h = GetHue(red, green, blue);
            var s = GetSaturation(red, green, blue);
            var b = GetBrightness(red, green, blue);

            if (await GetHasEffectsAsync().ConfigureAwait(false))
            {
                await TransitionLampStateAsync(TimeSpan.Zero, h, s, b, null);
            }
            else
            {
                await Task.WhenAll(new Task[]
                {
                    SetHueAsync(h),
                    SetSaturationAsync(s),
                    SetBrightnessAsync(b)
                }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Sets the state of the lamp over the specified transition time.
        /// </summary>
        /// <param name="transitionTime">Length of the transition</param>
        /// <param name="hue">Hue or null if leave unchanged</param>
        /// <param name="saturation">Saturation or null if leave unchanged</param>
        /// <param name="brightness">Brightness or null if leave unchanged</param>
        /// <param name="temperature">Temperature or null if leave unchanged</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task TransitionLampStateAsync(TimeSpan transitionTime, double? hue, double? saturation, double? brightness, double? temperature)
        {
            var newState = new Dictionary<object, object>();
            if (hue.HasValue)
            {
                newState.Add("Hue", (uint)(hue.Value / 360 * MaxValue));
            }

            if (saturation.HasValue)
            {
                newState.Add("Saturation", (uint)(saturation.Value * MaxValue));
            }

            if (brightness.HasValue)
            {
                newState.Add("Brightness", (uint)(brightness.Value * MaxValue));
            }

            if (temperature.HasValue)
            {
                newState.Add("ColorTemp", (uint)(temperature - 1000) / 19000 * MaxValue);
            }

            return lampState.InvokeMethodAsync("TransitionLampState", 0UL, newState.ToList(), (uint)transitionTime.TotalMilliseconds);
        }

        /// <summary>
        /// Temperature in Kelvin
        /// </summary>
        /// <returns>The temperature in kelvin</returns>
        public async Task<double> GetTemperatureAsync()
        {
            // In AllJoyn 0 means 1000K. uint32_max-1 means 20000K.
            uint t = await lampState.GetPropertyAsync<uint>("ColorTemp").ConfigureAwait(false);
            return (t / (MaxValue * 19000d)) + 1000;
        }

        /// <summary>
        /// Temperature in Kelvin
        /// </summary>
        /// <param name="temperature">The temperature in kelvin.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task SetTemperatureAsync(double temperature)
        {
            if (temperature < 1000)
            {
                temperature = 1000;
            }
            else if (temperature > 20000)
            {
                temperature = 20000;
            }

            return lampState.SetPropertyAsync("ColorTemp", (uint)((temperature - 1000) / 19000 * MaxValue));
        }

        private EventHandler _lampStateChanged;
        private ISignal _lampStateChangedSignal;

        /// <summary>
        /// Raised when the lamp state has changed
        /// </summary>
        public event EventHandler LampStateChanged
        {
            add
            {
                if (_lampStateChanged == null)
                {
                    if (_lampStateChangedSignal == null)
                    {
                        _lampStateChangedSignal = lampState.GetSignal(nameof(LampStateChanged));
                    }

                    if (_lampStateChangedSignal != null)
                    {
                        _lampStateChangedSignal.SignalRaised += LampStateChangedSignal_SignalRaised;
                    }
                }

                _lampStateChanged += value;
            }

            remove
            {
                _lampStateChanged -= value;
                if (_lampStateChanged == null && _lampStateChangedSignal != null)
                {
                    _lampStateChangedSignal.SignalRaised -= LampStateChangedSignal_SignalRaised;
                }
            }
        }

        private void LampStateChangedSignal_SignalRaised(ISignal sender, IList<object> args)
        {
            _lampStateChanged?.Invoke(this, EventArgs.Empty);
        }

        private static float GetHue(byte red, byte green, byte blue)
        {
            if (red == green && green == blue)
            {
                return 0; // 0 makes as good an UNDEFINED value as any
            }

            float r = (float)red / 255.0f;
            float g = (float)green / 255.0f;
            float b = (float)blue / 255.0f;

            float max, min;
            float delta;
            float hue = 0.0f;

            max = r;
            min = r;

            if (g > max)
            {
                max = g;
            }

            if (b > max)
            {
                max = b;
            }

            if (g < min)
            {
                min = g;
            }

            if (b < min)
            {
                min = b;
            }

            delta = max - min;

            if (r == max)
            {
                hue = (g - b) / delta;
            }
            else if (g == max)
            {
                hue = 2 + ((b - r) / delta);
            }
            else if (b == max)
            {
                hue = 4 + ((r - g) / delta);
            }

            hue *= 60;

            if (hue < 0.0f)
            {
                hue += 360.0f;
            }

            return hue;
        }

        private static float GetSaturation(byte red, byte green, byte blue)
        {
            float r = (float)red / 255.0f;
            float g = (float)green / 255.0f;
            float b = (float)blue / 255.0f;

            float max, min;
            float l, s = 0;

            max = r;
            min = r;

            if (g > max)
            {
                max = g;
            }

            if (b > max)
            {
                max = b;
            }

            if (g < min)
            {
                min = g;
            }

            if (b < min)
            {
                min = b;
            }

            // if max == min, then there is no color and
            // the saturation is zero.
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

        private static float GetBrightness(byte red, byte green, byte blue)
        {
            float r = (float)red / 255.0f;
            float g = (float)green / 255.0f;
            float b = (float)blue / 255.0f;

            float max, min;

            max = r;
            min = r;

            if (g > max)
            {
                max = g;
            }

            if (b > max)
            {
                max = b;
            }

            if (g < min)
            {
                min = g;
            }

            if (b < min)
            {
                min = b;
            }

            return (max + min) / 2;
        }
    }
}
