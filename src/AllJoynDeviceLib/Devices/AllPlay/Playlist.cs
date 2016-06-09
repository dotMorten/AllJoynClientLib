using System.Collections.Generic;
using System.Linq;
using DeviceProviders;

namespace AllJoynClientLib.Devices.AllPlay
{
    /// <summary>
    /// A media playlist
    /// </summary>
    public class Playlist
    {
        internal Playlist(IList<object> result)
        {
            var items = result[0] as IList<object>;
            Items = items.Select(i => new Media(i as AllJoynMessageArgStructure));
            ControllerType = result[1] as string;
            PlaylistUserData = result[2] as string;
        }

        /// <summary>
        /// Gets the items in the playlist.
        /// </summary>
        public IEnumerable<Media> Items { get; }

        /// <summary>
        /// Gets a user-defined string to identify the controller type.
        /// </summary>
        public string ControllerType { get; }

        /// <summary>
        /// Gets the user-defined data
        /// </summary>
        public string PlaylistUserData { get; }
    }
}