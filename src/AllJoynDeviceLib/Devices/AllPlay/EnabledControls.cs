using System;
using System.Collections.Generic;

namespace AllJoynClientLib.Devices.AllPlay
{
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
        public bool LoopMode { get; }
        public bool Next { get; }
        public bool Previous { get; }
        public bool Seek { get; }
        public bool ShuffleMode { get; }
    }
}
