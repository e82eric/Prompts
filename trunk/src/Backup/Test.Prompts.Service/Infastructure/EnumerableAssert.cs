using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Test.Prompts.Service.Infastructure
{
    public static class EnumerableAssert
    {
        public static void AssetContians<T>(this IEnumerable<T> source, IEnumerable<T> expected)
        {
            foreach (var t in expected)
            {
                var t1 = t;
                AssertSingle(source, s => s.Equals(t1));
            }
        }

        public static void AssetItemsAndLength<T>(this IEnumerable<T> source, params T[] expected)
        {
            Assert.AreEqual(expected.Length, source.Count());
            foreach (var t in expected)
            {
                AssertSingle(source, s => s.Equals(t));
            }
        }

        public static void AssertEqual<T>(this IEnumerable<T> source, IEnumerable<T> expected, Func<T, T, bool> predicate)
        {
            Assert.AreEqual(source.Count(), expected.Count());
            foreach (var t in expected)
            {
                var closureT = t;
                source.AssertSingle(s => predicate(closureT, s));
            }
        }

        public static void AssertEqual<T>(this IEnumerable<T> source, params T[] expected)
        {
            Assert.AreEqual(source.Count(), expected.Count());
            foreach (var t in expected)
            {
                source.AssertSingle(s => s.Equals(t));
            }
        }

        public static T AssertSingle<T>(this IEnumerable<T> source)
        {
            return AssertSingle(source, s => true);
        }

        public static T AssertSingle<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return AssertLength(source, predicate, 1).Single();
        }

        public static IEnumerable<T> AssertLength<T>(this IEnumerable<T> source, int expectedLength)
        {
            return AssertLength(source, i => true, expectedLength);
        }

        public static IEnumerable<T> AssertLength<T>(this IEnumerable<T> source, Func<T, bool> predicate, int expectedLength)
        {
            var count = 0;
            var enitiesToReturn = new List<T>();
            foreach (var t in source)
            {
                if (predicate(t))
                {
                    count++;
                    enitiesToReturn.Add(t);
                }
            }
            Assert.AreEqual(expectedLength, count);

            return enitiesToReturn;
        }
    }
}
