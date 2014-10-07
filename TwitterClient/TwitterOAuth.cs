﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Twitter
{
	class TwitterOAuth
	{
		private OAuth oauth;

		public string ConsumerKey
		{
			get
			{
				return oauth.ConsumerKey;
			}
		}

		public string ConsumerSecret
		{
			get
			{
				return oauth.ConsumerSecret;
			}
		}

		public string AccessToken
		{
			get
			{
				return oauth.Token;
			}
			set
			{
				oauth.Token = value;
			}
		}

		public string AccessTokenSecret
		{
			get
			{
				return oauth.TokenSecret;
			}
			set
			{
				oauth.TokenSecret = value;
			}
		}

		public TwitterOAuth(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
		{
			oauth = new OAuth(consumerKey, consumerSecret)
			{
				Token = accessToken,
				TokenSecret = accessTokenSecret
			};
		}

		public TwitterOAuth(string consumerKey, string consumerSecret)
			: this(consumerKey, consumerSecret, "", "")
		{
		}

		public string MakeAuthorizationHeader(HttpMethod method, Uri uri, List<QueryParameter> queryParameters)
		{
			var oauthParameters = new List<QueryParameter>();
			oauthParameters.Add(new QueryParameter("oauth_consumer_key", oauth.ConsumerKey));
			oauthParameters.Add(new QueryParameter("oauth_token", AccessToken));
			oauthParameters.Add(new QueryParameter("oauth_signature_method", "HMAC-SHA1"));
			oauthParameters.Add(new QueryParameter("oauth_timestamp", ((long)CurrentTime().TotalSeconds).ToString()));
			oauthParameters.Add(new QueryParameter("oauth_version", "1.0"));
			oauthParameters.Add(new QueryParameter("oauth_nonce", MakeNonce(34)));
			queryParameters.AddRange(oauthParameters);

			var request = new HttpRequestMessage(method, uri);
			var oauthData = oauth.MakeOAuthData(request, queryParameters);
			var oauthKey = oauth.MakeOAuthKey();
			var signature = oauth.MakeHashCode(oauthKey, oauthData);
			oauthParameters.Add(new QueryParameter("oauth_signature", OAuth.UriEncode(signature)));
			oauthParameters.Sort();

			var headerParameter = new StringBuilder();
			var length = oauthParameters.Count;
			for (int i = 0; i < length; i++)
			{
				var p = oauthParameters[i];
				headerParameter.Append(String.Format("{0}=\"{1}\"", p.Name, p.Value));
				if (i < length - 1)
				{
					headerParameter.Append(", ");
				}
			}
			return headerParameter.ToString();
		}

		private TimeSpan CurrentTime()
		{
			var unixEpock = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return DateTime.Now.ToUniversalTime() -unixEpock;
		}

		private string MakeNonce(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length < 0");
			}

			string table = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
			var nonce = new StringBuilder(length);
			var random = new Random();
			for (int i = 0; i < length; i++)
			{
				int index = random.Next(table.Length);
				char c = table[index];
				nonce.Append(c);
			}
			return nonce.ToString();
		}
	}
}
