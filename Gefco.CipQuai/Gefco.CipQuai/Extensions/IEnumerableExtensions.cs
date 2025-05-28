using System;
using System.Collections.Generic;
using System.Linq;

namespace Gefco.CipQuai.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.Last());
        }
        public static int RemoveWhere<T>(this ICollection<T> collection, Predicate<T> match)
        {
            if (collection.IsReadOnly) { throw new NotSupportedException("The collection is read-only."); }

            // Defer to existing implementation...
            var hashSetOfT = collection as HashSet<T>;
            if (hashSetOfT != null)
            {
                return hashSetOfT.RemoveWhere(match);
            }

            // Defer to existing implementation...
            var sortedSetOfT = collection as SortedSet<T>;
            if (sortedSetOfT != null)
            {
                return sortedSetOfT.RemoveWhere(match);
            }

            // Defer to existing implementation...
            var listOfT = collection as List<T>;
            if (listOfT != null)
            {
                return listOfT.RemoveAll(match);
            }

            // Have to use our own implementation.

            int removed = 0;

            // IList<T> is pretty efficient because we only have to enumerate
            // the list once and if a match, we remove at that position.
            // Enumerate backwards so that the indexes don't shift out from under us.
            var list = collection as IList<T>;
            if (list != null)
            {

                for (int i = list.Count - 1; i >= 0; i--)
                {
                    T item = list[i];
                    if (match(item))
                    {
                        list.RemoveAt(i);
                        removed++;
                    }
                }

                return removed;

            }

            // For ICollection<T> it isn't as efficient because we have to first
            // buffer all the items to remove in a temporary collection.
            // Then we enumerate that temp collection removing each individually
            // from the ICollection<T> which could be potentially O(n).

            var itemsToRemove = new List<T>(from x in collection where match(x) select x);
            foreach (T item in itemsToRemove)
            {
                if (collection.Remove(item))
                {
                    removed++;
                }
            }

            return removed;

        }
    }
}