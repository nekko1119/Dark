using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Response
{
	[DataContract]
	public class Tag
	{
		[DataMember(Name = "text")]
		public string Text
		{
			get;
			private set;
		}

		[DataMember(Name = "indices")]
		public List<long> Indices
		{
			get;
			private set;
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append(
				string.Format(
				"text: {0}\n",
				Text));
			builder.Append("indices:\n");
			if (Indices != null)
			{
				foreach (var i in Indices)
				{
					builder.Append(i + "\n");
				}
			}
			return builder.ToString();
		}
	}
}
