using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterClient
{
	enum Method
	{
		GET,
		POST,
		PUT,
		DELETE
	}

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
			return UriEncode(ConsumerKey) + "&" + UriEncode(TokenSecret);
		}

		public string MakeOAuthData(Method method, Uri uri, List<QueryParameter> queryParameters)
		{
			// 引数チェック

			if (method == null)
			{
				throw new ArgumentNullException();
			}

			if (uri == null)
			{
				throw new ArgumentNullException();
			}

			if (queryParameters == null)
			{
				throw new ArgumentNullException();
			}

			// クエリパラメータを&でつないだ文字列に変換する

			queryParameters.Sort();
			var queryParameterString = GenerateQueryParameterString(queryParameters);

			// URIを構築する

			var uriString = String.Format("{0}://{1}", uri.Scheme, uri.Host);
			if (!uri.IsDefaultPort)
			{
				uriString += ":" + uri.Port;
			}
			uriString += uri.LocalPath;

			// httpメソッド、URI、クエリをそれぞれURIエンコードし、&で繋ぐ

			StringBuilder builder = new StringBuilder();
			builder.Append(UriEncode(method.ToString()));
			builder.Append("&" + UriEncode(uriString));
			builder.Append("&" + UriEncode(queryParameterString));
			return builder.ToString();
		}

		/// <summary>
		/// 引数に与えられた文字列をUTF-8でURIエンコードします
		/// </summary>
		/// <param name="str">エンコードしたい文字列</param>
		/// <returns>エンコードされた文字列</returns>
		/// <exception cref="ArgumentNullException">strがnullの場合</exception>
		private string UriEncode(string str)
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

		private string GenerateQueryParameterString(List<QueryParameter> queryParameters)
		{
			if (queryParameters == null)
			{
				throw new ArgumentNullException("queryParameters null");
			}

			var builder = new StringBuilder();
			var length = queryParameters.Count;
			for (int i = 0; i < length; i++)
			{
				builder.Append(queryParameters[i]);
				if (i < length - 1)
				{
					builder.Append("&");
				}
			}

			return builder.ToString();
		}
	}
}
