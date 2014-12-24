using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Response
{
    [DataContract]
    public class Geometory
    {
        [DataMember(Name = "type")]
        public string Type { get; private set; }

        [DataMember(Name = "coordinates")]
        public List<double> Coordinates { get; private set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(
                string.Format(
                "type: {0}",
                Type));
            if (Coordinates != null)
            {
                foreach (var c in Coordinates)
                {
                    builder.Append(
                        string.Format(
                        "{0}\n",
                        c));
                }
            }
            return builder.ToString();
        }
    }
}
