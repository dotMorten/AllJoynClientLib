using AllJoynClientLib.Devices;
using AllJoynClientLib.Devices.SmartSpaces;
using System;
using System.Threading.Tasks;

namespace AllJoynSampleApp.ViewModels
{
    public class CurrentTemperatureVM : DeviceVMBase<CurrentTemperatureClient>
    {
        public CurrentTemperatureVM(CurrentTemperatureClient client) : base(client)
        {
        }

        protected override async Task Initialize()
        {
            _currentValue = await Client.GetCurrentValueAsync();
            OnPropertyChanged(nameof(Temperature));
            Client.CurrentValueChanged += Client_CurrentValueChanged;
        }

        private void Client_CurrentValueChanged(object sender, double e)
        {
            _currentValue = e;
            OnPropertyChanged(nameof(Temperature));
        }

        private double _currentValue;

        public string Temperature
        {
            get {
                double fahrenheit = Math.Round((_currentValue) * 9d / 5d + 32, 1);
                return $"{_currentValue}⁰C\n{fahrenheit}⁰F"; }
        }
    }
}