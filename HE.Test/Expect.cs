using System;
using NUnit.Framework;

namespace HE.Test
{
    public class Expect
    {
        public static void FloatsAreEqual(double actual, double expected)
        {
            if (Math.Abs(actual - expected) > 0.00001)
            {
                Assert.Fail("expected: " + expected + ", actual:" + actual);
            }
        }
    }
}