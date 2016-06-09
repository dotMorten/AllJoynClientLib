using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AllJoynClientLib.Devices
{
    /// <summary>
    /// The base device client object
    /// </summary>
    public abstract class DeviceClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceClient"/> class.
        /// </summary>
        /// <param name="service">The AllJoyn service.</param>
        protected DeviceClient(DeviceProviders.IService service)
        {
            Service = service;
        }

        /// <summary>
        /// Searches for the specified interface across all bus objects.
        /// </summary>
        /// <param name="name">The name of the interface.</param>
        /// <returns>DeviceProviders.IInterface.</returns>
        protected DeviceProviders.IInterface GetInterface(string name)
        {
            var items = Service.Objects;
            if (items != null)
            {
                foreach (var item in items)
                {
                    var i = item.GetInterface(name);
                    if (i != null)
                    {
                        return i;
                    }
                }
            }

            return null;
        }

        internal void DeviceLost()
        {
            OnDeviceLost();
        }

        /// <summary>
        /// Called when the device was lost from the network
        /// </summary>
        /// <remarks>
        /// Use this clean up.
        /// </remarks>
        protected virtual void OnDeviceLost()
        {
        }

        /// <summary>
        /// Gets a reference to the service for this device
        /// </summary>
        public DeviceProviders.IService Service { get; }

        /// <summary>
        /// Gets the name of the service
        /// </summary>
        public string Name
        {
            get
            {
                return Service?.Name;
            }
        }

        /// <summary>
        /// Gets the name of the device
        /// </summary>
        public string DeviceName
        {
            get
            {
                return Service?.AboutData?.DeviceName;
            }
        }

        /// <summary>
        /// Gets the model number of the device
        /// </summary>
        public string ModelNumber
        {
            get
            {
                return Service?.AboutData?.ModelNumber;
            }
        }

        /// <summary>
        /// Gets the ID of the device
        /// </summary>
        public string DeviceId
        {
            get
            {
                return Service?.AboutData?.DeviceId;
            }
        }

        /// <summary>
        /// Gets the device manufacturer
        /// </summary>
        public string Manufacturer
        {
            get
            {
                return Service?.AboutData?.Manufacturer;
            }
        }

        /// <summary>
        /// Gets a description for the device
        /// </summary>
        public string Description
        {
            get
            {
                return Service?.AboutData?.Description;
            }
        }

        /// <summary>
        /// Gets an icon for the device
        /// </summary>
        /// <returns>The icon</returns>
        public Task<Windows.UI.Xaml.Media.Imaging.BitmapSource> GetIconAsync()
        {
            // Currently causes a finalizer crash in DeviceProviders so just returning null for now
            // var result = await Service?.AboutData?.GetIconAsync();
            // if (result != null)
            // {
            //    if (result.Content != null && result.Content.Any())
            //    {
            //        var bmp = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
            //        using (var mv = new ReadOnlyByteStream(result.Content))
            //        {
            //            await bmp.SetSourceAsync(mv.AsRandomAccessStream());
            //        }
            //        return bmp;
            //    }
            //    Uri uri = null;
            //    if(Uri.TryCreate(result.Url, UriKind.Absolute, out uri))
            //    {
            //        return new Windows.UI.Xaml.Media.Imaging.BitmapImage(uri);
            //    }
            // }
            return Task.FromResult<Windows.UI.Xaml.Media.Imaging.BitmapSource>(null);
        }

        private class ReadOnlyByteStream : System.IO.Stream
        {
            private readonly IReadOnlyList<byte> _data;

            public ReadOnlyByteStream(IReadOnlyList<byte> data)
            {
                _data = data;
            }

            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanSeek
            {
                get { return true; }
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override long Length
            {
                get { return _data.Count; }
            }

            public override long Position { get; set; }

            public override void Flush()
            {
                throw new NotSupportedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                int i = 0;
                for (; i < count; i++)
                {
                    if (Position == _data.Count)
                    {
                        break;
                    }

                    buffer[i + offset] = _data[(int)Position++];
                }

                return i;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                if (origin == SeekOrigin.Begin)
                {
                    Position = offset;
                }
                else if (origin == SeekOrigin.Current)
                {
                    Position += offset;
                }
                else if (origin == SeekOrigin.End)
                {
                    Position = _data.Count + offset;
                }

                return Position;
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }
        }
    }
}
