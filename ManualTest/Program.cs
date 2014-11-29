using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Twitter;

namespace TwitterClientTest
{
	public class ManualTest
	{
		public static void Main(string[] args)
		{
			手動テスト_アクセストークン取得();
		}

		public static void 手動テスト_アクセストークン取得()
		{
			TwitterClient client = new TwitterClient
			(
				"r1xe4MXNfCg4aJRvmaNig5IOw",
				"DaczKjZivPFHOZ1DnwMPCD05EomwGLMMtxqs4McU8Sm7CtPb2b"
			);
			var response = client.GetRequestToken().Result;
			Debug.Assert(HttpStatusCode.OK == response.StatusCode.Code);

			var url = client.BaseUri + "/oauth/authorize?oauth_token=" + response.OAuthToken;
			System.Diagnostics.Process.Start(url);
			var pin = System.Console.ReadLine();

			System.Console.WriteLine("request_token: " + response.OAuthToken + " request_token_secret: " + response.OAuthTokenSecret);
			client.AccessToken = response.OAuthToken;
			client.AccessTokenSecret = response.OAuthTokenSecret;
			var response2 = client.GetAccessToken(pin).Result;
			Debug.Assert(HttpStatusCode.OK == response2.StatusCode.Code);

			System.Console.WriteLine(response2);
		}
	}
}
