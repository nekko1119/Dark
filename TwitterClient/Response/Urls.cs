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
            var builder = new StringBuilder();
            builder.Append(
                string.Format(
                "url: {0}\nexpanded_url: {1}\ndisplay_url: {2}\n",
                Url,
                ExpandedUrl,
                DisplayUrl));

            builder.Append("indices:\n");
            if (Indices != null)
            {
                foreach (var i in Indices)
                {
                    builder.Append(i + "\n");
                }
            }

            return builder.ToString();
        }
    }
}
