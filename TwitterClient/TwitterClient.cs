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
    public class TwitterClient
    {
		private HttpClient client;

		public TwitterClient()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri("http://api.twitter.com/1.1");
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["hoge"] = "あ";
			query["foo"] = "google";
			System.Console.WriteLine(query.ToString());
			var builder = new UriBuilder(client.BaseAddress);
			builder.Query = query.ToString();
			System.Console.WriteLine(builder.ToString());
			client.GetAsync(builder.ToString());
		}
    }
}
