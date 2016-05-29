using DeviceProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllJoynClientLib.Devices.AllPlay
{
    public class Mcu
    {
        private readonly IInterface mcu;

        internal Mcu(IInterface mcu)
        {
            this.mcu = mcu;
        }

        public Task PlayItemAsync(string url, string title, string artist, string thumbnailUrl,
            TimeSpan duration, string album, string genre)
        {
            return mcu.InvokeMethodAsync("PlayItem", url, title, artist, thumbnailUrl, 
                (long)duration.TotalMilliseconds, album, genre);
        }

        public Task AdvanceLoopModeAsync()
        {
            return mcu.InvokeMethodAsync("AdvanceLoopMode");
        }

        public Task ToggleShuffleModeAsync()
        {
            return mcu.InvokeMethodAsync("ToggleShuffleMode");
        }

        public async Task<string> GetCurrentItemUrlAsync()
        {
            var result = await mcu.InvokeMethodAsync("GetCurrentItemUrl").ConfigureAwait(false);
            return (string)result[0];
        }

        public Task SetExternalSourceAsync(string name, bool interruptible, bool volumeCtrlEnabled)
        {
            return mcu.InvokeMethodAsync("SetExternalSource", name, interruptible, volumeCtrlEnabled);
        }

        public Task<UInt16> GetVersionVersionAsync()
        {
            return mcu.GetPropertyAsync<UInt16>("Version");
        }
    }
}
