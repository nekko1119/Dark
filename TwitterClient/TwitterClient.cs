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
		private enum HttpMethod
		{
			GET,
			POST,
			PUT,
			DELETE,
			HEAD,
			OPTION
		}

		private HttpClient client;
		private readonly string consumerKey;
		private readonly string consumerSecret;
		private readonly string accessToken;
		private readonly string accessTokenSecret;

		public string BaseUri
		{
			get
			{
				return "https://api.twitter.com/1.1";
			}
		}

		public TwitterClient()
		{
			consumerKey = "r1xe4MXNfCg4aJRvmaNig5IOw";
			consumerSecret = "DaczKjZivPFHOZ1DnwMPCD05EomwGLMMtxqs4McU8Sm7CtPb2b";
			var docment = XElement.Load("../../../../access_token.xml");
			accessToken = docment.Element("key").Value;
			accessTokenSecret = docment.Element("secret").Value;
			client = new HttpClient();
			client.BaseAddress = new Uri(BaseUri);
			//var query = HttpUtility.ParseQueryString(string.Empty);
			//query["hoge"] = "あ";
			//query["foo"] = "google";
			//System.Console.WriteLine(query.ToString());
			//var builder = new UriBuilder(client.BaseAddress);
			//builder.Query = query.ToString();
			//System.Console.WriteLine(builder.ToString());
			//client.GetAsync(builder.ToString());
		}

		public Task<HttpResponseMessage> GetProfile(string screenName)
		{
			var query = HttpUtility.ParseQueryString(String.Empty);
			query["screen_name"] = screenName;
			
			var authorizationParam = GetAutorizationParameter();
			var signatureKey = AuthorizationKey();
			var signatureData = AuthorizationData(MergeParams(authorizationParam, query), HttpMethod.GET, BaseUri + @"/users/show");
			var signature = HMacSha1(signatureKey, signatureData);
			authorizationParam["oauth_signature"] = signature;
			
			var headerParam = BuildQueryParam(authorizationParam, "\"", ", ");
			AuthorizationHeader(headerParam);

			var builder = new UriBuilder(client.BaseAddress + @"/users/show");
			builder.Query = query.ToString();
			System.Console.WriteLine(client.DefaultRequestHeaders.ToString());
			return client.GetAsync(builder.ToString());
		}

		/// <summary>
		/// 署名に必要なキーを作成します
		/// </summary>
		/// <returns>署名のキー</returns>
		private string AuthorizationKey()
		{
			return UriEncode(consumerSecret) + "&" + UriEncode(accessTokenSecret);
		}

		private NameValueCollection GetAutorizationParameter()
		{
			var authorizationData = new NameValueCollection();
			authorizationData["oauth_consumer_key"] = consumerKey;
			authorizationData["oauth_token"] = accessToken;
			authorizationData["oauth_nonce"] = ((long)CurrentTimeMilliseconds().TotalMilliseconds).ToString();
			authorizationData["oauth_signature_method"] = "HMAC-SHA1";
			authorizationData["oauth_timestamp"] = ((long)CurrentTimeMilliseconds().TotalSeconds).ToString();
			authorizationData["oauth_version"] = "1.0";
			return authorizationData;
		}

		public NameValueCollection MergeParams(NameValueCollection nvc1, NameValueCollection nvc2)
		{
			NameValueCollection nvc = new NameValueCollection();
			nvc.Add(nvc1);
			nvc.Add(nvc2);
			return nvc;
		}
		/// <summary>
		/// 署名に必要なデータを作成します
		/// </summary>
		/// <param name="queryParam">リクエストURIにつけるクエリパラメータ</param>
		/// <param name="httpMethod">リクエストURIを呼ぶHttpメソッド</param>
		/// <param name="requestUri">リクエストURI</param>
		/// <returns>署名のデータ</returns>
		private string AuthorizationData(NameValueCollection queryParam, HttpMethod httpMethod, String requestUri)
		{
			var query = BuildQueryParam(queryParam, "", "&");

			// URIエンコードしつつhttpメソッド、リクエストURI、クエリパラメータを&でつなぐ
			var requestParam = UriEncode(query);
			var method = UriEncode(httpMethod.ToString());
			requestUri = UriEncode(requestUri);
			System.Console.WriteLine(method + "&" + requestUri + "&" + requestParam);
			return method + "&" + requestUri + "&" + requestParam;
		}

		private string BuildQueryParam(NameValueCollection nvc, string enclose, string separator)
		{
			var builder = new StringBuilder();
			foreach (var key in nvc.AllKeys.OrderBy(k => k))
			{
				builder.AppendFormat("{0}={1}{2}{1}{3}", key, enclose, nvc[key], separator);
			}
			builder.Remove(builder.Length - separator.Length, separator.Length);
			return builder.ToString();
		}

		private TimeSpan CurrentTimeMilliseconds()
		{
			var unixEpock = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			var currentTime = DateTime.Now.ToUniversalTime();
			return currentTime - unixEpock;
		}

		/// <summary>
		/// HMAC-SHA1でエンコードし、Base64エンコードをした文字列を返す
		/// </summary>
		/// <param name="key">HMAC-SHA1に使うキー</param>
		/// <param name="data">エンコードしたいデータ</param>
		/// <returns></returns>
		private string HMacSha1(string key, string data)
		{
			var keyBytes = System.Text.Encoding.ASCII.GetBytes(key);
			var dataBytes = System.Text.Encoding.ASCII.GetBytes(data);
			using (var hmac = new System.Security.Cryptography.HMACSHA1(keyBytes))
			{
				return Convert.ToBase64String(hmac.ComputeHash(dataBytes));
			}
		}

		/// <summary>
		/// 引数に与えられた文字列をUTF-8でURIエンコードします
		/// </summary>
		/// <param name="str">エンコードしたい文字列</param>
		/// <returns>エンコードされた文字列</returns>
		private string UriEncode(string str)
		{
			string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
			StringBuilder result = new StringBuilder();
			byte[] data = Encoding.UTF8.GetBytes(str);

			int len = data.Length;

			for (int i = 0; i < len; i++)
			{
				int c = data[i];
				if (c < 0x80 && unreservedChars.IndexOf((char)c) != -1)
				{
					result.Append((char)c);
				}
				else
				{
					result.Append('%' + String.Format("{0:X2}", (int)data[i]));
				}

			}
			return result.ToString();
		}

		private void AuthorizationHeader(string headerParam)
		{
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", headerParam);
		}
    }
}
