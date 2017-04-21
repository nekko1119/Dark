using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter
{
    public class QueryParameter : IComparable<QueryParameter>
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public int CompareTo(QueryParameter other)
        {
            if (Name == other.Name)
            {
                return Value.CompareTo(other.Value);
            }
            else
            {
                return Name.CompareTo(other.Name);
            }
        }

        public override string ToString()
        {
            return Name + "=" + Value;
        }

        public static string Generate(List<QueryParameter> queryParameters)
        {
            if (queryParameters == null)
            {
                throw new ArgumentNullException("queryParameters");
            }

            return queryParameters.Join("&");
        }
    }
}
