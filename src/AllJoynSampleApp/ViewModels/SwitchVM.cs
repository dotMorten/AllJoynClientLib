using AllJoynClientLib.Devices;
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
            Client.Toggled += Client_Toggled;
            _isOn = await Client.GetOnOffAsync();
            OnPropertyChanged(nameof(IsOn));
        }

        protected internal override void Unload()
        {
            Client.Toggled -= Client_Toggled;
            base.Unload();
        }

        private void Client_Toggled(object sender, bool e)
        {
            _isOn = e;
            OnPropertyChanged(nameof(IsOn));
        }

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
    }
}