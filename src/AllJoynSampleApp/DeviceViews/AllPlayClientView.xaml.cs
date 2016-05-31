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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AllJoynSampleApp.DeviceViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllPlayClientView : Page
    {
        public AllPlayClientView()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            VM = new ViewModels.MediaPlayerVM(e.Parameter as AllJoynClientLib.Devices.AllPlayClient);
        }
        private void playlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VM?.PlayListEntry(playlist.SelectedIndex);
        }
        public ViewModels.MediaPlayerVM VM { get; private set; }
    }
}
