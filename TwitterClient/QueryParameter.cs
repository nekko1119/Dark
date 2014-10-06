using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterClient
{
	class QueryParameter : IComparable<QueryParameter>
	{
		private readonly string name;

		public string Name
		{
			get
			{
				return name;
			}
		}

		private readonly string value;

		public string Value
		{
			get
			{
				return value;
			}
		}

		public QueryParameter(string name, string value)
		{
			this.name = name;
			this.value = value;
		}

		public int CompareTo(QueryParameter other)
		{
			if (Name == other.Name)
			{
				return Value.CompareTo(other);
			}
			else
			{
				return Name.CompareTo(other);
			}
		}

		public override string ToString()
		{
			return Name + "=" + Value;
		}
	}
}
