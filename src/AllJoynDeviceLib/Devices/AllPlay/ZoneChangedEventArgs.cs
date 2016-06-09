namespace AllJoynClientLib.Devices.AllPlay
{
    /// <summary>
    /// Event args for the <see cref="ZoneManager.OnZoneChanged"/> event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public sealed class ZoneChangedEventArgs : System.EventArgs
    {
        /*
            <arg name="zoneId" type="s" direction="out"/>
            <arg name="timestamp" type="i" direction="out"/>
            <arg name="slaves" type="a{si}" direction="out"/>
        */
    }
}