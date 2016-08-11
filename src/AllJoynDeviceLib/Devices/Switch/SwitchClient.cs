using System;
using System.Threading.Tasks;

namespace AllJoynClientLib.Devices
{
    /// <summary>
    /// Abstract class for any type of switch.
    /// </summary>
    /// <seealso cref="AllJoynClientLib.Devices.DeviceClient" />
    public abstract class SwitchClient : DeviceClient
    {
        internal SwitchClient(DeviceProviders.IService service) : base(service)
        {
        }

        /// <summary>
        /// Sets the on/off state.
        /// </summary>
        /// <param name="isOn">On if set to <c>true</c> otherwise off.</param>
        /// <returns>Task.</returns>
        public abstract Task SetOnOffAsync(bool isOn);

        /// <summary>
        /// Gets the on state.
        /// </summary>
        /// <returns><c>true</c> if on</returns>
        public abstract Task<bool> GetOnOffAsync();

        /// <summary>
        /// Raised if the switch toggles its on/off state
        /// </summary>
        /// <seealso cref="CanRaiseToggledEvent"/>
        public event EventHandler<bool> Toggled;

        /// <summary>
        /// Gets or sets a value indicating whether this device can raise the toggled event
        /// </summary>
        public bool CanRaiseToggledEvent { get; protected set; } = false;

        /// <summary>
        /// Raises the <see cref="Toggled"/> event.
        /// </summary>
        /// <param name="onOffState">New on/off state</param>
        protected void OnToggled(bool onOffState)
        {
            Toggled?.Invoke(this, onOffState);
        }
    }
}
