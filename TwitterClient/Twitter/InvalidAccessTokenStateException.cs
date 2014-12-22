using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter
{
	/// <summary>
	/// AccessTokenが不正の状態を表す例外です
	/// </summary>
	public class InvalidAccessTokenStateException
		: InvalidOperationException
	{
		public InvalidAccessTokenStateException()
			: base()
		{
		}

		public InvalidAccessTokenStateException(string accessToken, string accessTokenSecret)
			: base()
		{
			AccessToken = accessToken;
			AccessTokenSecret = accessTokenSecret;
		}

		public InvalidAccessTokenStateException(string message)
			: base(message)
		{
		}

		public InvalidAccessTokenStateException(string message, string accessToken, string accessTokenSecret)
			: base(message)
		{
			AccessToken = accessToken;
			AccessTokenSecret = accessTokenSecret;
		}

		public InvalidAccessTokenStateException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public InvalidAccessTokenStateException(
			string message, Exception innerException, string accessToken, string accessTokenSecret)
			: base(message, innerException)
		{
			AccessToken = accessToken;
			AccessTokenSecret = accessTokenSecret;
		}

		public string AccessToken
		{
			get;
			private set;
		}

		public string AccessTokenSecret
		{
			get;
			private set;
		}

		public override string ToString()
		{
			string accessToken = "AccessToken : " + AccessToken;
			string accessTokenSecret = "AccessTokenSecret : " + AccessTokenSecret;
			return string.Join(" ", base.ToString(), accessToken, AccessTokenSecret);
		}
	}
}
