using System.Collections.Generic;
using System.Linq;

namespace AllJoynClientLib.Devices.AllPlay
{

    /// <summary>
    /// Zone information.
    /// </summary>
    public class ZoneInfo
    {
        internal ZoneInfo(object o)
        {
            // ZoneInfo = struct of zone id+timestamp+variant
            // The variant is either a string of lead player name, or dictionary of {players' known-name + timestamp}
        }

        /// <summary>
        /// Gets the zone identifier.
        /// </summary>
        /// <value>The zone identifier.</value>
        public string ZoneId { get; }

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public int TimeStamp { get; }
    }
}
