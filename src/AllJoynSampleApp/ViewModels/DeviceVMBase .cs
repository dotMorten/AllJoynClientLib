using AllJoynClientLib.Devices;
using System.Threading.Tasks;

namespace AllJoynSampleApp.ViewModels
{
    public abstract class DeviceVMBase<T> : ViewModelBase where T: DeviceClient
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


        private Task<Windows.UI.Xaml.Media.Imaging.BitmapSource> iconTask;
        public Windows.UI.Xaml.Media.Imaging.BitmapSource Icon
        {
            get
            {
                return LoadIcon();
            }
        }

        private Windows.UI.Xaml.Media.Imaging.BitmapSource LoadIcon()
        {
            if(iconTask == null)
            {
                iconTask = Client.GetIconAsync();
                iconTask.ContinueWith(t =>
                {
                    if(t.IsCompleted && !t.IsFaulted && !t.IsCanceled)
                    {
                        OnPropertyChanged(nameof(Icon));
                    }
                });
            }
            if (iconTask.IsCompleted)
                return iconTask.Result;
            return null;
        }
    }
}