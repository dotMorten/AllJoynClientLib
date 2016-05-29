using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceProviders;

namespace AllJoynClientLib.Devices.AllPlay
{
    /// <summary>
    /// This is used to control the playing of media content.
    /// </summary>
    public partial class MediaPlayer
    {
        private readonly IInterface mediaPlayer;
        private bool IsAlljoyn;
        private bool IsAllPlay;
        internal MediaPlayer(IInterface mediaPlayer)
        {
            this.mediaPlayer = mediaPlayer;
            IsAllPlay = mediaPlayer.Name == "net.allplay.MediaPlayer";
            IsAlljoyn = mediaPlayer.Name == "org.allseen.media.control.mediaPlayer";
            InitSignals();
        }

        /// <summary>
        /// Get information about the player.
        /// </summary>
        /// <returns></returns>
        public async Task<PlayerInfo> GetPlayerInfoAsync()
        {
            var result = await mediaPlayer.InvokeMethodAsync("GetPlayerInfo").ConfigureAwait(false);
            return new PlayerInfo(result);
        }

        public Task GetPlaylistInfoAsync()
        {
            throw new NotImplementedException("TODO");
            //var result = await mediaPlayer.InvokeMethodAsync("GetPlaylistInfo").ConfigureAwait(false);
            //return new PlaylistInfo(result);
        }

        /// <summary>
        /// Update the current play list.
        /// </summary>
        /// <param name="items">Items in the play list.</param>
        /// <param name="index">New index of the current item.</param>
        /// <param name="controllerType">User-defined string to identify the controller type.</param>
        /// <param name="playlistUserData">User-defined information.</param>
        /// <returns></returns>
        public Task UpdatePlaylistAsync(IEnumerable<Media> items, Int32 index, string controllerType, string playlistUserData)
        {
            //playlistItems: a(ssssxsssa{ss}a{sv}v)
            //index: i
            //controllerType: s
            //playlistUserData: s
            return mediaPlayer.InvokeMethodAsync("UpdatePlaylist",
                new List<object>(items.Select(i => i.ToParameter())), index, controllerType ?? "ABC", playlistUserData ?? "ABC");
        }

        /// <summary>
        /// Current play state information.
        /// </summary>
        /// <returns></returns>
        public async Task<PlayState> GetPlayStateAsync()
        {
            //<property name="PlayState" type="(sxuuuiia(ssssxsssa{ss}a{sv}v))" access="read"/>
            var result = await mediaPlayer.GetPropertyAsync("PlayState");
            var state = result as AllJoynMessageArgStructure;
            return new PlayState(state);
        }

        /// <summary>
        /// Gets the Shuffle mode setting
        /// </summary>
        /// <returns></returns>
        public async Task<ShuffleMode> GetShuffleModeAsync()
        {
            var mode = await mediaPlayer.GetPropertyAsync("ShuffleMode").ConfigureAwait(false) as string;
            return StringToShuffleMode(mode);
        }

        /// <summary>
        /// Sets the Shuffle mode setting
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Task SetShuffleModeAsync(ShuffleMode mode)
        {
            return mediaPlayer.SetPropertyAsync("ShuffleMode", mode.ToString().ToUpper());
        }

        private static ShuffleMode StringToShuffleMode(string mode)
        {
            switch (mode)
            {
                case "SHUFFLE": return ShuffleMode.Shuffle;
                case "LINEAR":
                default:
                    return ShuffleMode.Linear;
            }
        }

        /// <summary>
        /// Gets the Loop mode setting
        /// </summary>
        /// <returns></returns>
        public async Task<LoopMode> GetLoopModeAsync()
        {
            var mode = await mediaPlayer.GetPropertyAsync("LoopMode").ConfigureAwait(false) as string;
            return StringToLoopMode(mode);
        }

        /// <summary>
        /// Sets the Loop mode setting
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Task SetLoopModeAsync(LoopMode mode)
        {
            return mediaPlayer.SetPropertyAsync("LoopMode", mode.ToString().ToUpper());
        }
        private static LoopMode StringToLoopMode(string mode)
        {
            switch (mode)
            {
                case "ALL": return LoopMode.All;
                case "ONE": return LoopMode.One;
                case "NONE":
                default:
                    return LoopMode.None;
            }
        }

        public async Task<EnabledControls> GetEnabledControlsAsync()
        {
            //if (IsAlljoyn)
            //    return true;
            var ctrls = await mediaPlayer.GetPropertyAsync("EnabledControls").ConfigureAwait(false);
            return new EnabledControls(ctrls as IList<KeyValuePair<object, object>>);
        }

        public Task<bool> GetInterruptibleAsync()
        {
            //if (IsAlljoyn)
            //    return true;
            return mediaPlayer.GetPropertyAsync<bool>("Interruptible");
        }

        /// <summary>
        /// Start playing the next item in the playlist.
        /// </summary>
        /// <returns></returns>
        public Task NextAsync()
        {
            return mediaPlayer.InvokeMethodAsync("Next");
        }

        /// <summary>
        /// If currently at the start of the item, then play the previous 
        /// item (if it exists). Otherwise, rewind to the start of the item.
        /// </summary>
        /// <returns></returns>
        public Task PreviousAsync()
        {
            return mediaPlayer.InvokeMethodAsync("Previous");
        }

        /// <summary>
        /// Always move to the previous item regardless of the position in the current item.
        /// </summary>
        /// <returns></returns>
        public Task ForcedPreviousAsync()
        {
            return mediaPlayer.InvokeMethodAsync("ForcedPrevious");
        }

        /// <summary>
        /// Stop the playback and set the playback postion to the start of the item.
        /// </summary>
        /// <returns></returns>
        public Task StopAsync()
        {
            return mediaPlayer.InvokeMethodAsync("Stop");
        }

        /// <summary>
        /// Pause the playback.
        /// </summary>
        /// <returns></returns>
        public Task PauseAsync()
        {
            return mediaPlayer.InvokeMethodAsync("Pause");
        }

        /// <summary>
        /// Resume the playback
        /// </summary>
        /// <returns></returns>
        public Task ResumeAsync()
        {
            return mediaPlayer.InvokeMethodAsync("Resume");
        }

        /// <summary>
        /// Start playing the item at the index at the specified start position.
        /// If Play() is called while the playlist is playing, it will restart 
        /// playback from the start of the current track.
        /// </summary>
        /// <param name="itemIndex">Index in the playlist of the item to play</param>
        /// <param name="startPosition">Start position</param>
        /// <param name="pauseStateOnly">Indicates whether to start streaming(false) or just pause at the
        /// specific position(true). This is used for transferring of playlists.</param>
        /// <returns></returns>
        public Task PlayAsync(int itemIndex, TimeSpan startPosition, bool pauseStateOnly)
        {
            return mediaPlayer.InvokeMethodAsync("Play", itemIndex, (long)startPosition.TotalMilliseconds, pauseStateOnly);
        }

        /// <summary>
        /// Set the current postion in a track.
        /// </summary>
        /// <param name="position">Position offset in the play item</param>
        /// <returns></returns>
        public Task SetPosition(TimeSpan position)
        {
            return mediaPlayer.InvokeMethodAsync("SetPosition", (long)position.TotalMilliseconds);
        }

        /// <summary>
        /// Read the current play list.
        /// </summary>
        /// <returns></returns>
        public async Task<Playlist> GetPlaylistAsync()
        {
            var result = await mediaPlayer.InvokeMethodAsync("GetPlaylist").ConfigureAwait(false);
            return new Playlist(result);
        }
    }
}
