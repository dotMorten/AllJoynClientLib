using DeviceProviders;

namespace AllJoynClientLib.Devices.AllPlay
{
    //Link to spec: https://wiki.allseenalliance.org/_media/baseservices/alljoyn_media_control_service_0.3_hld.pdf
    public partial class PlayerClient :  DeviceClient
    {        
        internal PlayerClient(IService service) : base(service)
        {
            IInterface mediaPlayer = null;
            IInterface mcu = null;
            IInterface volume = null;
            IInterface zoneManager = null;
            foreach (var item in service.Objects)
            {
                if(mediaPlayer == null)
                    mediaPlayer = item.GetInterface("net.allplay.MediaPlayer");
                if (zoneManager == null)
                    zoneManager = item.GetInterface("net.allplay.ZoneManager");
                if (mcu == null)
                    mcu = item.GetInterface("net.allplay.MCU");
                if (volume == null)
                    volume = item.GetInterface("org.alljoyn.Control.Volume");
            }
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
        public string DeviceName { get { return Service.AboutData.DeviceName; } }
        public string Manufacturer { get { return Service.AboutData.Manufacturer; } }        
    }
}
