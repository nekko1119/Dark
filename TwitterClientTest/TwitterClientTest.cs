using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Xml.Linq;
using Twitter;

namespace TwitterTest
{
    [TestClass]
    public class TwitterClientTest
    {
        private static TwitterClient client;

        [ClassInitialize]
        public static void InitilaizeClass(TestContext testContext)
        {
            client = new TwitterClient
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

        [Ignore]
        [TestMethod]
        public void プロフィール取得()
        {
            var response = client.GetProfile("root1119").Result;
            System.Console.WriteLine(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode.Code);
        }

        [TestClass]
        public class リクエストトークン取得
        {
            [Ignore]
            [TestMethod]
            public void 正常系()
            {
                string accessToken = client.AccessToken;
                string accessTokenSecret = client.AccessTokenSecret;
                try
                {
                    client.AccessToken = "";
                    client.AccessTokenSecret = "";
                    var response = client.GetRequestToken().Result;
                    System.Console.WriteLine(
                        "oauth_token: {0}, oauth_token_secret: {1}, oauth_callback_confirmed: {2}",
                        response.OAuthToken, response.OAuthTokenSecret, response.OAuthCallbackConfirmed);
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode.Code);
                }
                finally
                {
                    client.AccessToken = accessToken;
                    client.AccessTokenSecret = accessTokenSecret;
                }
            }

            [Ignore]
            [TestMethod]
            public void アクセストークン設定済み()
            {
                try
                {
                    var response = client.GetRequestToken().Result;
                    Assert.Fail("例外が発生しませんでした");
                }
                catch (AggregateException aggregateException)
                {
                    InvalidAccessTokenStateException e =
                        aggregateException.InnerException as InvalidAccessTokenStateException;
                    Assert.IsNotNull(e);
                    System.Console.WriteLine(e);
                    Assert.IsTrue
                    (
                        !string.IsNullOrEmpty(e.AccessToken) ||
                        !string.IsNullOrEmpty(e.AccessTokenSecret)
                    );
                }
            }
        }
    }
}
