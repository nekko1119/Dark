using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterClient
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

		public string GetOAuthKey()
		{
			return UriEncode(ConsumerKey) + "&" + UriEncode(TokenSecret);
		}

		public OAuth(string consumerKey, string consumerSecret, string token, string tokenSecret)
		{
			this.consumerKey = consumerKey;
			this.consumerSecret = consumerSecret;
			Token = token;
			TokenSecret = tokenSecret;
		}

		public OAuth(string consumerKey, string consumerSecret)
			: this(consumerKey, consumerSecret, null, null)
		{
		}

		private long GetCurrentUnixTime()
		{
			var unixEpock = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			var currentTime = DateTime.Now.ToUniversalTime();
			return (long)((currentTime - unixEpock).TotalMilliseconds);
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
	}
}
