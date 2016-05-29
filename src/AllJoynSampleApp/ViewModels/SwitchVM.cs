using AllJoynClientLib.Devices.Switch;
using System.Threading.Tasks;

namespace AllJoynSampleApp.ViewModels
{
    public class SwitchVM : DeviceVMBase<SwitchClient>
    {
        public SwitchVM(SwitchClient client) : base(client)
        {
        }

        protected override async Task Initialize()
        {
            _isOn = await Client.GetOnOffAsync();
            OnPropertyChanged(nameof(IsOn));
        }

        private bool _isOn;

        public bool IsOn
        {
            get { return _isOn; }
            set { _isOn = value; OnPropertyChanged();
                var _ = Client.SetOnOffAsync(value);
            }
        }
    }
}