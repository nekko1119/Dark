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
		private static Twitter.TwitterClient client;

		[ClassInitialize]
		public static void InitilaizeClass(TestContext testContext)
		{
			client = new Twitter.TwitterClient
				(
				"r1xe4MXNfCg4aJRvmaNig5IOw",
				"DaczKjZivPFHOZ1DnwMPCD05EomwGLMMtxqs4McU8Sm7CtPb2b"
				);
		}

		[TestInitialize]
		public void Initialzie()
		{
			var docment = XElement.Load("../../../../access_token.xml");
			client.AccessToken = docment.Element("key").Value;
			client.AccessTokenSecret = docment.Element("secret").Value;
		}

		[ClassCleanup]
		public static void CleanupClass()
		{
			client.Dispose();
		}

		[TestMethod]
		public void URIエンコード()
		{
			Assert.AreEqual("%E3%81%82", OAuth.UriEncode("あ"));
		}

		[TestMethod]
		public void プロフィール取得()
		{
			var response = client.GetProfile("root1119").Result;
			System.Console.WriteLine(response);
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode.Code);
		}

		[TestMethod]
		public void リクエストトークン取得()
		{
			client.AccessToken = "";
			client.AccessTokenSecret = "";
			var response = client.GetRequestToken().Result;
			System.Console.WriteLine("oauth_token: {0}, oauth_token_secret: {1}, oauth_callback_confirmed: {2}", response.OAuthToken, response.OAuthTokenSecret, response.OAuthCallbackConfirmed);
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode.Code);
		}
	}
}
