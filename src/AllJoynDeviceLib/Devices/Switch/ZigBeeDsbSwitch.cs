using System;
using System.Threading.Tasks;
using DeviceProviders;

namespace AllJoynClientLib.Devices
{
    internal class ZigBeeDsbSwitch : SwitchClient
    {
        private IInterface switchInterface = null;
        public ZigBeeDsbSwitch(IService service) : base(service)
        {
            var items = service.Objects;
            if (items != null)
            {
                foreach (var item in items)
    	        {
	                if (switchInterface == null)
                    switchInterface = item.GetInterface("com.microsoft.ZWaveBridge.SwitchBinary.Switch");
                    if (switchInterface == null)
                        switchInterface = item.GetInterface("com.microsoft.ZWaveBridge.Switch");
                }
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