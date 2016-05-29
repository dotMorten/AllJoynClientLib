using DeviceProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Task<bool> GetIsEnabledAsync()
        {
            throw new NotImplementedException("TODO");
        }
        public Task<UInt16> GetVersionVersionAsync()
        {
            return zoneManager.GetPropertyAsync<UInt16>("Version");
        }
        public Task<ZoneInfo> CreateZone(IEnumerable<string> slaves)
        {
            throw new NotImplementedException("TODO");
        }
        public Task<DateTime> SetZoneLead(string zoneId, string timeServerIp, UInt16 timeServerPort)
        {
            throw new NotImplementedException("TODO");
        }

        public event EventHandler EnabledChanged;
        public event EventHandler OnZoneChanged;
        public event EventHandler PlayerReady;
        public event EventHandler SlaveOutOfData;
        public event EventHandler SlaveState;
    }
}
