using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Twitter;
using System.Net;

namespace TwitterClientTest
{
	[TestClass]
	public class TwitterClientTest
	{
		Twitter.TwitterClient client;

		[TestInitialize]
		public void Initilaize()
		{
			client = new Twitter.TwitterClient();
		}

		[TestMethod]
		public void URIエンコード()
		{
		}

		[TestMethod]
		public void プロフィール取得()
		{
			var response = client.GetProfile("nekko1119");
			System.Console.WriteLine(response.Result.Content.ReadAsStringAsync().Result);
			Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
		}
	}
}
