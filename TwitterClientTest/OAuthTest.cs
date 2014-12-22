using Microsoft.VisualStudio.TestTools.UnitTesting;
using Twitter;

namespace TwitterTest
{
	[TestClass]
	public class OAuthTest
	{
		[TestMethod]
		public void URIエンコード()
		{
			Assert.AreEqual("%E3%81%82", OAuth.UriEncode("あ"));
		}
	}
}
