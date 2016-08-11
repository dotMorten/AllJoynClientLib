using System.Threading.Tasks;
using DeviceProviders;

namespace AllJoynClientLib.Devices
{
    internal class ZigBeeDsbSwitch : SwitchClient
    {
        private IInterface switchInterface = null;
        private IProperty valueProperty = null;

        public ZigBeeDsbSwitch(IService service) : base(service)
        {
            switchInterface = GetInterface("com.microsoft.ZWaveBridge.SwitchBinary.Switch") ??
                              GetInterface("com.microsoft.ZWaveBridge.Switch");
            valueProperty = switchInterface.GetProperty("Value");
            CanRaiseToggledEvent =
                valueProperty.Annotations.ContainsKey("org.freedesktop.DBus.Property.EmitsChangedSignal") &&
                valueProperty.Annotations["org.freedesktop.DBus.Property.EmitsChangedSignal"] == "true";
            if (CanRaiseToggledEvent)
            {
                valueProperty.ValueChanged += ValueProperty_ValueChanged;
            }
        }

        private void ValueProperty_ValueChanged(IProperty sender, object args)
        {
            OnToggled((bool)args);
        }

        public override Task<bool> GetOnOffAsync()
        {
            return switchInterface.GetPropertyAsync<bool>("Value");
        }

        public override Task SetOnOffAsync(bool isOn)
        {
            return switchInterface.SetPropertyAsync("Value", isOn);
        }
    }
}