using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Response
{
    // TODO 不完全
    [DataContract]
    public class EntityForTweets
    {
        [DataMember(Name = "hashtags")]
        public List<Tag> HashTags { get; private set; }

        [DataMember(Name = "symbols")]
        public List<Tag> Symbols { get; private set; }

        [DataMember(Name = "urls")]
        public List<URL> Urls { get; private set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("hashtags:\n");
            if (HashTags != null)
            {
                foreach (var h in HashTags)
                {
                    builder.Append(h + "\n");
                }
            }
            builder.Append("symbols:\n");
            if (Symbols != null)
            {
                foreach (var s in Symbols)
                {
                    builder.Append(s + "\n");
                }
            }
            builder.Append("urls:\n");
            if (Urls != null)
            {
                foreach (var u in Urls)
                {
                    builder.Append(u + "\n");
                }
            }
            return builder.ToString();
        }
    }
}
