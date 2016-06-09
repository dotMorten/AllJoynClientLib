namespace AllJoynClientLib.Devices.AllPlay
{
    /// <summary>
    /// Event args for the <see cref="ZoneManager.PlayerReady"/> event.
    /// </summary>
    public class PlayerReadyEventArgs
    {
        internal PlayerReadyEventArgs(ulong resumeLatency)
        {
            ResumeLatency = resumeLatency;
        }

        /// <summary>
        /// Gets the resume latency.
        /// </summary>
        /// <value>The resume latency.</value>
        public ulong ResumeLatency { get; }
    }
}