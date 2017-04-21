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
    public class EntityForUsers
    {
        [DataMember(Name = "url")]
        public UrlInfo Url { get; private set; }

        [DataMember(Name = "description")]
        public UrlInfo Description { get; private set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
