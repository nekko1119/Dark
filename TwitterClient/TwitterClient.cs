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
    public class TwitterClient : IDisposable
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
			client = new HttpClient();
			client.BaseAddress = new Uri(BaseUri);
		}

		public async Task<Response.ProfileResponse> GetProfile(string screenName)
		{
			var targetUri = BaseUri + "/" + ApiVersion + "/users/show";
			
			var queryParameters = new List<QueryParameter>();
			queryParameters.Add(new QueryParameter() { Name = "screen_name", Value = screenName });
			
			var message =  await Get(targetUri, queryParameters);
			
			var serializer = new DataContractJsonSerializer(typeof(Response.ProfileResponse));
			using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(message.Content.ReadAsStringAsync().Result)))
			{
				var respone = (Response.ProfileResponse)serializer.ReadObject(memoryStream);
				respone.StatusCode = new Response.StatusCode()
				{
					Code = message.StatusCode,
					Message = message.ReasonPhrase
				};
				System.Console.WriteLine(message.Content.ReadAsStringAsync().Result);
				return respone;
			}
		}

		public async Task<Response.RequestTokenResponse> GetRequestToken()
		{
			var targetUri = BaseUri + "/oauth/request_token";
			
			var message = await Post(targetUri, new List<QueryParameter>(), "");
			
			var nvc = HttpUtility.ParseQueryString(await message.Content.ReadAsStringAsync());
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
		}

		public async Task<Response.AccessTokenResponse> GetAccessToken(string oauthVerifier)
		{
			var targetUri = BaseUri + "/oauth/access_token";
			
			var queryParameters = new List<QueryParameter>();
			queryParameters.Add(new QueryParameter() { Name = "oauth_verifier", Value = oauthVerifier });

			var message = await Get(targetUri, queryParameters);
			System.Console.WriteLine(await message.Content.ReadAsStringAsync());

			var nvc = HttpUtility.ParseQueryString(await message.Content.ReadAsStringAsync());
			var response = new Response.AccessTokenResponse
			(
				nvc["oauth_token"],
				nvc["oauth_token_secret"],
				long.Parse(nvc["user_id"]),
				nvc["screen_name"]
			);
			response.StatusCode = new Response.StatusCode()
			{
				Code = message.StatusCode,
				Message = message.ReasonPhrase
			};
			return response;
		}

		private async Task<HttpResponseMessage> Get(string targetUri, List<QueryParameter> queryParameters)
		{
			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("OAuth", twitterOAuth.MakeAuthorizationHeader(new HttpMethod("GET"), new Uri(targetUri), queryParameters));
			if (!client.DefaultRequestHeaders.Accept.Contains(new MediaTypeHeaderValue("application/json")))
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			}

			System.Console.WriteLine(client.DefaultRequestHeaders);
			return await client.GetAsync(targetUri + "?" + QueryParameter.GenerateQueryParameterString(queryParameters));
		}

		private async Task<HttpResponseMessage> Post(string targetUri, List<QueryParameter> bodyParameters, string callbackUrl)
		{
			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("OAuth", twitterOAuth.MakeAuthorizationHeader(new HttpMethod("POST"), new Uri(targetUri), bodyParameters, callbackUrl));
			if (!client.DefaultRequestHeaders.Accept.Contains(new MediaTypeHeaderValue("application/json")))
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			}
			bodyParameters.Sort();
			var dict = bodyParameters.ToDictionary(p => p.Name, p => p.Value);
			HttpContent content = new FormUrlEncodedContent(dict);

			System.Console.WriteLine(client.DefaultRequestHeaders);
			return await client.PostAsync(targetUri, null);
		}

		public void Dispose()
		{
			client.Dispose();
		}
	}
}
