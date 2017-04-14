using System;
using System.Collections.Generic;

namespace PlanPresentation
{
    public static class Extensions
    {
        public static IEnumerable<TResult> Pairwise<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> resultSelector)
        {
            TSource previous = default(TSource);

            using (var it = source.GetEnumerator())
            {
                if (it.MoveNext())
                    previous = it.Current;

                while (it.MoveNext())
                    yield return resultSelector(previous, previous = it.Current);
            }
        }

        public static void Pairwise<TSource>(this IEnumerable<TSource> source, Action<TSource, TSource> resultSelector)
        {
            var previous = default(TSource);

            using (var it = source.GetEnumerator())
            {
                if (it.MoveNext())
                    previous = it.Current;

                while (it.MoveNext())
                    resultSelector(previous, previous = it.Current);
            }
        }
    }
}
