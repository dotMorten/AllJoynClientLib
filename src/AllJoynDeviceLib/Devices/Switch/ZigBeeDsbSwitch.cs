using System;
using System.Threading.Tasks;
using DeviceProviders;

namespace AllJoynClientLib.Devices.Switch
{
    internal class ZigBeeDsbSwitch : SwitchClient
    {
        private IInterface switchInterface = null;
        public ZigBeeDsbSwitch(IService service) : base(service)
        {
            foreach (var item in service.Objects)
            {
                if (switchInterface == null)
                    switchInterface = item.GetInterface("com.microsoft.ZWaveBridge.SwitchBinary.Switch");
            }
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