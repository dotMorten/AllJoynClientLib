using AllJoynClientLib.Devices;
using AllJoynClientLib.Devices.SmartSpaces;
using System;
using System.Threading.Tasks;

namespace AllJoynSampleApp.ViewModels
{
    public class CurrentHumidityVM : DeviceVMBase<CurrentHumidityClient>
    {
        public CurrentHumidityVM(CurrentHumidityClient client) : base(client)
        {
        }

        protected override async Task Initialize()
        {
            _currentValue = await Client.GetCurrentValueAsync();
            OnPropertyChanged(nameof(Humidity));
            Client.CurrentValueChanged += Client_CurrentValueChanged;
        }

        private void Client_CurrentValueChanged(object sender, double e)
        {
            _currentValue = e;
            OnPropertyChanged(nameof(Humidity));
        }

        private double _currentValue;

        public string Humidity
        {
            get {
                return $"{_currentValue}%"; }
        }
    }
}