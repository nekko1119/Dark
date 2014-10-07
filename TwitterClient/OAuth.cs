using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Twitter
{
	class OAuth
	{
		private readonly string consumerKey;

		public string ConsumerKey
		{
			get
			{
				return consumerKey;
			}
		}

		private readonly string consumerSecret;

		public string ConsumerSecret
		{
			get
			{
				return consumerSecret;
			}
		}

		public string Token
		{
			get;
			set;
		}

		public string TokenSecret
		{
			get;
			set;
		}

		public OAuth(string consumerKey, string consumerSecret, string token, string tokenSecret)
		{
			this.consumerKey = consumerKey;
			this.consumerSecret = consumerSecret;
			Token = token;
			TokenSecret = tokenSecret;
		}

		public OAuth(string consumerKey, string consumerSecret)
			: this(consumerKey, consumerSecret, "", "")
		{
		}

		public string MakeOAuthKey()
		{
			return UriEncode(consumerSecret) + "&" + UriEncode(TokenSecret);
		}

		public string MakeOAuthData(HttpRequestMessage request, List<QueryParameter> queryParameters)
		{
			// 引数チェック
			if (request == null)
			{
				throw new ArgumentNullException();
			}

			if (request.Method == null)
			{
				throw new ArgumentNullException();
			}

			if (request.RequestUri == null)
			{
				throw new ArgumentNullException();
			}

			if (queryParameters == null)
			{
				queryParameters = new List<QueryParameter>();
			}

			// クエリパラメータをソートして文字列に変換する

			queryParameters.Sort();
			var queryParameterString = QueryParameter.GenerateQueryParameterString(queryParameters);

			// URIを構築する

			var uriString = String.Format("{0}://{1}", request.RequestUri.Scheme, request.RequestUri.Host);
			if (!request.RequestUri.IsDefaultPort)
			{
				uriString += ":" + request.RequestUri.Port;
			}
			uriString += request.RequestUri.LocalPath;

			// httpメソッド、URI、クエリをそれぞれURIエンコードし、&で繋ぐ

			StringBuilder builder = new StringBuilder();
			builder.Append(UriEncode(request.Method.ToString()));
			builder.Append("&" + UriEncode(uriString));
			builder.Append("&" + UriEncode(queryParameterString));
			return builder.ToString();
		}

		public string MakeHashCode(string key, string data)
		{
			var keyBytes = System.Text.Encoding.ASCII.GetBytes(key);
			var dataBytes = System.Text.Encoding.ASCII.GetBytes(data);
			using (var hash = new System.Security.Cryptography.HMACSHA1(keyBytes))
			{
				return Convert.ToBase64String(hash.ComputeHash(dataBytes));
			}
		}

		/// <summary>
		/// 引数に与えられた文字列をUTF-8でURIエンコードします
		/// </summary>
		/// <param name="str">エンコードしたい文字列</param>
		/// <returns>エンコードされた文字列</returns>
		/// <exception cref="ArgumentNullException">strがnullの場合</exception>
		public static string UriEncode(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException();
			}

			const string unReservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			StringBuilder encoded = new StringBuilder();
			int length = bytes.Length;
			for (int i = 0; i < length; ++i)
			{
				int c = bytes[i];
				if (c < 0x80 && unReservedChars.IndexOf((char)c) != -1)
				{
					encoded.Append((char)c);
				}
				else
				{
					encoded.Append("%" + String.Format("{0:X2}", (int)bytes[i]));
				}

			}
			return encoded.ToString();
		}
	}
}
