using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Response
{
    [DataContract]
    public class URL
    {
        [DataMember(Name = "url")]
        private string UrlString
        {
            get { return Url.ToString(); }
            set { Url = new Uri(value); }
        }

        public Uri Url { get; private set; }

        [DataMember(Name = "expanded_url")]
        private string ExpandedUrlString
        {
            get { return ExpandedUrl.ToString(); }
            set { ExpandedUrl = new Uri(value); }
        }

        public Uri ExpandedUrl { get; private set; }

        [DataMember(Name = "display_url")]
        public string DisplayUrl { get; private set; }

        [DataMember(Name = "indices")]
        public List<long> Indices { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
