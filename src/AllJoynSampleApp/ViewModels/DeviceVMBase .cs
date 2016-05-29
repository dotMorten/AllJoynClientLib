using AllJoynClientLib.Devices.LSF;
using System.Threading.Tasks;

namespace AllJoynSampleApp.ViewModels
{
    public abstract class DeviceVMBase<T> : ViewModelBase where T: AllJoynClientLib.Devices.DeviceClient
    {
        private T _client;

        public DeviceVMBase(T lightClient)
        {
            _client = lightClient;
            OnStart();
        }

        private async void OnStart()
        {
            await Initialize();
            IsInitialized = true;
            OnPropertyChanged(nameof(IsInitialized));
        }

        protected virtual Task Initialize()
        {
            return Task.FromResult<object>(null);
        }

        public bool IsInitialized { get; private set; }
        
        public T Client { get { return _client; } }

    }
}