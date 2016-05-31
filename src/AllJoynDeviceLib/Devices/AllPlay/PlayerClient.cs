using AllJoynClientLib.Devices.AllPlay;
using DeviceProviders;

namespace AllJoynClientLib.Devices
{
    //Link to spec: https://wiki.allseenalliance.org/_media/baseservices/alljoyn_media_control_service_0.3_hld.pdf
    public partial class AllPlayClient :  DeviceClient
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
            if (volume != null) Volume = new Volume(volume);
            if (zoneManager != null) ZoneManager = new ZoneManager(zoneManager);
            if (mcu != null) Mcu = new Mcu(mcu);
        }
        protected override void OnDeviceLost()
        {
            base.OnDeviceLost();
            MediaPlayer.OnDeviceLost();
        }
        public MediaPlayer MediaPlayer { get; }
        public Volume Volume { get; }
        public ZoneManager ZoneManager { get; }
        public Mcu Mcu { get; }
    }
}
