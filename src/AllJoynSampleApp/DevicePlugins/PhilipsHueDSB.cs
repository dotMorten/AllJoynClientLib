using AllJoynClientLib.Devices;
using DeviceProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllJoynSampleApp.DevicePlugins
{
    // A custom device plugin for the Philips Hue Device Service Bridge
    // See https://github.com/dotMorten/AllJoynPhilipsHueDSB
    public class PhilipsHueDSB : DeviceClient
    {
        public static void Register()
        {
            AllJoynClientLib.DeviceManager.RegisterClient("com.dotMorten.PhilipsHueDSB.PhilipsHue.MainInterface", (service) => { return new PhilipsHueDSB(service); });
        }

        private IInterface mainInterface;
        private IInterface hueInterface;

        public PhilipsHueDSB(DeviceProviders.IService service) : base(service)
        {
            mainInterface = GetInterface("com.dotMorten.PhilipsHueDSB.PhilipsHue.MainInterface");
            hueInterface = GetInterface("com.dotMorten.PhilipsHueDSB.PhilipsHue");
            //bool IsLinked {get;}
        }

        public Task<bool> GetIsLinkedAsync()
        {
            return hueInterface.GetPropertyAsync<bool>("IsLinked");
        }

        public async Task<string> LinkAsync()
        {
            var result = await mainInterface.InvokeMethodAsync("Link");
            return result.First() as string;
        }
    }
}
