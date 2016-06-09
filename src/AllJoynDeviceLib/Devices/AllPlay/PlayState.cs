using System;
using System.Collections.Generic;
using DeviceProviders;

namespace AllJoynClientLib.Devices.AllPlay
{
    /// <summary>
    /// Contains the current state of the media player
    /// </summary>
    public class PlayState
    {
        internal PlayState(AllJoynMessageArgStructure arg)
        {
            // (sxuuuiia(ssssxsssa{ss}a{sv}v))
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

            // Note: Still a few more values for custom data not being processed here
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

        /// <summary>
        /// Gets the currently playing media
        /// </summary>
        public Media CurrentMedia { get; private set; }

        /// <summary>
        /// Gets the next upcoming media
        /// </summary>
        public Media NextMedia { get; private set; }

        /// <summary>
        /// Gets the current media state
        /// </summary>
        public MediaState State { get; }

        /// <summary>
        /// Gets the current position
        /// </summary>
        public TimeSpan Position { get; }

        /// <summary>
        /// Gets the current sample rate
        /// </summary>
        public uint SampleRate { get; }

        /// <summary>
        /// Gets the number of audio channels
        /// </summary>
        public uint AudioChannels { get; }

        /// <summary>
        /// Gets the current bits per sample
        /// </summary>
        public uint BitsPerSample { get; }

        /// <summary>
        /// Gets the index of the currently playing item
        /// </summary>
        public int IndexCurrentItem { get; }

        /// <summary>
        /// Gets the index of the next item in the playlist
        /// </summary>
        public int IndexNextItem { get; }
    }
}
