using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage.Streams;
using Windows.UI.Input;
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
        private bool isLoaded;
        private RadialController myController;
        private bool supportsHaptics;

        public LightClientView()
        {
            this.InitializeComponent();
        }

        public ViewModels.LightVM VM { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            isLoaded = true;
            base.OnNavigatedTo(e);
            VM = new ViewModels.LightVM(e.Parameter as AllJoynClientLib.Devices.LightClient);
            InitializeRadialController();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            isLoaded = false;
            VM.Unload();
            if(myController != null)
            {
                myController.Menu.Items.Clear();
                myController.ButtonClicked -= MyController_ButtonClicked;
                myController.RotationChanged -= MyController_RotationChanged;
            }
            base.OnNavigatingFrom(e);
        }

        private async void InitializeRadialController()
        {
            if (!ApiInformation.IsTypePresent(typeof(RadialController).FullName))
                return;
            myController = RadialController.CreateForCurrentView();
            if (myController == null)
                return;

            myController.RotationResolutionInDegrees = 1;
            myController.UseAutomaticHapticFeedback = false;

            await VM.InitializeTask;
            if(!isLoaded)
                return;

            RandomAccessStreamReference icon = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/StoreLogo.png"));

            // Create a menu item for the custom tool.
            myController.Menu.Items.Clear();

            if (VM.SupportsDimming)
            {
                myController.Menu.Items.Add(RadialControllerMenuItem.CreateFromIcon("Brightness", RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Brightness.png"))));
            }
            if (VM.SupportsColor)
            {
                myController.Menu.Items.Add(RadialControllerMenuItem.CreateFromIcon("Saturation", RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Saturation.png"))));
                myController.Menu.Items.Add(RadialControllerMenuItem.CreateFromIcon("Hue", RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/HueIcon.png"))));
            }

            var config = RadialControllerConfiguration.GetForCurrentView();
            config.SetDefaultMenuItems(new RadialControllerSystemMenuItemKind[] { });


            supportsHaptics = ApiInformation.IsTypePresent("Windows.Devices.Haptics.SimpleHapticsController");
            myController.ButtonClicked += MyController_ButtonClicked;
            myController.RotationChanged += MyController_RotationChanged;

            SurfaceDialTip.Visibility = Visibility.Visible;
        }

        private void MyController_RotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs args)
        {
            if (sender.Menu.GetSelectedMenuItem().DisplayText == "Brightness")
            {
                VM.Brightness = Math.Max(0, Math.Min(100, VM.Brightness + args.RotationDeltaInDegrees));
                if(VM.Brightness == 100 && args.RotationDeltaInDegrees > 0 ||
                    VM.Brightness == 0 && args.RotationDeltaInDegrees < 0)
                {
                    if (supportsHaptics)
                    {
                        args.SimpleHapticsController.SendHapticFeedback(args.SimpleHapticsController.SupportedFeedback.First());
                    }
                }
            }
            else if (sender.Menu.GetSelectedMenuItem().DisplayText == "Saturation")
            {
                VM.Saturation = Math.Max(0, Math.Min(1, VM.Saturation + args.RotationDeltaInDegrees / 100));
                if (VM.Saturation == 1 && args.RotationDeltaInDegrees > 0 ||
                    VM.Saturation == 0 && args.RotationDeltaInDegrees < 0)
                {
                    if (supportsHaptics)
                    {
                        args.SimpleHapticsController.SendHapticFeedback(args.SimpleHapticsController.SupportedFeedback.First());
                    }
                }
            }
            else if (sender.Menu.GetSelectedMenuItem().DisplayText == "Hue")
            {
                var hue = Math.Floor((VM.Hue + args.RotationDeltaInDegrees)) % 360;
                if (hue < 0) hue = 360 + hue;
                VM.Hue = hue;
                if (hue % 60 == 0)
                {
                    if (supportsHaptics)
                    {
                        args.SimpleHapticsController.SendHapticFeedback(args.SimpleHapticsController.SupportedFeedback.First());
                    }
                }
            }
        }

        private void MyController_ButtonClicked(RadialController sender, RadialControllerButtonClickedEventArgs args)
        {
            VM.IsOn = !VM.IsOn;
        }

        private void ColorPicker_ColorChanging(object sender, Controls.ColorPicker.HS e)
        {
            VM.SetHueAndSaturation(e.Hue, e.Saturation);
        }
    }
}
