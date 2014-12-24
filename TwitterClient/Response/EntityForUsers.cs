using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Response
{
    [DataContract]
    public class EntityForUsers
    {
        [DataMember(Name = "url")]
        public UrlInfo Url { get; private set; }

        [DataMember(Name = "description")]
        public UrlInfo Description { get; private set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("url:\nurls:\n");
            if (Url != null && Url.Urls != null)
            {
                foreach (var url in Url.Urls)
                {
                    builder.Append(url + "\n");
                }
            }
            builder.Append("description:\nurls:\n");
            if (Description != null && Description.Urls != null)
            {
                foreach (var desc in Description.Urls)
                {
                    builder.Append(desc + "\n");
                }
            }
            return builder.ToString();
        }
    }
}
