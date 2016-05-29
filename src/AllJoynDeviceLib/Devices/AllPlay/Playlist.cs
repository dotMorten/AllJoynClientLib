using DeviceProviders;
using System.Collections.Generic;
using System.Linq;

namespace AllJoynClientLib.Devices.AllPlay
{
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
        /// Items in the play list.
        /// </summary>
        public IEnumerable<Media> Items { get; }
        /// <summary>
        /// User-defined string to identify the controller type.
        /// </summary>
        public string ControllerType { get; }
        /// <summary>
        /// User-defined data
        /// </summary>
        public string PlaylistUserData { get; }
    }
}