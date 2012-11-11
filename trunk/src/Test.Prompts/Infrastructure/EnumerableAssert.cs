using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Prompts.Infrastructure
{
    public static class EnumerableAssert
    {
        public static void AssertEquals<T>(this IEnumerable<T> source, IEnumerable<T> expected)
        {
            foreach (var t in expected)
            {
                var localT = t;
                source.AssertSingle(s => s.Equals(localT));
            }
        }

        public static T AssertSingle<T>(this IEnumerable<T> source)
        {
            return AssertSingle(source, t => true);
        }

        public static T AssertSingle<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var numberOfMatches = 0;
            var ret = default(T);

            foreach (var t in source.Where(predicate))
            {
                ret = t;
                numberOfMatches++;
            }

            Assert.AreEqual(1, numberOfMatches);
            return ret;
        }

        public static void AssertLength<T>(this IEnumerable<T> source, int expectedLength)
        {
            Assert.AreEqual(expectedLength, source.Count());
        }

        public static void AssertContains<T>(this IEnumerable<T> source, params T[] array)
        {
            foreach (var t in array)
            {
                var localT = t;
                source.AssertSingle(s => s.Equals(localT));
            }
        }
    }
}
