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
                "IQCYDtTgyYbWxZId1kGI85uWz",
                "KrnbkvZ8zglSQkHf9TgOeRwebCzAnrkqvuPeQCVrb8UjivGs00"
            );
            var requestTokenResponse = client.GetRequestToken().Result;
            Debug.Assert(HttpStatusCode.OK == requestTokenResponse.StatusCode.Code);

            var url = client.BaseUri + "/oauth/authorize?oauth_token=" + requestTokenResponse.OAuthToken;
            Process.Start(url);
            Console.Write("input pin >");
            var pin = Console.ReadLine();

            client.AccessToken = requestTokenResponse.OAuthToken;
            client.AccessTokenSecret = requestTokenResponse.OAuthTokenSecret;
            var accessTokenResponse = client.GetAccessToken(pin).Result;
            Debug.Assert(HttpStatusCode.OK == accessTokenResponse.StatusCode.Code);
            Console.WriteLine(accessTokenResponse);

            var profileResponse = client.GetProfile("nekko1119").Result;
            Debug.Assert(HttpStatusCode.OK == profileResponse.StatusCode.Code);
            Console.WriteLine(profileResponse);
        }
    }
}
