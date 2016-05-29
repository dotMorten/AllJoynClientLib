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
            clients = new ObservableCollection<DeviceClient>();
            manager = new AllJoynClientLib.DeviceManager();
            manager.DeviceJoined += Manager_DeviceJoined;
            manager.DeviceDropped += Manager_DeviceDropped;
            manager.TrackUnknownDevices = false;
            manager.Start();
        }

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
