using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Twitter
{
    class TwitterOAuth
    {
        private OAuth oauth;

        public string ConsumerKey
        {
            get { return oauth.ConsumerKey; }
        }

        public string ConsumerSecret
        {
            get { return oauth.ConsumerSecret; }
        }

        public string AccessToken
        {
            get { return oauth.Token; }
            set { oauth.Token = value; }
        }

        public string AccessTokenSecret
        {
            get { return oauth.TokenSecret; }
            set { oauth.TokenSecret = value; }
        }

        public TwitterOAuth(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            oauth = new OAuth(consumerKey, consumerSecret)
            {
                Token = accessToken,
                TokenSecret = accessTokenSecret
            };
        }

        public TwitterOAuth(string consumerKey, string consumerSecret)
            : this(consumerKey, consumerSecret, "", "")
        {
        }

        public string GenerateAuthorizationHeader(HttpMethod method, Uri uri, List<QueryParameter> queryParameters)
        {
            return GenerateAuthorizationHeader(method, uri, queryParameters, null);
        }

        public string GenerateAuthorizationHeader(HttpMethod method, Uri uri, List<QueryParameter> queryParameters, string callbackUri)
        {
            var oauthParameters = new List<QueryParameter>();

            // 必須パラメータ
            oauthParameters.Add(new QueryParameter() { Name = "oauth_consumer_key", Value = oauth.ConsumerKey });
            oauthParameters.Add(new QueryParameter() { Name = "oauth_signature_method", Value = "HMAC-SHA1" });
            oauthParameters.Add(new QueryParameter() { Name = "oauth_timestamp", Value = ((long)CurrentTime().TotalSeconds).ToString() });
            oauthParameters.Add(new QueryParameter() { Name = "oauth_version", Value = "1.0" });
            oauthParameters.Add(new QueryParameter() { Name = "oauth_nonce", Value = GenerateNonce(34) });

            // オプションパラメータ
            if (!string.IsNullOrEmpty(AccessToken))
            {
                oauthParameters.Add(new QueryParameter() { Name = "oauth_token", Value = AccessToken });
            }
            if (!string.IsNullOrEmpty(callbackUri))
            {
                oauthParameters.Add(new QueryParameter() { Name = "oauth_callback", Value = callbackUri });
            }

            queryParameters.AddRange(oauthParameters);

            var request = new HttpRequestMessage(method, uri);
            var oauthData = oauth.GenerateOAuthData(request, queryParameters);
            var oauthKey = oauth.GenerateOAuthKey();
            var signature = oauth.MakeHashCode(oauthKey, oauthData);
            oauthParameters.Add(new QueryParameter() { Name = "oauth_signature", Value = OAuth.UriEncode(signature) });
            oauthParameters.Sort();

            int index = oauthParameters.FindIndex(q => q.Name == "oauth_callback");
            if (index != -1)
            {
                oauthParameters[index].Value = OAuth.UriEncode(queryParameters[index].Value);
            }

            var headerParameter = new StringBuilder();
            var length = oauthParameters.Count;
            for (int i = 0; i < length; i++)
            {
                var p = oauthParameters[i];
                headerParameter.Append(String.Format("{0}=\"{1}\"", p.Name, p.Value));
                if (i < length - 1)
                {
                    headerParameter.Append(", ");
                }
            }
            return headerParameter.ToString();
        }

        private TimeSpan CurrentTime()
        {
            var unixEpock = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return DateTime.Now.ToUniversalTime() - unixEpock;
        }

        private string GenerateNonce(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length < 0");
            }

            string table = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var nonce = new StringBuilder(length);
            var random = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(table.Length);
                char c = table[index];
                nonce.Append(c);
            }
            return nonce.ToString();
        }
    }
}
