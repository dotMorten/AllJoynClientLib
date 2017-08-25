using AllJoynClientLib.Devices;
using System.Threading.Tasks;

namespace AllJoynSampleApp.ViewModels
{
    public class PhilipsHueBridgeVM : DeviceVMBase<DevicePlugins.PhilipsHueDSB>
    {
        public PhilipsHueBridgeVM(DevicePlugins.PhilipsHueDSB client) : base(client)
        {
            LinkCommand = new GenericCommand(async (obj) =>
            {
                LinkResult = ""; //clear last result
                OnPropertyChanged(nameof(LinkResult));
                LinkResult = await Client.LinkAsync(); //link hub
                OnPropertyChanged(nameof(LinkResult));
                IsLinked = await Client.GetIsLinkedAsync(); //update islinked property
                OnPropertyChanged(nameof(IsLinked));
            });
        }

        protected override async Task Initialize()
        {
            IsLinked = await Client.GetIsLinkedAsync();
            OnPropertyChanged(nameof(IsLinked));
        }

        public bool IsLinked { get; private set; }

        public System.Windows.Input.ICommand LinkCommand { get; }

        public string LinkResult { get; private set; }
    }
}