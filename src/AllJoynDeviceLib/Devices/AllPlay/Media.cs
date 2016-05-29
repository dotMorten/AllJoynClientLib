using DeviceProviders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AllJoynClientLib.Devices.AllPlay
{
    public class Media
    {
        public Media(string url, string title = null, string artist = null,
            Uri thumbnailUrl = null, TimeSpan? duration = null,
            string mediaType = null, string album = null, string genre = null)
        {
            Url = url;
            Title = title;
            Artist = artist;
            ThumbnailUrl = thumbnailUrl;
            Duration = duration == null ? TimeSpan.Zero : duration.Value;
            MediaType = mediaType;
            Album = album;
            Genre = genre;
        }
        internal IList<object> ToParameter()
        {
            var t = AllJoynTypeDefinition.CreateTypeDefintions("(ssssxsssa{ss}a{sv}v)").First();
            var argument = new AllJoynMessageArgStructure(t);
            
            //var count = paramDef.Fields.Count;
            //string[] types = paramDef.Fields.Select(f => f.Type.ToString()).ToArray();
            //List<object> argument = new List<object>();
            argument.Add(Url);
            argument.Add(Title ?? " ");
            argument.Add(Artist ?? " ");
            argument.Add(ThumbnailUrl?.OriginalString ?? " ");
            argument.Add((long)Duration.TotalMilliseconds);
            argument.Add(MediaType ?? " ");
            argument.Add(Album ?? " ");
            argument.Add(Genre ?? " ");
            argument.Add(new Dictionary<object, object>().ToList()); //Other data: a{ss}
            argument.Add(new Dictionary<object, object>().ToList()); //medium desc: a{sv}
            //AllJoynMessageArgVariant v = AllJoynMessageArgVariant();
            //var arg = new DeviceProviders.AllJoynMessageArgVariant(AllJoynTypeDefinition.CreateTypeDefintions("v").First(), 0);            
            argument.Add(null); //Variant: userdata
            return argument;
        }
        internal Media(AllJoynMessageArgStructure s)
        {
            Url = s[0] as string;
            Title = s[1] as string;
            Artist = s[2] as string;
            ThumbnailUrl = s[3] is string ? new Uri(s[3] as string) : null;
            Duration = TimeSpan.FromMilliseconds((long)s[4]);
            MediaType = s[5] as string;
            Album = s[6] as string;
            Genre = s[7] as string;
            otherData = s[8];
            mediumDesc = s[9];
            userData = s[10];
        }
        private object otherData;
        private object mediumDesc;
        private object userData;
        public string Url { get; }
        public string Title { get; }
        public string Artist { get; }
        public Uri ThumbnailUrl { get; }
        public TimeSpan Duration { get; }
        public string MediaType { get; }
        public string Album { get; }
        public string Genre { get; }
    }
}
