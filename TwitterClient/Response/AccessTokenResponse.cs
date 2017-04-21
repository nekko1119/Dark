using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Response
{
    [DataContract]
    public class AccessTokenResponse
    {
        public StatusCode StatusCode { get; set; }

        [DataMember(Name = "oauth_token")]
        public string OAuthToken { get; private set; }

        [DataMember(Name = "oauth_token_secret")]
        public string OAuthTokenSecret { get; private set; }

        [DataMember(Name = "user_id")]
        public long UserId { get; private set; }

        [DataMember(Name = "screen_name")]
        public string ScreenName { get; private set; }

        public AccessTokenResponse()
        {
        }

        public AccessTokenResponse(string oauthToken, string oauthTokenSecret, long userId, string screenName)
        {
            OAuthToken = oauthToken;
            OAuthTokenSecret = oauthTokenSecret;
            UserId = userId;
            ScreenName = screenName;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
