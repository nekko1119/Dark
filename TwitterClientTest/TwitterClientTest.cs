using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Twitter;
using System.Net;
using System.Xml.Linq;

namespace TwitterClientTest
{
	[TestClass]
	public class TwitterClientTest
	{
		Twitter.TwitterClient client;

		[TestInitialize]
		public void Initilaize()
		{
			var docment = XElement.Load("../../../../access_token.xml");
			client = new Twitter.TwitterClient
				(
				"r1xe4MXNfCg4aJRvmaNig5IOw",
				"DaczKjZivPFHOZ1DnwMPCD05EomwGLMMtxqs4McU8Sm7CtPb2b",
				docment.Element("key").Value,
				docment.Element("secret").Value
				);
		}

		[TestMethod]
		public void URIエンコード()
		{
		}

		[TestMethod]
		public void プロフィール取得()
		{
			var response = client.GetProfile("nekko1119");
			System.Console.WriteLine(response.Content.ReadAsStringAsync().Result);
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[TestMethod]
		public void リクエストトークン取得()
		{
			client.AccessToken = "";
			client.AccessTokenSecret = "";
			var response = client.PostRequestToken();
			System.Console.WriteLine("oauth_token {0}, oauth_token_secret {1}, oauth_callback_confirmed {2}", response.OAuthToken, response.OAuthTokenSecret, response.OAuthCallbackConfirmed);
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode.Code);
		}
	}
}
