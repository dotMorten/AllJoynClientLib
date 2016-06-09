using System.Collections.Generic;

namespace AllJoynClientLib.Devices.AllPlay
{
    /// <summary>
    /// Holds the current state of enabled playback controls
    /// </summary>
    public class EnabledControls
    {
        internal EnabledControls(IList<KeyValuePair<object, object>> value)
        {
            foreach (var pair in value)
            {
                switch ((string)pair.Key)
                {
                    case "loopMode":
                        LoopMode = (bool)pair.Value; break;
                    case "next":
                        Next = (bool)pair.Value; break;
                    case "previous":
                        Previous = (bool)pair.Value; break;
                    case "seek":
                        Seek = (bool)pair.Value; break;
                    case "shuffleMode":
                        ShuffleMode = (bool)pair.Value; break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether changing loop mode is possible.
        /// </summary>
        public bool LoopMode { get; }

        /// <summary>
        /// Gets a value indicating whether skipping to next track is possible.
        /// </summary>
        public bool Next { get; }

        /// <summary>
        /// Gets a value indicating whether skipping to previous track is possible.
        /// </summary>
        public bool Previous { get; }

        /// <summary>
        /// Gets a value indicating whether seeking is possible.
        /// </summary>
        public bool Seek { get; }

        /// <summary>
        /// Gets a value indicating whether changing shuffle mode is currently possible.
        /// </summary>
        public bool ShuffleMode { get; }
    }
}
