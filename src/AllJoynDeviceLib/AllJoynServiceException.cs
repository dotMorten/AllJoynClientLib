using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;

namespace AllJoynClientLib
{
    public class AllJoynServiceException : Exception
    {
        internal AllJoynServiceException(AllJoynStatus status, IInterface iface, string operation) : base(status.StatusText)
        {
            this.StatusCode = status.StatusCode;
            Interface = iface.Name;
            Path = iface.BusObject.Path;
            Operation = operation;
            Service = iface.BusObject.Service.Name + ":" + iface.BusObject.Service.AnnouncedPort;
        }
        public string Service { get; }
        public string Path { get; }
        public string Interface { get; }
        public string Operation { get; }

        public uint StatusCode { get; }
    }
}
