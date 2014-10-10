using System;
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
    public class TwitterClient
    {
		private HttpClient client;
		private TwitterOAuth twitterOAuth;

		public string BaseUri
		{
			get
			{
				return "https://api.twitter.com";
			}
		}

		public string ApiVersion
		{
			get
			{
				return "1.1";
			}
		}

		public string AccessToken
		{
			get
			{
				return twitterOAuth.AccessToken;
			}
			set
			{
				twitterOAuth.AccessToken = value;
			}
		}

		public string AccessTokenSecret
		{
			get
			{
				return twitterOAuth.AccessTokenSecret;
			}
			set
			{
				twitterOAuth.AccessTokenSecret = value;
			}
		}

		public TwitterClient(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
		{
			twitterOAuth = new TwitterOAuth
				(
				consumerKey, consumerSecret,
				accessToken, accessSecret
				);
			client = new HttpClient();
			client.BaseAddress = new Uri(BaseUri);
		}

		public HttpResponseMessage GetProfile(string screenName)
		{
			var targetUri = BaseUri + "/" + ApiVersion + "/users/show";
			var queryParameters = new List<QueryParameter>();
			queryParameters.Add(new QueryParameter() { Name = "screen_name", Value = screenName });
			return Get(targetUri, queryParameters);
		}

		public Response.RequestTokenResponse PostRequestToken()
		{
			var targetUri = BaseUri + "/oauth/request_token";
			var message = Post(targetUri, new List<QueryParameter>());
			var nvc = HttpUtility.ParseQueryString(message.Content.ReadAsStringAsync().Result);
			var response = new Response.RequestTokenResponse
				(
				nvc["oauth_token"],
				nvc["oauth_token_secret"],
				bool.Parse(nvc["oauth_callback_confirmed"])
				);
			response.StatusCode = new Response.StatusCode()
			{
				Code = message.StatusCode,
				Message = message.ReasonPhrase
			};
			System.Console.WriteLine(message.Content.ReadAsStringAsync().Result);
			return response;
			//var serializer = new DataContractJsonSerializer(typeof(Response.RequestTokenResponse));
			//using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(response.Content.ReadAsStringAsync().Result)))
			//{
			//	return (Response.RequestTokenResponse)serializer.ReadObject(memoryStream);
			//}
		}

		private HttpResponseMessage Get(string targetUri, List<QueryParameter> queryParameters)
		{
			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("OAuth", twitterOAuth.MakeAuthorizationHeader(new HttpMethod("GET"), new Uri(targetUri), queryParameters));
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			System.Console.WriteLine(client.DefaultRequestHeaders);
			return client.GetAsync(targetUri + "?" + QueryParameter.GenerateQueryParameterString(queryParameters)).Result;
		}

		private HttpResponseMessage Post(string targetUri, List<QueryParameter> bodyParameters)
		{
			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("OAuth", twitterOAuth.MakeAuthorizationHeader(new HttpMethod("POST"), new Uri(targetUri), bodyParameters));
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			bodyParameters.Sort();
			var dict = bodyParameters.ToDictionary(p => p.Name, p => p.Value);
			HttpContent content = new FormUrlEncodedContent(dict);

			System.Console.WriteLine(client.DefaultRequestHeaders);
			return client.PostAsync(targetUri, null).Result;
		}
    }
}
