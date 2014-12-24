using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Response
{
    [DataContract]
    public class UrlInfo
    {
        [DataMember(Name = "urls")]
        public List<URL> Urls { get; private set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
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
