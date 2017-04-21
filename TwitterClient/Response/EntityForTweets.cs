using Newtonsoft.Json;
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
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
