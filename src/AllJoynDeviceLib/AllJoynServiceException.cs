using System;
using DeviceProviders;

namespace AllJoynClientLib
{
    /// <summary>
    /// AllJoyn specific error
    /// </summary>
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

        /// <summary>
        /// Gets the service name this error occured on
        /// </summary>
        public string Service { get; }

        /// <summary>
        /// Gets the path to the bus object this error occured on
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets the name of the interface this error occured on
        /// </summary>
        public string Interface { get; }

        /// <summary>
        /// Gets the operation on the <see cref="Interface"/> that failed
        /// </summary>
        public string Operation { get; }

        /// <summary>
        /// Gets the AllJoyn status error code
        /// </summary>
        public uint StatusCode { get; }
    }
}
