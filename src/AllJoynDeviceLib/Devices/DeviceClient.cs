using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllJoynClientLib.Devices
{
    public abstract class DeviceClient
    {
        protected DeviceClient(DeviceProviders.IService service)
        {
            Service = service;
        }

        internal void DeviceLost()
        {
            OnDeviceLost();
        }
        protected virtual void OnDeviceLost()
        {
        }

        public DeviceProviders.IService Service { get; }

        public string Name
        {
            get
            {
                return Service?.Name;
            }
        }

        public string DeviceName
        {
            get
            {
                return Service?.AboutData?.DeviceName;
            }
        }

        public string ModelNumber
        {
            get
            {
                return Service?.AboutData?.ModelNumber;
            }
        }

        public string DeviceId
        {
            get
            {
                return Service?.AboutData?.DeviceId;
            }
        }

        public string Manufacturer
        {
            get
            {
                return Service?.AboutData?.Manufacturer;
            }
        }

        public string Description
        {
            get
            {
                return Service?.AboutData?.Description;
            }
        }
    }
}
