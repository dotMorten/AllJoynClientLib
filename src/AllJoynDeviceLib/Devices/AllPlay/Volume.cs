using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeviceProviders;

namespace AllJoynClientLib.Devices.AllPlay
{
    /// <summary>
    /// The Volume interface is implemented on a multimedia device
    /// to allow an AllJoyn client to control its audio volume.
    /// </summary>
    public class Volume
    {
        private readonly IInterface volume;
        private ISignal _enabledChangedSignal;
        private ISignal _muteChangedSignal;
        private ISignal volumeChangedSignal;

        internal Volume(IInterface volume)
        {
            this.volume = volume;
            _enabledChangedSignal = volume.GetSignal(nameof(EnabledChanged));
            _enabledChangedSignal.SignalRaised += EnabledChangedSignal_SignalRaised;
            _muteChangedSignal = volume.GetSignal(nameof(MuteChanged));
            _muteChangedSignal.SignalRaised += MuteChangedSignal_SignalRaised;
            volumeChangedSignal = volume.GetSignal(nameof(VolumeChanged));
            volumeChangedSignal.SignalRaised += VolumeChangedSignal_SignalRaised;
        }

        /// <summary>
        /// Indicates if the volume control is enabled.
        /// </summary>
        /// <returns>Value indicating whether volume control is enabled</returns>
        public Task<bool> GetVolumeEnabledAsync()
        {
            return volume.GetPropertyAsync<bool>("Enabled");
        }

        /// <summary>
        /// The volume range as 3 values: low, high and minimum increments.
        /// </summary>
        /// <returns>Gets the volume range</returns>
        public Task<short[]> GetVolumeRangeAsync()
        {
            return volume.GetPropertyAsync<short[]>("VolumeRange");
        }

        /// <summary>
        /// The volume's mute state.
        /// </summary>
        /// <returns>The current mute state (true if muted)</returns>
        public Task<bool> GetMuteAsync()
        {
            return volume.GetPropertyAsync<bool>("Mute");
        }

        /// <summary>
        /// The volume's mute state.
        /// </summary>
        /// <param name="mute"><c>true</c>to mute, <c>false</c> to unmute</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task SetMuteAsync(bool mute)
        {
            return volume.SetPropertyAsync("Mute", mute);
        }

        /// <summary>
        /// Gets the current volume.
        /// </summary>
        /// <returns>The current volume</returns>
        /// <seealso cref="GetVolumeRangeAsync"/>
        public Task<short> GetVolumeAsync()
        {
            return volume.GetPropertyAsync<short>("Volume");
        }

        /// <summary>
        /// Gets the current volume.
        /// </summary>
        /// <param name="volume">Volume</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <seealso cref="GetVolumeRangeAsync"/>
        public Task SetVolumeAsync(short volume)
        {
            return this.volume.SetPropertyAsync("Volume", volume);
        }

        /// <summary>
        /// Adjust the volume by the given number. The adjustment can be up
        /// (positive value) or down (negative value)
        /// </summary>
        /// <param name="delta">Number of increments to adjust.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task AdjustVolumeAsync(short delta)
        {
            return volume.InvokeMethodAsync("AdjustVolumeAsync", delta);
        }

        /// <summary>
        /// Adjust the volume to a percentage of the maximum value.
        /// </summary>
        /// <param name="delta">Change amount</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// The change has floating point values between -1.0 and 1.0 to represent volume changes between -100% to 100%.
        /// A positive value(respectively negative), will increase(respectively decrease) the volume by the percentage of the “remaining range” towards the maximum(respectively minimum) value, i.e.difference between the current volume and the maximum(respectively minimum) volume.
        /// For example, when the volume range is [0-100]
        ///         and we want to adjust by +50%:
        /// - If the current volume is 25, the increment will be:
        /// “(100-25)*50%=75*0.5=38” (once rounded) so the new volume will be 63.
        /// - Another adjustment by +50% will be “(100-63)*0.5=19” to a volume of 82.
        /// If we want instead to adjust by -50%, the decrement would be “(25-0)*0.5=13” to a volume of 12, and another adjustment by -50% would be “(12-0)*0.5=6” to a volume of 6.
        /// This behavior provides a better user experience when changing the volume of multiple speakers(group). At the same time, although each speaker has a different starting point, all the players will reach 100% (or 0%) at the same time.
        /// </remarks>
        public Task AdjustVolumePercentAsync(double delta)
        {
            return volume.InvokeMethodAsync("AdjustVolumePercentAsync", delta);
        }

        private void VolumeChangedSignal_SignalRaised(ISignal sender, IList<object> args)
        {
            VolumeChanged?.Invoke(this, (short)args[0]);
        }

        private void EnabledChangedSignal_SignalRaised(ISignal sender, IList<object> args)
        {
            EnabledChanged?.Invoke(this, (bool)args[0]);
        }

        private void MuteChangedSignal_SignalRaised(ISignal sender, IList<object> args)
        {
            MuteChanged?.Invoke(this, (bool)args[0]);
        }

        /// <summary>
        /// The volume control enable value has changed.
        /// </summary>
        public event EventHandler<bool> EnabledChanged;

        /// <summary>
        /// The mute value has changed.
        /// </summary>
        public event EventHandler<bool> MuteChanged;

        /// <summary>
        /// The volume has changed
        /// </summary>
        public event EventHandler<short> VolumeChanged;
    }
}
