using AllJoynClientLib.Devices;
using System.Threading.Tasks;
using System;

namespace AllJoynSampleApp.ViewModels
{
    public class LightVM : DeviceVMBase<LightClient>
    {

        public LightVM(LightClient lightClient) : base(lightClient)
        {
        }

        protected override async Task Initialize()
        {
            await LoadLampStateAsync();
            Client.LampStateChanged += Client_LampStateChanged;
        }

        protected internal override void Unload()
        {
            Client.LampStateChanged -= Client_LampStateChanged;
            base.Unload();
        }

        private async Task LoadLampStateAsync()
        {
            _isOn = await Client.GetOnOffAsync();
            SupportsDimming = await Client.GetIsDimmableAsync();
            if (SupportsDimming)
            {
                _brightness = await Client.GetBrightnessAsync();
            }
            SupportsColor = await Client.GetIsColorSupportedAsync();
            if (SupportsColor)
            {
                _Hue = await Client.GetHueAsync();
                _Saturation = await Client.GetSaturationAsync();
            }
            SupportsTemperature = await Client.GetIsVariableColorTempAsync();
            if (SupportsTemperature)
            {
                _Temperature = await Client.GetTemperatureAsync();
                MinTemperature = await Client.GetMinTemperatureAsync();
                MaxTemperature = await Client.GetMaxTemperatureAsync();
            }
            OnPropertyChanged(nameof(IsOn), nameof(Brightness), nameof(SupportsDimming), nameof(Hue), nameof(Saturation), nameof(SupportsColor),
                 nameof(Temperature), nameof(MinTemperature), nameof(MaxTemperature), nameof(SupportsTemperature));
        }


        private void Client_LampStateChanged(object sender, System.EventArgs e)
        {
            var _ = LoadLampStateAsync();
        }

        public bool SupportsDimming { get; private set; } = false;
        public bool SupportsColor { get; private set; } = false;
        public bool SupportsTemperature { get; private set; } = false;
        public double MinTemperature { get; private set; } = 1000;
        public double MaxTemperature { get; private set; } = 20000;

        private bool _isOn;

        public bool IsOn
        {
            get { return _isOn; }
            set
            {
                _isOn = value;
                OnPropertyChanged();
                var _ = Client.SetOnOffAsync(value);
            }
        }

        private ThrottleTask brightnessThrottle = new ThrottleTask(50);
        private double _brightness;
        public double Brightness
        {
            get
            {
                return _brightness * 100;
            }
            set
            {
                _brightness = value / 100;
                OnPropertyChanged();
                brightnessThrottle.Invoke(() =>
                {
                    var _ = Client.SetBrightnessAsync(_brightness);
                });
            }
        }

        private double _Hue;
        public double Hue
        {
            get
            {
                return _Hue;
            }
            set
            {
                _Hue = value;
                OnPropertyChanged();
                var _ = Client.SetHueAsync(_Hue);
            }
        }

        private double _Saturation;
        public double Saturation
        {
            get
            {
                return _Saturation;
            }
            set
            {
                _Saturation = value;
                OnPropertyChanged();
                var _ = Client.SetSaturationAsync(_Saturation);
            }
        }

        ThrottleTask huesat = new ThrottleTask(50);
        public void SetHueAndSaturation(double hue, double saturation)
        {
            huesat.Invoke(() => { SetHueAndSaturationInternal(hue, saturation); });
        }
        private async void SetHueAndSaturationInternal(double hue, double saturation)
        { 
            if(await Client.GetHasEffectsAsync())
            {
                var _ = Client.TransitionLampStateAsync(System.TimeSpan.Zero, hue, saturation, null, null);
                _Hue = hue;
                _Saturation = saturation;
                OnPropertyChanged(nameof(Hue), nameof(Saturation));
            }
            else
            {
                Hue = hue;
                Saturation = saturation;
            }
        }


        private ThrottleTask temperatureThrottle = new ThrottleTask(50);
        private double _Temperature = 2800;
        public double Temperature
        {
            get
            {
                return _Temperature;
            }
            set
            {
                _Temperature = value;
                OnPropertyChanged();
                temperatureThrottle.Invoke(() =>
                {
                    var _ = Client.SetTemperatureAsync(_Temperature);
                });
            }
        }
    }
}