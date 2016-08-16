using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AllJoynSampleApp.DeviceViews
{
    public sealed partial class LightClientView : Page
    {
        public LightClientView()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            VM = new ViewModels.LightVM(e.Parameter as AllJoynClientLib.Devices.LightClient);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            VM.Unload();
            base.OnNavigatingFrom(e);
        }

        public ViewModels.LightVM VM { get; private set; }

        private void ColorPicker_ColorChanging(object sender, Controls.ColorPicker.HS e)
        {
            VM.SetHueAndSaturation(e.Hue, e.Saturation);
        }
    }
}
