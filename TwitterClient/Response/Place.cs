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
    public class Place
    {
        [DataMember(Name = "id")]
        private string IdStr
        {
            get { return Id.ToString(); }
            set { Id = Convert.ToInt64(value, 16); }
        }

        public long Id { get; private set; }

        [DataMember(Name = "url")]
        private string UrlString
        {
            get { return Url.ToString(); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                Url = new Uri(value);
            }
        }

        public Uri Url { get; private set; }

        [DataMember(Name = "place_type")]
        public string PlaceType { get; private set; }

        [DataMember(Name = "name")]
        public string Name { get; private set; }

        [DataMember(Name = "full_name")]
        public string FullName { get; private set; }

        [DataMember(Name = "country_code")]
        public string CountryCode { get; private set; }

        [DataMember(Name = "country")]
        public string Country { get; private set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
