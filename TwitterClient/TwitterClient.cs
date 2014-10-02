using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Twitter
{
	/// <summary>
	/// Twitter APIにアクセスするためのクライアントクラスです
	/// </summary>
    public class TwitterClient
    {
		private HttpClient client;
		private readonly string baseAddress = "http://api.twitter.com/1.1";
		// TODO app.configに書き出す
		private readonly string consumerKey;
		private readonly string consumerSecret;

		public TwitterClient()
		{
			consumerKey = System.Configuration.ConfigurationManager.AppSettings["consumerKey"];
			consumerSecret = System.Configuration.ConfigurationManager.AppSettings["consumerSecret"];
			client = new HttpClient();
			client.BaseAddress = new Uri(baseAddress);
			//var query = HttpUtility.ParseQueryString(string.Empty);
			//query["hoge"] = "あ";
			//query["foo"] = "google";
			//System.Console.WriteLine(query.ToString());
			//var builder = new UriBuilder(client.BaseAddress);
			//builder.Query = query.ToString();
			//System.Console.WriteLine(builder.ToString());
			//client.GetAsync(builder.ToString());
		}

		/// <summary>
		/// 引数に与えられた文字列をUTF-8でURIエンコードします
		/// </summary>
		/// <param name="str">エンコードしたい文字列</param>
		/// <returns>エンコードされた文字列</returns>
		private string UriEncode(string str)
		{
			return HttpUtility.UrlEncode(str, Encoding.UTF8).ToUpper();
		}
    }
}
