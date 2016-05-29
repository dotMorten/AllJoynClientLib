using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllJoynClientLib.Devices
{
    public class UnknownClient : DeviceClient
    {
        internal UnknownClient(DeviceProviders.IService service) : base(service)
        {
        }
    }
}
