using System;
using System.Threading.Tasks;
using DeviceProviders;

namespace AllJoynClientLib.Devices.AllPlay
{
    /// <summary>
    /// MCU (undocumented AllPlay interface, so doc in this class is questionable)
    /// </summary>
    public class Mcu
    {
        private readonly IInterface mcu;

        internal Mcu(IInterface mcu)
        {
            this.mcu = mcu;
        }

        /// <summary>
        /// Plays an item
        /// </summary>
        /// <param name="url">Url to the source</param>
        /// <param name="title">Title</param>
        /// <param name="artist">Artist</param>
        /// <param name="thumbnailUrl">Url for the thumbnail</param>
        /// <param name="duration">Duration</param>
        /// <param name="album">Album</param>
        /// <param name="genre">Genre</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task PlayItemAsync(string url, string title, string artist, string thumbnailUrl,
            TimeSpan duration, string album, string genre)
        {
            return mcu.InvokeMethodAsync("PlayItem", url, title, artist, thumbnailUrl,
                (long)duration.TotalMilliseconds, album, genre);
        }

        /// <summary>
        /// Selects the next loop mode
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task AdvanceLoopModeAsync()
        {
            return mcu.InvokeMethodAsync("AdvanceLoopMode");
        }

        /// <summary>
        /// Toggles the shuffle mode
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task ToggleShuffleModeAsync()
        {
            return mcu.InvokeMethodAsync("ToggleShuffleMode");
        }

        /// <summary>
        /// Gets the URL of the currently playing item
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<string> GetCurrentItemUrlAsync()
        {
            var result = await mcu.InvokeMethodAsync("GetCurrentItemUrl").ConfigureAwait(false);
            return (string)result[0];
        }

        /// <summary>
        /// No idea what this does
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="interruptible">interruptible</param>
        /// <param name="volumeCtrlEnabled">volume control enabled</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task SetExternalSourceAsync(string name, bool interruptible, bool volumeCtrlEnabled)
        {
            return mcu.InvokeMethodAsync("SetExternalSource", name, interruptible, volumeCtrlEnabled);
        }

        /// <summary>
        /// Gets the version of the MCU interface
        /// </summary>
        /// <returns>The version of the interface</returns>
        public Task<ushort> GetVersionVersionAsync()
        {
            return mcu.GetPropertyAsync<ushort>("Version");
        }
    }
}
