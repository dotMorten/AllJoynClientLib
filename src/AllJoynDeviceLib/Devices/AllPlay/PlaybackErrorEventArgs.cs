using System;
using System.Collections.Generic;
using System.Linq;
using DeviceProviders;

namespace AllJoynClientLib.Devices.AllPlay
{
    /// <summary>
    /// Event argument for the <see cref="MediaPlayer.OnPlaybackError"/> event.
    /// </summary>
    public sealed class PlaybackErrorEventArgs : EventArgs
    {
        internal PlaybackErrorEventArgs(IList<object> args)
        {
            Index = (int)args[0];
            Error = args[1] as string;
            Description = args[2] as string;
        }

        /// <summary>
        /// Gets the index in the playlist for which this error occured
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the error message
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Gets a description of the error
        /// </summary>
        public string Description { get; }
    }
}
