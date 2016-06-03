using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllJoynClientLib.Devices
{
    public abstract class DeviceClient
    {
        protected DeviceClient(DeviceProviders.IService service)
        {
            Service = service;
        }

        protected DeviceProviders.IInterface GetInterface(string name)
        {
            var items = Service.Objects;
            if (items != null)
            {
                foreach (var item in items)
                {
                    var i = item.GetInterface(name);
                    if (i != null) return i;
                }
            }
            return null;
        }

        internal void DeviceLost()
        {
            OnDeviceLost();
        }
        protected virtual void OnDeviceLost()
        {
        }

        public DeviceProviders.IService Service { get; }

        public string Name
        {
            get
            {
                return Service?.Name;
            }
        }

        public string DeviceName
        {
            get
            {
                return Service?.AboutData?.DeviceName;
            }
        }

        public string ModelNumber
        {
            get
            {
                return Service?.AboutData?.ModelNumber;
            }
        }

        public string DeviceId
        {
            get
            {
                return Service?.AboutData?.DeviceId;
            }
        }

        public string Manufacturer
        {
            get
            {
                return Service?.AboutData?.Manufacturer;
            }
        }

        public string Description
        {
            get
            {
                return Service?.AboutData?.Description;
            }
        }

        public async Task<Windows.UI.Xaml.Media.Imaging.BitmapSource> GetIconAsync()
        {
            var result = await Service?.AboutData?.GetIconAsync();
            if (result != null)
            {
                if (result.Content != null && result.Content.Any())
                {
                    var bmp = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                    using (var mv = new ReadOnlyByteStream(result.Content))
                    {
                        bmp.SetSource(mv.AsRandomAccessStream());
                    }
                    return bmp;
                }
                Uri uri = null;
                if(Uri.TryCreate(result.Url, UriKind.Absolute, out uri))
                {
                    return new Windows.UI.Xaml.Media.Imaging.BitmapImage(uri);
                }
            }
            return null;
        }


        private class ReadOnlyByteStream : System.IO.Stream
        {
            readonly IReadOnlyList<byte> _data;
            public ReadOnlyByteStream(IReadOnlyList<byte> data)
            {
                _data = data;
            }
            public override bool CanRead { get { return true; } }

            public override bool CanSeek { get { return true; } }

            public override bool CanWrite { get { return false; } }

            public override long Length { get { return _data.Count; } }

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
                        break;
                    buffer[i + offset] = _data[(int)Position++];
                }
                return i;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                if (origin == SeekOrigin.Begin)
                    Position = offset;
                else if (origin == SeekOrigin.Current)
                    Position += offset;
                else if (origin == SeekOrigin.End)
                    Position = _data.Count + offset;
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
