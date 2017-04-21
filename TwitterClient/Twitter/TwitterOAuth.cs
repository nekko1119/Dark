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

        public string GenerateAuthorizationHeader(
            HttpMethod method, Uri uri, List<QueryParameter> queryParameters, string callbackUri)
        {
            var oauthParameters = new List<QueryParameter>
            {
                // 必須パラメータ
                new QueryParameter() { Name = "oauth_consumer_key", Value = oauth.ConsumerKey },
                new QueryParameter() { Name = "oauth_signature_method", Value = "HMAC-SHA1" },
                new QueryParameter() {
                    Name = "oauth_timestamp", Value = ((long) CurrentTime().TotalSeconds).ToString()
                },
                new QueryParameter() { Name = "oauth_version", Value = "1.0" },
                new QueryParameter() { Name = "oauth_nonce", Value = GenerateNonce(34) }
            };

            // オプションパラメータ
            if (!string.IsNullOrEmpty(AccessToken))
            {
                oauthParameters.Add(new QueryParameter() { Name = "oauth_token", Value = AccessToken });
            }
            if (!string.IsNullOrEmpty(callbackUri))
            {
                oauthParameters.Add(new QueryParameter() { Name = "oauth_callback", Value = callbackUri });
            }

            var queryAndOAuthParameter = queryParameters.Concat(oauthParameters)
                .Select(p => new QueryParameter() { Name = p.Name, Value = OAuth.UriEncode(p.Value) })
                .ToList();
            queryAndOAuthParameter.Sort();

            var request = new HttpRequestMessage(method, uri);
            var oauthData = oauth.GenerateOAuthData(request, queryAndOAuthParameter);
            var oauthKey = oauth.GenerateOAuthKey();
            var signature = oauth.MakeHashCode(oauthKey, oauthData);

            int index = queryAndOAuthParameter.FindIndex(q => q.Name == "oauth_callback");
            if (index != -1)
            {
                queryAndOAuthParameter[index].Value = OAuth.UriEncode(queryAndOAuthParameter[index].Value);
            }
            queryAndOAuthParameter.Add(
                new QueryParameter() { Name = "oauth_signature", Value = OAuth.UriEncode(signature) }
            );

            var headerParameter = new StringBuilder();
            var length = queryAndOAuthParameter.Count;
            for (int i = 0; i < length; i++)
            {
                var p = queryAndOAuthParameter[i];
                headerParameter.Append(String.Format("{0}={1}", p.Name, p.Value));
                if (i < length - 1)
                {
                    headerParameter.Append(",");
                }
            }
            Console.WriteLine(headerParameter.ToString());
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

            var table = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
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
