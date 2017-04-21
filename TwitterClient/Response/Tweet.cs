using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Response
{
    // TODO 不完全
    [DataContract]
    public class Tweet
    {
        [DataMember(Name = "created_at")]
        private string CreateAtString
        {
            get { return CreateAt.ToString(); }
            set
            {
                CreateAt = DateTime.ParseExact(value, "ddd MMM dd HH:mm:ss zzz yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);
            }
        }

        public DateTime CreateAt { get; private set; }

        [DataMember(Name = "id")]
        public long Id { get; private set; }

        [DataMember(Name = "id_str")]
        public string IdStr { get; private set; }

        [DataMember(Name = "text")]
        public string Text { get; private set; }

        [DataMember(Name = "source")]
        public string Source { get; private set; }

        [DataMember(Name = "trancated")]
        public bool Trancated { get; private set; }

        [DataMember(Name = "in_reply_to_status_id")]
        public long? InReplyToStatusId { get; private set; }

        [DataMember(Name = "in_reply_to_status_id_str")]
        public string InReplyToStatusIdStr { get; private set; }

        [DataMember(Name = "in_reply_to_user_id")]
        public long? InReplyToUserId { get; private set; }

        [DataMember(Name = "in_reply_to_user_id_str")]
        public string InReplyToUserIdStr { get; private set; }

        [DataMember(Name = "in_reply_to_screen_name")]
        public string InReplyToScreenName { get; private set; }

        [DataMember(Name = "geo")]
        public Geometory Geo { get; private set; }

        [DataMember(Name = "coordinates")]
        public Geometory Coordinates { get; private set; }

        [DataMember(Name = "place")]
        public Place Place { get; private set; }

        [DataMember(Name = "contributors")]
        public List<long> Contributors { get; private set; }

        [DataMember(Name = "retweet_count")]
        public long RetweetCount { get; private set; }

        [DataMember(Name = "favorite_count")]
        public long FavoriteCount { get; private set; }

        [DataMember(Name = "entities")]
        public EntityForTweets Entities { get; private set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
