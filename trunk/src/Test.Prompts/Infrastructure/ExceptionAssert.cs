using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Prompts.Infrastructure
{
    internal static class ExceptionAssert
    {
        public static void Throws<T>(Action action) where T : Exception
        {
            Throws<T>(action, e => { });
        }

        public static void Throws<T>(string expectedMessage, Action action) where T : Exception
        {
            Throws<T>(action, e => Assert.AreEqual(expectedMessage, e.Message));
        }

        private static void Throws<T>(Action action, Action<T> validateException) where T : Exception
        {
            var numberOfExceptions = 0;
            try
            {
                action();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(T), e.GetType());
                validateException((T)e);
                numberOfExceptions++;
            }

            Assert.AreEqual(1, numberOfExceptions);
        }
    }
}
