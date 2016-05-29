using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllJoynClientLib.Devices.AllPlay
{
    public class PlayerInfo
    {
        public PlayerInfo(IList<object> result)
        {
            DisplayName = result[0] as string;
            Capabilities = (result[1] as IList<object>).OfType<string>().ToArray();
            MaximumVolume = (int)result[2];
            ZoneInfo = new ZoneInfo(result[3]);
        }
        public string DisplayName { get; }
        /// <summary>
        /// Array of the MIME types (string) that the player supports, e.g., {“audio/mpeg”,video/mp4”,”image/jpeg”}.
        /// </summary>
        public string[] Capabilities { get; } //Note: In alljoyn it's a key/value pair
        /// <summary>
        /// Maximum setable volume.
        /// </summary>
        public int MaximumVolume { get; }
        public object ZoneInfo { get; }
    }
    public class ZoneInfo
    {
        public ZoneInfo(object o)
        {
            //ZoneInfo = struct of zone id+timestamp+variant
            //The variant is either a string of lead player name, or dictionary of {players' known-name + timestamp}
        }
        public string ZoneId { get; }
        public int TimeStamp { get; }

    }
}
