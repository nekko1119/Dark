using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter
{
    public static class EnumerableExtensions
    {
        public static string Join<T>(this IEnumerable<T> source)
        {
            return Join(source, "");
        }

        public static string Join<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }
    }
}
