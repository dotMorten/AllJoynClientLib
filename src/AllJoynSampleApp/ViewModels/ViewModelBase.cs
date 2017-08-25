using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace AllJoynSampleApp.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase()
        {
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        }

        protected CoreDispatcher Dispatcher { get; }

        public void GoBack()
        {
            var frame = Windows.UI.Xaml.Window.Current.Content as Windows.UI.Xaml.Controls.Frame;
            if (frame != null && frame.CanGoBack)
                frame.GoBack();
        }

        protected void ExecuteOnUIThread(Action action)
        {
            if (Dispatcher.HasThreadAccess)
            {
                action();
            }
            else
            {
                var _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
            }
        }

        protected void OnPropertyChanged(params string[] properties)
        {
            ExecuteOnUIThread(() =>
            {
                var handler = PropertyChanged;
                if (handler != null)
                {
                    foreach (var propertyName in properties)
                        handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            });
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            ExecuteOnUIThread(() =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
