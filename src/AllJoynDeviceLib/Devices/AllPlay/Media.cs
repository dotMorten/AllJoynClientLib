using System;
using System.Collections.Generic;
using System.Linq;
using DeviceProviders;
using System.Collections.ObjectModel;

namespace AllJoynClientLib.Devices.AllPlay
{
    /// <summary>
    /// A media entry item
    /// </summary>
    public class Media
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Media"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="title">The title.</param>
        /// <param name="artist">The artist.</param>
        /// <param name="thumbnailUrl">The thumbnail URL.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="album">The album.</param>
        /// <param name="genre">The genre.</param>
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

        internal Media(AllJoynMessageArgStructure s)
        {
            // "(ssssxsssa{ss}a{sv}v)
            Url = s[0] as string;
            Title = s[1] as string;
            Artist = s[2] as string;
            ThumbnailUrl = s[3] is string ? new Uri(s[3] as string) : null;
            Duration = TimeSpan.FromMilliseconds((long)s[4]);
            MediaType = s[5] as string;
            Album = s[6] as string;
            Genre = s[7] as string;
            var otherDataArg = s[8] as IList<KeyValuePair<object, object>>;
            OtherData = new Dictionary<string, string>();
            foreach (var item in otherDataArg)
            {
                OtherData.Add((string)item.Key, (string)item.Value);
            }
            var mediumArg = s[9] as IList<KeyValuePair<object, object>>;
            MediumDesc = new Dictionary<string, object>();
            foreach (var item in mediumArg)
            {
                MediumDesc.Add((string)item.Key, item.Value);
            }
            UserData = s[10] as AllJoynMessageArgVariant;
        }

        internal IList<object> ToParameter()
        {
            var t = AllJoynTypeDefinition.CreateTypeDefintions("(ssssxsssa{ss}a{sv}v)").First();
            var argument = new AllJoynMessageArgStructure(t);

            // var count = paramDef.Fields.Count;
            // string[] types = paramDef.Fields.Select(f => f.Type.ToString()).ToArray();
            // List<object> argument = new List<object>();
            argument.Add(Url);
            argument.Add(Title ?? " ");
            argument.Add(Artist ?? " ");
            argument.Add(ThumbnailUrl?.OriginalString ?? " ");
            argument.Add((long)Duration.TotalMilliseconds);
            argument.Add(MediaType ?? " ");
            argument.Add(Album ?? " ");
            argument.Add(Genre ?? " ");

            // Other data: a{ss}
            var otherData = new Dictionary<object, object>();
            if (OtherData != null)
            {
                foreach (var item in OtherData)
                {
                    otherData.Add(item.Key, item.Value);
                }
            }

            argument.Add(otherData.ToList());

            // medium desc: a{sv}
            var mediumDesc = new Dictionary<object, object>();
            if (OtherData != null)
            {
                foreach (var item in OtherData)
                {
                    mediumDesc.Add(item.Key, item.Value);
                }
            }

            argument.Add(mediumDesc.ToList());

            // AllJoynMessageArgVariant v = new AllJoynMessageArgVariant();
            // var arg = new DeviceProviders.AllJoynMessageArgVariant(AllJoynTypeDefinition.CreateTypeDefintions("v").First(), 0);
            // arg.Value = "upnp";
            argument.Add(UserData ?? "upnp"); // Variant: userdata
            return argument;
        }

        /// <summary>
        /// Gets the url to the item
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Gets the title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the artist
        /// </summary>
        public string Artist { get; }

        /// <summary>
        /// Gets the media thumbnail url
        /// </summary>
        public Uri ThumbnailUrl { get; }

        /// <summary>
        /// Gets the duration
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Gets the type of media
        /// </summary>
        public string MediaType { get; }

        /// <summary>
        /// Gets the album
        /// </summary>
        public string Album { get; }

        /// <summary>
        /// Gets the genre
        /// </summary>
        public string Genre { get; }

        /// <summary>
        /// Gets or sets additional data.
        /// </summary>
        /// <value>The additional data.</value>
        public IDictionary<string, string> OtherData { get; set; }

        /// <summary>
        /// Gets or sets additional media descriptors.
        /// </summary>
        /// <value>The medium desc.</value>
        public IDictionary<string, object> MediumDesc { get; set; }

        private object UserData { get; set; }
    }
}
