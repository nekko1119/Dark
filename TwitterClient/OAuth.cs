using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Twitter
{
    public class OAuth
    {
        public string ConsumerKey { get; private set; }
        public string ConsumerSecret { get; private set; }
        public string Token { get; set; }
        public string TokenSecret { get; set; }

        public OAuth(string consumerKey, string consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public string GenerateOAuthKey()
        {
            var temp = UriEncode(ConsumerSecret) + "&";
            if (!String.IsNullOrEmpty(TokenSecret))
            {
                temp += UriEncode(TokenSecret);
            }
            return temp;
        }

        public string GenerateOAuthData(HttpRequestMessage request, List<QueryParameter> queryParameters)
        {
            // 引数検証
            if (request == null || request.Method == null || request.RequestUri == null || queryParameters == null)
            {
                throw new ArgumentNullException();
            }

            // クエリパラメータをソートして文字列に変換する
            queryParameters.Sort();
            var queryParameterString = QueryParameter.GenerateQueryParameterString(queryParameters);

            // URIを構築する
            var uriString = String.Format("{0}://{1}", request.RequestUri.Scheme, request.RequestUri.Host);
            if (!request.RequestUri.IsDefaultPort)
            {
                uriString += ":" + request.RequestUri.Port;
            }
            uriString += request.RequestUri.LocalPath;

            // httpメソッド、URI、クエリをそれぞれURIエンコードし、&で繋ぐ
            var oauthData = string.Join(
                "&",
                UriEncode(request.Method.ToString()),
                UriEncode(uriString),
                UriEncode(queryParameterString));
            System.Console.WriteLine(oauthData);
            return oauthData;
        }

        public string MakeHashCode(string key, string data)
        {
            var keyBytes = System.Text.Encoding.ASCII.GetBytes(key);
            var dataBytes = System.Text.Encoding.ASCII.GetBytes(data);
            using (var hash = new System.Security.Cryptography.HMACSHA1(keyBytes))
            {
                return Convert.ToBase64String(hash.ComputeHash(dataBytes));
            }
        }

        /// <summary>
        /// 引数に与えられた文字列をUTF-8でURIエンコードします
        /// </summary>
        /// <param name="str">エンコードしたい文字列</param>
        /// <returns>エンコードされた文字列</returns>
        /// <exception cref="ArgumentNullException">strがnullの場合</exception>
        public static string UriEncode(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException();
            }

            const string unReservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            return Encoding.UTF8.GetBytes(str).Select(b =>
            {
                if ((int)b < 0x80 && unReservedChars.Any(c => c == (char)b))
                {
                    return ((char)b).ToString();
                }
                else
                {
                    return "%" + string.Format("{0:X2}", (int)b);
                }
            }).JoinString();
            /*http://ideone.com/e72V66 */
        }
    }
}
