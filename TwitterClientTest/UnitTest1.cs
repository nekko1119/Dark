using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TwitterClientTest
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			Twitter.TwitterClient client = new Twitter.TwitterClient();
		}
	}
}
