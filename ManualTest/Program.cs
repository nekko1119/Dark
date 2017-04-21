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
            try
            {
                TwitterClient client = new TwitterClient
                (
                    "IQCYDtTgyYbWxZId1kGI85uWz",
                    "KrnbkvZ8zglSQkHf9TgOeRwebCzAnrkqvuPeQCVrb8UjivGs00"
                );
                var requestTokenResponse = client.GetRequestToken().Result;

                var url = client.BaseUri + "/oauth/authenticate?oauth_token=" + requestTokenResponse.OAuthToken;
                Process.Start(url);
                Console.Write("input pin >");
                var pin = Console.ReadLine();
                var accessTokenResponse = client.GetAccessToken(pin).Result;
                var profileResponse = client.GetProfile("nekko1119").Result;
                Console.WriteLine(profileResponse);
                Console.Write(client.Tweet("テスト").Result);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
