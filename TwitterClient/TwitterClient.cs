using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
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
				return "https://api.twitter.com/1.1";
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

		public Task<HttpResponseMessage> GetProfile(string screenName)
		{
			var targetUri = BaseUri + "/users/show";
			var queryParameters = new List<QueryParameter>();
			queryParameters.Add(new QueryParameter("screen_name", screenName));
			client.DefaultRequestHeaders.Authorization =
				new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", twitterOAuth.MakeAuthorizationHeader(new HttpMethod("GET"), new Uri(targetUri), queryParameters));
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
			return client.GetAsync(targetUri + "?" + QueryParameter.GenerateQueryParameterString(queryParameters));
		}
    }
}
