using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Response
{
	[DataContract]
	public class RequestTokenResponse
	{
		public StatusCode StatusCode
		{
			get;
			set;
		}

		[DataMember(Name="oauth_token")]
		public string OAuthToken
		{
			get;
			private set;
		}

		[DataMember(Name = "oauth_token_secret")]
		public string OAuthTokenSecret
		{
			get;
			private set;
		}

		[DataMember(Name = "oauth_callback_confirmed")]
		public bool OAuthCallbackConfirmed
		{
			get;
			private set;
		}

		public RequestTokenResponse()
		{
		}

		public RequestTokenResponse(string oauthToken, string oauthTokenSecret, bool oauthCallbackConfirmed)
		{
			OAuthToken = oauthToken;
			OAuthTokenSecret = oauthTokenSecret;
			OAuthCallbackConfirmed = oauthCallbackConfirmed;
		}
	}
}
