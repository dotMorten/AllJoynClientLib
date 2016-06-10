using System.Collections.Generic;
using System.Linq;

namespace AllJoynClientLib.Devices.AllPlay
{
    /// <summary>
    /// Zone information.
    /// </summary>
    public class ZoneInfo
    {
        internal ZoneInfo(DeviceProviders.AllJoynMessageArgStructure arg)
        {
            ZoneId = (string)arg[0];
            TimeStamp = (int)arg[1];
            LeadPlayerName = arg[2] as string; // Name of master this is slaved to
            var slavesArg = arg[2] as DeviceProviders.AllJoynMessageArgVariant; // Names of slaves to this player
            if (slavesArg != null)
            {
                var value = slavesArg.Value as IList<KeyValuePair<object,object>>;
                var players = new Dictionary<string, int>();
                foreach (var item in value)
                {
                    players.Add((string)item.Key, (int)item.Value);
                }
                PlayerNames = new System.Collections.ObjectModel.ReadOnlyDictionary<string, int>(players);
            }
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

        /// <summary>
        /// Gets the name of the lead player if this is running as a slave.
        /// </summary>
        /// <value>The name of the lead player.</value>
        public string LeadPlayerName { get; }

        /// <summary>
        /// Gets the name of player slaves if this is running as a lead.
        /// </summary>
        /// <value>The player names.</value>
        public IReadOnlyDictionary<string, int> PlayerNames { get; }
    }
}
