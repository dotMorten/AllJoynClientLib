using System.Collections.Generic;
using System.Linq;

namespace AllJoynClientLib.Devices.AllPlay
{
    /// <summary>
    /// Information about the player.
    /// </summary>
    public class PlayerInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerInfo"/> class.
        /// </summary>
        /// <param name="result">The object collection returned from the service.</param>
        internal PlayerInfo(IList<object> result)
        {
            DisplayName = result[0] as string;
            Capabilities = (result[1] as IList<object>).OfType<string>().ToArray();
            MaximumVolume = (int)result[2];
            ZoneInfo = new ZoneInfo(result[3] as DeviceProviders.AllJoynMessageArgStructure);
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; }

        /// <summary>
        /// Gets an array of the MIME types (string) that the player supports, e.g., {“audio/mpeg”,video/mp4”,”image/jpeg”}.
        /// </summary>
        public string[] Capabilities { get; } // Note: In alljoyn it's a key/value pair

        /// <summary>
        /// Gets the maximum settable volume.
        /// </summary>
        public int MaximumVolume { get; }

        /// <summary>
        /// Gets the zone information.
        /// </summary>
        /// <value>The zone information.</value>
        public object ZoneInfo { get; }
    }
}
