using System.Net;

namespace Twitter.Response
{
	public class StatusCode
	{
		public HttpStatusCode Code
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}
	}
}
