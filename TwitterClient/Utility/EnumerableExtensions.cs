using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter
{
    public static class EnumerableExtensions
    {
        public static string JoinString<T>(this IEnumerable<T> source)
        {
            return JoinString(source, "");
        }

        public static string JoinString<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }
    }
}
