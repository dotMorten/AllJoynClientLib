using AllJoynClientLib.Devices.AllPlay;
using DeviceProviders;

namespace AllJoynClientLib.Devices
{
    // Link to spec: https://wiki.allseenalliance.org/_media/baseservices/alljoyn_media_control_service_0.3_hld.pdf

    /// <summary>
    /// An AllPlay Media Player client
    /// </summary>
    public partial class AllPlayClient : DeviceClient
    {
        internal AllPlayClient(IService service) : base(service)
        {
            IInterface mediaPlayer = null;
            IInterface mcu = null;
            IInterface volume = null;
            IInterface zoneManager = null;
            mediaPlayer = GetInterface("net.allplay.MediaPlayer");
            zoneManager = GetInterface("net.allplay.ZoneManager");
            mcu = GetInterface("net.allplay.MCU");
            volume = GetInterface("org.alljoyn.Control.Volume");

            this.MediaPlayer = new MediaPlayer(mediaPlayer);
            if (volume != null)
            {
                Volume = new Volume(volume);
            }

            if (zoneManager != null)
            {
                ZoneManager = new ZoneManager(zoneManager);
            }

            if (mcu != null)
            {
                Mcu = new Mcu(mcu);
            }
        }

        /// <summary>
        /// Called when the device was lost from the network
        /// </summary>
        protected sealed override void OnDeviceLost()
        {
            base.OnDeviceLost();
            MediaPlayer.OnDeviceLost();
        }

        /// <summary>
        /// Gets a reference to the media player part of the client. This is used to control the playing of media content.
        /// </summary>
        public MediaPlayer MediaPlayer { get; }

        /// <summary>
        /// Gets the Volume interface is implemented by an AllJoyn application on a multimedia device to
        /// allow an AllJoyn client to control its audio volume.
        /// </summary>
        public Volume Volume { get; }

        /// <summary>
        /// Gets the interface that stores information about the zone the player belongs to. A player can only be part of one zone at any given time.
        /// </summary>
        public ZoneManager ZoneManager { get; }

        /// <summary>
        /// Gets the MCU interface <c>net.allplay.MCU</c> (undocumented by Qualcomm)
        /// </summary>
        public Mcu Mcu { get; }
    }
}
