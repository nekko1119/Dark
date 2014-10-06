using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Twitter;

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
			Assert.AreEqual("%E3%81%82", client.AsDynamic().UriEncode("あ"));
			var nvc = new System.Collections.Specialized.NameValueCollection();
			nvc["screen_name"] = "hoge";
		}

		[TestMethod]
		public void プロフィール取得()
		{
			var response = client.GetProfile("nekko1119");
			System.Console.WriteLine(response.Result);
			System.Console.WriteLine(response.Result.Content.ReadAsStringAsync());
		}
	}
}
