using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatternMatchExpressions;

namespace PatternMatchExpressions.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Int_ShouldPass_CS()
        {
            int value = 3;

            bool success = false;

            value
                .Case((v) => v > 0 && v < 5)
                .Statement((v) => success = true);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void Int_ShouldPass_CSCSCS()
        {
            int value = 5;

            bool r1 = false;
            bool r2 = true;
            bool r3 = false;

            value
                .Case((v) => v > 0 && v < 10)
                .Statement((v) => r1 = true)
                .Case((v) => v < 2 || v > 10)
                .Statement((v) => r2 = false)
                .Case((v) => v > 0)
                .Statement((v) => r3 = true);

            Assert.IsTrue(r1);
            Assert.IsTrue(r2);
            Assert.IsTrue(r3);
        }

        [TestMethod]
        public void Int_ShouldPass_CCS()
        {
            int value = 5;

            bool result = false;

            value
                .Case((v) => v > 6)
                .Case((v) => v > 3 && v < 7)
                .Statement((v) => result = true);

            Assert.IsTrue(result);
        }
    }
}
