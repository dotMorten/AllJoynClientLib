using DeviceProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllJoynClientLib.Devices.AllPlay
{
    public class PlayState
    {
        internal PlayState(AllJoynMessageArgStructure arg)
        {
            //(sxuuuiia(ssssxsssa{ss}a{sv}v))
            this.State = StringToMediaState(arg[0] as string);
            Position = TimeSpan.FromMilliseconds((long)arg[1]);
            SampleRate = (uint)arg[2];
            AudioChannels = (uint)arg[3];
            BitsPerSample = (uint)arg[4];
            IndexCurrentItem = (int)arg[5];
            IndexNextItem = (int)arg[6];
            var t = arg[7] as IList<object>;
            if (t?.Count > 0)
            {
                CurrentMedia = new Media(t[0] as AllJoynMessageArgStructure);
                if (t.Count > 1)
                {
                    NextMedia = new Media(t[1] as AllJoynMessageArgStructure);
                }
            }
            //Note: Still a few more values for custom data not being processed here
        }

        private static MediaState StringToMediaState(string mode)
        {
            switch (mode)
            {
                case "BUFFERING": return MediaState.Buffering;
                case "TRANSITIONING": return MediaState.Transitioning;
                case "PLAYING": return MediaState.Playing;
                case "STOPPED":
                default:
                    return MediaState.Stopped;
            }
        }


        public Media CurrentMedia;
        public Media NextMedia;
        public MediaState State { get; }
        public TimeSpan Position { get; }
        public uint SampleRate { get; }
        public uint AudioChannels { get; }
        public uint BitsPerSample { get; }
        public int IndexCurrentItem { get; }
        public int IndexNextItem { get; }
    }

    
}
