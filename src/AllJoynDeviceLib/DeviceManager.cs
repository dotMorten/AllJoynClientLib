using System;
using System.Collections.Generic;
using System.Linq;
using AllJoynClientLib.Devices;

namespace AllJoynClientLib
{
    /// <summary>
    /// AllJoyn Device Manager
    /// </summary>
    public class DeviceManager
    {
        private object devicesLock = new object();
        private DeviceProviders.AllJoynProvider provider;
        private Dictionary<string, DeviceClient[]> devices = new Dictionary<string, DeviceClient[]>();
        private static Dictionary<string, Func<DeviceProviders.IService, DeviceClient>> devicesCreators = new Dictionary<string, Func<DeviceProviders.IService, DeviceClient>>();

        static DeviceManager()
        {
            RegisterClient("net.allplay.MediaPlayer", (svc) => new Devices.AllPlayClient(svc));  // Could probably also be "org.allseen.media.control.mediaPlayer"
            RegisterClient("org.allseen.LSF.LampState", (svc) => new Devices.LightClient(svc));
            RegisterClient("com.microsoft.ZWaveBridge.SwitchBinary.Switch", (svc) => new Devices.ZigBeeDsbSwitch(svc));
            RegisterClient("com.microsoft.ZWaveBridge.Switch", (svc) => new Devices.ZigBeeDsbSwitch(svc));
            RegisterClient("org.alljoyn.SmartSpaces.Environment.CurrentTemperature", (svc) => new Devices.SmartSpaces.CurrentTemperatureClient(svc));
            RegisterClient("org.alljoyn.SmartSpaces.Environment.CurrentHumidity", (svc) => new Devices.SmartSpaces.CurrentHumidityClient(svc));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceManager"/> class.
        /// </summary>
        public DeviceManager()
        {
        }

        /// <summary>
        /// Allows you to register extra device clients based on an interface
        /// </summary>
        /// <param name="interfaceName">Name of the interface that identifies the device (if multiple only register the main / required one)</param>
        /// <param name="creator">Method for creating a DeviceClient</param>
        public static void RegisterClient(string interfaceName, Func<DeviceProviders.IService, DeviceClient> creator)
        {
            devicesCreators[interfaceName] = creator;
        }

        /// <summary>
        /// Gets or sets a value indicating whether device types not registered with the device manager
        /// are also discovered and reported
        /// </summary>
        /// <seealso cref="UnknownClient"/>
        public bool TrackUnknownDevices { get; set; }

        /// <summary>
        /// Starts searching for devices
        /// </summary>
        public void Start()
        {
            if (provider != null)
            {
                return;
            }

            provider = new DeviceProviders.AllJoynProvider();
            provider.ServiceJoined += Provider_ServiceJoined;
            provider.ServiceDropped += Provider_ServiceDropped;
            provider.Start();
        }

        /// <summary>
        /// Stops searching for and disposes any tracked devices.
        /// </summary>
        public void Stop()
        {
            if (provider == null)
            {
                return;
            }

            provider.ServiceJoined -= Provider_ServiceJoined;
            provider.ServiceDropped -= Provider_ServiceDropped;
            lock (devicesLock)
            {
                foreach (var device in devices)
                {
                    foreach (var client in device.Value)
                    {
                        client.ServiceDropped();
                    }
                }

                devices.Clear();
            }

            provider.Shutdown();
            provider = null;
        }

        private void Provider_ServiceJoined(DeviceProviders.IProvider sender, DeviceProviders.ServiceJoinedEventArgs args)
        {
            var svc = args.Service;
            svc.JoinSession();
            System.Diagnostics.Debug.WriteLine($"Service Joined: {svc?.AboutData?.DeviceName ?? "<UNKNOWN>"} ({svc?.AboutData?.DeviceId} - {svc.Name})");
            List<DeviceClient> clients = new List<DeviceClient>();
            foreach (var interfaceName in devicesCreators)
            {
                if (svc.ImplementsInterface(interfaceName.Key))
                {
                    clients.Add(interfaceName.Value(args.Service));
                }
            }

            if (TrackUnknownDevices && !clients.Any())
            {
                clients.Add(new UnknownClient(svc));
            }

            if (clients.Any())
            {
                // Device found
                lock (devicesLock)
                {
                    devices.Add(svc.Name, clients.ToArray());
                }

                foreach (var client in clients)
                {
                    DeviceJoined?.Invoke(this, client);
                }
            }
        }

        private void Provider_ServiceDropped(DeviceProviders.IProvider sender, DeviceProviders.ServiceDroppedEventArgs args)
        {
            var svc = args.Service.Name;
            System.Diagnostics.Debug.WriteLine($"Service Joined: {svc ?? "<UNKNOWN>"}");
            DeviceClient[] clients = null;
            lock (devicesLock)
            {
                if (devices.ContainsKey(svc))
                {
                    clients = devices[svc];
                    devices.Remove(svc);
                }
            }

            if (clients != null)
            {
                foreach (var client in clients)
                {
                    client.ServiceDropped();
                    DeviceDropped?.Invoke(this, client);
                }
            }
        }

        /// <summary>
        /// Raised when a device is found
        /// </summary>
        public event EventHandler<DeviceClient> DeviceJoined;

        /// <summary>
        /// Raised when a device is lost
        /// </summary>
        public event EventHandler<DeviceClient> DeviceDropped;
    }
}
