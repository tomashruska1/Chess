using System;
using System.Collections.Generic;

namespace Chess.Application.ChessAIs.Extensions
{
    /// <summary>
    /// Extension methods for Linq.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Returns the largest <typeparamref name="T"/> item from a collection of <typeparamref name="T"/> based on value taken by the <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="selector"></param>
        /// <returns><typeparamref name="T"/> or null if the collection is empty</returns>
        public static T MaxBy<T>(this IEnumerable<T> collection, Func<T, double> selector) where T : class
        {
            T max = null;
            double value = 0;

            foreach (T item in collection)
            {
                if (item is null)
                    continue;

                double newValue = selector(item);
                if (max is null || newValue > value)
                {
                    value = newValue;
                    max = item;
                }
            }

            return max;
        }
    }
}
