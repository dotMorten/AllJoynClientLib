using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllJoynClientLib.Devices
{
    public abstract class SwitchClient : DeviceClient
    {
        internal SwitchClient(DeviceProviders.IService service) : base(service)
        {
        }


        public abstract Task SetOnOffAsync(bool isOn);

        public abstract Task<bool> GetOnOffAsync();
    }
}
