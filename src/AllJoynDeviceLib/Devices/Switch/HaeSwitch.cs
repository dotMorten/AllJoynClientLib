using System.Threading.Tasks;
using DeviceProviders;

namespace AllJoynClientLib.Devices
{
    internal class HaeSwitch : SwitchClient
    {
        private IInterface offInterface = null;
        private IInterface onInterface = null;
        private IInterface onOffInterface = null;
        private IProperty onOffProperty = null;

        public HaeSwitch(IService service) : base(service)
        {
            offInterface = GetInterface("org.alljoyn.SmartSpaces.Operation.OffControl");
            onInterface = GetInterface("org.alljoyn.SmartSpaces.Operation.OnControl");
            onOffInterface = GetInterface("org.alljoyn.SmartSpaces.Operation.OnOffStatus");
            CanRaiseToggledEvent = onOffInterface != null;
            onOffProperty = onOffInterface.GetProperty("OnOff");
            onOffProperty.ValueChanged += OnOffProperty_ValueChanged;
        }

        private void OnOffProperty_ValueChanged(IProperty sender, object args)
        {
            OnToggled((bool)args);
        }

        public override Task<bool> GetOnOffAsync()
        {
            return onOffInterface.GetPropertyAsync<bool>("OnOff");
        }

        public override Task SetOnOffAsync(bool isOn)
        {
            if (isOn)
            {
                if (onInterface != null)
                {
                    return onInterface.InvokeMethodAsync("SwitchOn");
                }
            }
            else
            {
                if (offInterface != null)
                {
                    return offInterface.InvokeMethodAsync("SwitchOff");
                }
            }

            return Task.FromResult<object>(null);
        }
    }
}