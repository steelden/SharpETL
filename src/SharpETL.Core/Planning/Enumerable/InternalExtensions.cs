using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Planning.Enumerable
{
    internal static class InternalExtensions
    {
        public static IObservable<T> AsObservable<T>(this IEnumerable<T> enumerable)
        {
            return new EnumerableToObservable<T>(enumerable);
        }

        public static IEnumerable<T> AsEnumerable<T>(this IObservable<T> observable)
        {
            return new ObservableToEnumerable<T>(observable);
        }

        public static IEnumerable<T> ConcatAll<T>(this IEnumerable<T> baseEnumerable, IEnumerable<IEnumerable<T>> enumerableOfEnumerables)
        {
            IEnumerable<T> result = baseEnumerable;
            foreach (var enumerable in enumerableOfEnumerables) {
                result = result.Concat(enumerable);
            }
            return result;
        }

        public static IEnumerable<T> Merge<T>(this IEnumerable<IEnumerable<T>> enumerableOfEnumerables)
        {
            IEnumerable<T> result = null;
            foreach (var enumerable in enumerableOfEnumerables) {
                if (result == null) {
                    result = enumerable;
                } else {
                    result = result.Concat(enumerable);
                }
            }
            return result;
        }
    }
}
