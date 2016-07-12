using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeviceProviders;

namespace AllJoynClientLib.Devices.AllPlay
{
    /// <summary>
    /// This stores information about the zone the player belongs to. A player can
    /// only be part of one zone at any given time
    /// </summary>
    public class ZoneManager
    {
        private readonly IInterface zoneManager;

        internal ZoneManager(IInterface zoneManager)
        {
            this.zoneManager = zoneManager;
        }

        /// <summary>
        /// Gets a value indicating whether the zone manager is enabled
        /// </summary>
        /// <returns>True if enabled</returns>
        public Task<bool> GetIsEnabledAsync()
        {
            return zoneManager.GetPropertyAsync<bool>("Enabled");
        }

        /// <summary>
        /// Gets the version of the ZoneManager interface
        /// </summary>
        /// <returns>The ZoneManager interface version</returns>
        public Task<ushort> GetVersionAsync()
        {
            return zoneManager.GetPropertyAsync<ushort>("Version");
        }

        /// <summary>
        /// Create a zone of players. Each player is added as a player to the main player
        /// </summary>
        /// <param name="slaves">List of player names</param>
        /// <returns>The created zone</returns>
        public Task<ZoneInfo> CreateZone(IEnumerable<string> slaves)
        {
            throw new NotImplementedException("TODO");
        }

        /* No reason to expose in a client lib :
         * This is used by the zone lead player to tell other slaves who leads
        /// <summary>
        /// Assigns the zone lead
        /// </summary>
        /// <param name="zoneId">The ID of the zone</param>
        /// <param name="timeServerIp">The time server to use for synchronization</param>
        /// <param name="timeServerPort">Port of the time server</param>
        /// <returns>The time</returns>
        public Task<DateTime> SetZoneLead(string zoneId, string timeServerIp, ushort timeServerPort)
        {
            throw new NotImplementedException("TODO");
        }*/
        }

        /// <summary>
        /// Raised if the enabled state has changed
        /// </summary>
        public event EventHandler<bool> EnabledChanged;

        /// <summary>
        /// Raised if the zone has changed
        /// </summary>
        public event EventHandler<ZoneChangedEventArgs> OnZoneChanged;

        /// <summary>
        /// Raised when the player is ready
        /// </summary>
        public event EventHandler<PlayerReadyEventArgs> PlayerReady;

        /// <summary>
        /// Raised when a slave has run out of data
        /// </summary>
        public event EventHandler SlaveOutOfData;

        /// <summary>
        /// Raised when a slaves state has changed
        /// </summary>
        public event EventHandler<SlaveStateEventArgs> SlaveState;
    }
}
