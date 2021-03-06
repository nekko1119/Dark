﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Twitter
{
    /// <summary>
    /// Twitter APIにアクセスするためのクライアントクラスです
    /// </summary>
    public class TwitterClient : IDisposable
    {
        private HttpClient client;
        private TwitterOAuth twitterOAuth;

        public string BaseUri
        {
            get { return "https://api.twitter.com"; }
        }

        public string ApiVersion
        {
            get { return "1.1"; }
        }

        public string AccessToken
        {
            get { return twitterOAuth.AccessToken; }
            set { twitterOAuth.AccessToken = value; }
        }

        public string AccessTokenSecret
        {
            get { return twitterOAuth.AccessTokenSecret; }
            set { twitterOAuth.AccessTokenSecret = value; }
        }

        public TwitterClient(string consumerKey, string consumerSecret)
            : this(consumerKey, consumerSecret, "", "")
        {
        }

        public TwitterClient(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
        {
            twitterOAuth = new TwitterOAuth
            (
                consumerKey, consumerSecret,
                accessToken, accessSecret
            );
            client = new HttpClient()
            {
                BaseAddress = new Uri(BaseUri)
            };
        }

        /// <summary>
        /// ユーザのプロフィールを取得します
        /// </summary>
        /// <param name="screenName">取得したいユーザのスクリーンネーム</param>
        /// <returns>ユーザのプロフィール</returns>
        public async Task<Response.ProfileResponse> GetProfile(string screenName)
        {
            var targetUri = BaseUri + "/" + ApiVersion + "/users/show";

            var queryParameters = new List<QueryParameter>
            {
                new QueryParameter() { Name = "screen_name", Value = screenName }
            };
            var message = await Get(targetUri, queryParameters);
            if (message.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine(message);
                return null;
            }

            var serializer = new DataContractJsonSerializer(typeof(Response.ProfileResponse));
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(message.Content.ReadAsStringAsync().Result)))
            {
                var response = (Response.ProfileResponse)serializer.ReadObject(memoryStream);
                response.StatusCode = new Response.StatusCode()
                {
                    Code = message.StatusCode,
                    Message = message.ReasonPhrase
                };
                return response;
            }
        }

        /// <summary>
        /// ツイートする
        /// </summary>
        /// <param name="content">ツイート文言</param>
        /// <returns></returns>
        public async Task<string> Tweet(string content)
        {
            var targetUri = BaseUri + "/" + ApiVersion + "/statuses/update";

            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }
            var bodyParameters = new List<QueryParameter>
            {
                new QueryParameter() { Name = "status", Value = content }
            };

            var message = await Post(targetUri, bodyParameters, null);
            return message.ToString();
        }

        /// <summary>
        /// リクエストトークンを取得します
        /// </summary>
        /// <returns>リクエストトークン</returns>
        public async Task<Response.RequestTokenResponse> GetRequestToken()
        {
            if (!string.IsNullOrEmpty(AccessToken) || !string.IsNullOrEmpty(AccessTokenSecret))
            {
                throw new InvalidAccessTokenStateException(
                    "既にアクセストークンが設定されています", AccessToken, AccessTokenSecret);
            }

            var targetUri = BaseUri + "/oauth/request_token";

            var message = await Post(targetUri, new List<QueryParameter>(), "");

            if (message.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine(message);
                return null;
            }

            var nvc = HttpUtility.ParseQueryString(await message.Content.ReadAsStringAsync());
            var response = new Response.RequestTokenResponse
            (
                nvc["oauth_token"],
                nvc["oauth_token_secret"],
                bool.Parse(nvc["oauth_callback_confirmed"])
            )
            {
                StatusCode = new Response.StatusCode()
                {
                    Code = message.StatusCode,
                    Message = message.ReasonPhrase
                }
            };
            AccessToken = response.OAuthToken;
            AccessTokenSecret = response.OAuthTokenSecret;
            return response;
        }

        /// <summary>
        /// アクセストークンを取得します。クライアントのAccessToken, AccessTokenSecretも変更されます。
        /// </summary>
        /// <param name="oauthVerifier">ブラウザから取得したPINコード</param>
        /// <returns>アクセストークン</returns>
        public async Task<Response.AccessTokenResponse> GetAccessToken(string oauthVerifier)
        {
            var targetUri = BaseUri + "/oauth/access_token";

            var queryParameters = new List<QueryParameter>
            {
                new QueryParameter() { Name = "oauth_verifier", Value = oauthVerifier }
            };
            var message = await Get(targetUri, queryParameters);
            if (message.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine(message);
                return null;
            }

            var nvc = HttpUtility.ParseQueryString(await message.Content.ReadAsStringAsync());
            var response = new Response.AccessTokenResponse
            (
                nvc["oauth_token"],
                nvc["oauth_token_secret"],
                long.Parse(nvc["user_id"]),
                nvc["screen_name"]
            )
            {
                StatusCode = new Response.StatusCode()
                {
                    Code = message.StatusCode,
                    Message = message.ReasonPhrase
                }
            };
            AccessToken = response.OAuthToken;
            AccessTokenSecret = response.OAuthTokenSecret;

            return response;
        }

        private async Task<HttpResponseMessage> Get(string targetUri, List<QueryParameter> queryParameters)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "OAuth", twitterOAuth.GenerateAuthorizationHeader(
                        new HttpMethod("GET"), new Uri(targetUri), queryParameters));
            if (!client.DefaultRequestHeaders.Accept.Contains(new MediaTypeHeaderValue("application/json")))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            return await client.GetAsync(targetUri + "?" + QueryParameter.Generate(queryParameters));
        }

        private async Task<HttpResponseMessage> Post(string targetUri, List<QueryParameter> bodyParameters, string callbackUrl)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "OAuth", twitterOAuth.GenerateAuthorizationHeader(
                        new HttpMethod("POST"), new Uri(targetUri), bodyParameters, callbackUrl));
            if (!client.DefaultRequestHeaders.Accept.Contains(new MediaTypeHeaderValue("application/json")))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            if (bodyParameters != null && bodyParameters.Count != 0)
            {
                var dict = bodyParameters.ToDictionary(p => p.Name, p => p.Value);
                HttpContent content = new FormUrlEncodedContent(dict);
                return await client.PostAsync(targetUri, content);
            }
            return await client.PostAsync(targetUri, null);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
