using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Twitter
{
	/// <summary>
	/// Twitter APIにアクセスするためのクライアントクラスです
	/// </summary>
    public class TwitterClient
    {
		private HttpClient client;
		private readonly string consumerKey;
		private readonly string consumerSecret;
		private readonly string accessToken;
		private readonly string accessTokenSecret;

		public string BaseUri
		{
			get
			{
				return "http://api.twitter.com/1.1";
			}
		}

		public TwitterClient()
		{
			consumerKey = System.Configuration.ConfigurationManager.AppSettings["consumerKey"];
			consumerSecret = System.Configuration.ConfigurationManager.AppSettings["consumerSecret"];
			var docment = XElement.Load("../../../../access_token.xml");
			accessToken = docment.Element("key").Value;
			accessTokenSecret = docment.Element("secret").Value;
			System.Console.WriteLine(accessToken);
			System.Console.WriteLine(accessTokenSecret);
			client = new HttpClient();
			client.BaseAddress = new Uri(BaseUri);
			var query = HttpUtility.ParseQueryString(string.Empty);
			//query["hoge"] = "あ";
			//query["foo"] = "google";
			//System.Console.WriteLine(query.ToString());
			//var builder = new UriBuilder(client.BaseAddress);
			//builder.Query = query.ToString();
			//System.Console.WriteLine(builder.ToString());
			//client.GetAsync(builder.ToString());
		}

		private string AuthorizationKey()
		{
			return UriEncode(consumerSecret) + "&" + UriEncode(accessTokenSecret);
		}

		private string AuthorizationData(NameValueCollection queryParam)
		{
			var authornizationData = new NameValueCollection();
			authornizationData["oauth_consumer_key"] = consumerKey;
			authornizationData["oauth_consumer_key"] = consumerKey;
			authornizationData["oauth_nonce"] = CurrentTimeMilliseconds().Milliseconds.ToString();
			authornizationData["oauth_signature_method"] = "HMAC-SHA1";
			authornizationData["oauth_timestamp"] = CurrentTimeMilliseconds().Seconds.ToString();
			authornizationData["oauth_version"] = "1.0";
			System.Console.WriteLine(authornizationData.ToString());
			return "";
		}

		private TimeSpan CurrentTimeMilliseconds()
		{
			var unixEpock = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			var currentTime = DateTime.Now.ToUniversalTime();
			return urrentTime - unixEpock;
		}

		/// <summary>
		/// 引数に与えられた文字列をUTF-8でURIエンコードします
		/// </summary>
		/// <param name="str">エンコードしたい文字列</param>
		/// <returns>エンコードされた文字列</returns>
		private string UriEncode(string str)
		{
			return HttpUtility.UrlEncode(str, Encoding.UTF8).ToUpper();
		}
    }
}
