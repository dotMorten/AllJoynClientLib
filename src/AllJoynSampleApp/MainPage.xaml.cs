using AllJoynClientLib.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AllJoynSampleApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            handler = new KeyEventHandler(App_KeyDown);
            VM.DeviceDropped += VM_DeviceDropped;
        }

        private async void VM_DeviceDropped(object sender, DeviceClient e)
        {
            if(activeClient == e)
            {
                await new Windows.UI.Popups.MessageDialog($"The device {e.DeviceName} was lost", "Device Lost").ShowAsync();
                Frame.GoBack();
            }
        }

        KeyEventHandler handler;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            activeClient = null;
            base.OnNavigatedTo(e);
            Window.Current.Content.AddHandler(UIElement.KeyDownEvent, handler, true);
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Window.Current.Content.RemoveHandler(UIElement.KeyDownEvent, handler);
            base.OnNavigatingFrom(e);
        }
        private void App_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.F5)
            {
                VM.Restart();

            }
            base.OnKeyDown(e);
        }

        public ViewModels.MainViewModel VM { get; } = new ViewModels.MainViewModel();

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as DeviceClient;
            if (item is AllJoynClientLib.Devices.LightClient)
            {
                Frame.Navigate(typeof(DeviceViews.LightClientView), item);
            }
            else if (item is AllJoynClientLib.Devices.AllPlayClient)
            {
                Frame.Navigate(typeof(DeviceViews.AllPlayClientView), item);
            }
            else if (item is AllJoynClientLib.Devices.SwitchClient)
            {
                Frame.Navigate(typeof(DeviceViews.SwitchClientView), item);
            }
            else if (item is AllJoynClientLib.Devices.SmartSpaces.CurrentTemperatureClient)
            {
                Frame.Navigate(typeof(DeviceViews.TemperatureClientView), item);
            }
            else if (item is AllJoynClientLib.Devices.SmartSpaces.CurrentHumidityClient)
            {
                Frame.Navigate(typeof(DeviceViews.HumidityClientView), item);
            }
            else if (item is DevicePlugins.PhilipsHueDSB)
            {
                Frame.Navigate(typeof(DeviceViews.PhilipsHueBridgeView), item);
            }
            else return;
            activeClient = item;
        }
        private DeviceClient activeClient;

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }
    }
}
