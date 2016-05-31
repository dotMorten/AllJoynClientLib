using AllJoynClientLib.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllJoynSampleApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private AllJoynClientLib.DeviceManager manager;
        private ObservableCollection<DeviceClient> clients;

        public MainViewModel()
        {
            DevicePlugins.PhilipsHueDSB.Register(); //Register custom device plugin
            clients = new ObservableCollection<DeviceClient>();
            clients.CollectionChanged += Clients_CollectionChanged;
            Start();
        }


        private void Start()
        {
            manager = new AllJoynClientLib.DeviceManager();
            manager.DeviceJoined += Manager_DeviceJoined;
            manager.DeviceDropped += Manager_DeviceDropped;
            manager.TrackUnknownDevices = _trackUnknownDevices;
            manager.Start();
        }

        public void Restart()
        {
            manager.DeviceJoined -= Manager_DeviceJoined;
            manager.DeviceDropped -= Manager_DeviceDropped;
            manager.Stop();
            manager = null;
            clients.Clear();
            Start();
        }

        private bool _trackUnknownDevices;

        public bool TrackUnknownDevices
        {
            get { return _trackUnknownDevices; }
            set
            {
                if (_trackUnknownDevices != value)
                {
                    _trackUnknownDevices = value; Restart();
                }
            }
        }

        private void Clients_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var any = clients.Any();
            if (any != DevicesAvailable)
            {
                DevicesAvailable = any;
                OnPropertyChanged(nameof(DevicesAvailable));
            }
        }

        public bool DevicesAvailable { get; private set; }

        private void Manager_DeviceDropped(object sender, AllJoynClientLib.Devices.DeviceClient e)
        {
            ExecuteOnUIThread(() =>
            {
                clients.Remove(e);
            });
        }

        private void Manager_DeviceJoined(object sender, AllJoynClientLib.Devices.DeviceClient e)
        {
            ExecuteOnUIThread(() =>
            {
                clients.Add(e);
            });
        }

        public IEnumerable<DeviceClient> Devices { get { return clients; } }
    }
}
