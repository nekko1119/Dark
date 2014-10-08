using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

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
				return "/1.1";
			}
		}

		public TwitterClient()
		{
			var docment = XElement.Load("../../../../access_token.xml");
			twitterOAuth = new TwitterOAuth
				(
				"r1xe4MXNfCg4aJRvmaNig5IOw",
				"DaczKjZivPFHOZ1DnwMPCD05EomwGLMMtxqs4McU8Sm7CtPb2b",
				docment.Element("key").Value,
				docment.Element("secret").Value
				);
			client = new HttpClient();
			client.BaseAddress = new Uri(BaseUri);
		}

		public HttpResponseMessage GetProfile(string screenName)
		{
			var targetUri = BaseUri + ApiVersion + "/users/show";
			var queryParameters = new List<QueryParameter>();
			queryParameters.Add(new QueryParameter("screen_name", screenName));
			return Get(targetUri, queryParameters);
		}

		public HttpResponseMessage PostRequestToken()
		{
			var targetUri = BaseUri + "/oauth/request_token";
			return Post(targetUri, new List<QueryParameter>(), new Uri("https://www.google.co.jp/"));
		}

		private HttpResponseMessage Get(string targetUri, List<QueryParameter> queryParameters)
		{
			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("OAuth", twitterOAuth.MakeAuthorizationHeader(new HttpMethod("GET"), new Uri(targetUri), queryParameters));
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			System.Console.WriteLine(client.DefaultRequestHeaders);
			return client.GetAsync(targetUri + "?" + QueryParameter.GenerateQueryParameterString(queryParameters)).Result;
		}

		private HttpResponseMessage Post(string targetUri, List<QueryParameter> queryParameters, Uri callback)
		{
			twitterOAuth.AccessToken = "";
			twitterOAuth.AccessTokenSecret = "";
			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("OAuth", twitterOAuth.MakeAuthorizationHeader(new HttpMethod("POST"), new Uri(targetUri), queryParameters));
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
			System.Console.WriteLine(client.DefaultRequestHeaders);
			return client.PostAsync(targetUri, null).Result;
		}
    }
}
