using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Response
{
    [DataContract]
    public class Tag
    {
        [DataMember(Name = "text")]
        public string Text { get; private set; }

        [DataMember(Name = "indices")]
        public List<long> Indices { get; private set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
