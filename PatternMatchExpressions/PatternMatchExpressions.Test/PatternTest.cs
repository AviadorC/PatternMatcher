using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PatternMatcher.Test
{
    public class ABC
    {
        public int A { get; set; }
        public double B { get; set; }
        public string C { get; set; }
    }

    [TestClass]
    public class PatternTest
    {
        [TestMethod]
        public void Int_ShouldPass_CS()
        {
            int value = 3;

            bool success = false;

            Pattern.Switch(value)
                .PatternCase((v) => v > 0 && v < 5)
                .Do((v) => success = true);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void Int_ShouldPass_CSCSCS()
        {
            int value = 5;

            bool r1 = false;
            bool r2 = true;
            bool r3 = false;

            Pattern.Switch(value)
                .PatternCase((v) => v > 0 && v < 10)
                .Do((v) => r1 = true)
                .PatternCase((v) => v < 2 || v > 10)
                .Do((v) => r2 = false)
                .PatternCase((v) => v > 0)
                .Do((v) => r3 = true);

            Assert.IsTrue(r1);
            Assert.IsTrue(r2);
            Assert.IsTrue(r3);
        }

        [TestMethod]
        public void Int_ShouldPass_CCS()
        {
            int value = 5;

            bool result = false;

            Pattern.Switch(value)
                .PatternCase((v) => v > 6)
                .PatternCase((v) => v > 3 && v < 7)
                .Do((v) => result = true);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Int_ShouldPass_Break()
        {
            int value = 5;

            Pattern.Switch(value)
                .PatternCase((v) => v > 3)
                .Do(() => value++)
                .PatternCase((v) => v < 7)
                .PatternCase((v) => v < 10)
                .Do(() => value++)
                .Break()
                .PatternCase((v) => v > 1)
                .Do(() => value++);

            Assert.AreEqual(7, value);
        }

        [TestMethod]
        public void Int_ShouldPass_NotCalledStatement_NoBreak()
        {
            int value = 5;

            Pattern.Switch(value)
                .PatternCase((v) => v > 3)
                .Do(() => value++)
                .PatternCase((v) => v < 1)
                .PatternCase((v) => v > 10)
                .Do(() => value++)
                .Break()
                .PatternCase((v) => v > 1)
                .Do(() => value += 5);

            Assert.AreEqual(11, value);
        }

        [TestMethod]
        public void Object_ShouldPass()
        {
            ABC value = new ABC()
            {
                A = 1,
                B = 2.0,
                C = "ABC"
            };

            Pattern.Switch(value)
                .PatternCase(v => v.A == 1 && v.C.Length == 3)
                .Do(v => v.A++)
                .PatternCase(v => v.A > 0)
                .PatternCase(v => v.B > 0)
                .Do(v => v.B += 3.0);

            Assert.AreEqual(5.0, value.B);
            Assert.AreEqual(2, value.A);
        }

        [TestMethod]
        public void MultiPattern()
        {
            var multiVar = Tuple.Create(1, 2.0f, "ABC");

            bool r1 = false;
            bool r2 = false;

            Pattern.Switch(multiVar)
                .PatternCase(v => v.Item1 == 1 && v.Item2 == 3 && v.Item3.Length > 0)
                .Do(v =>
                {
                    r1 = true;
                })
                .PatternCase<Tuple<int, float, string>>(v => v.Item1 == 1 && v.Item2 == 2.0f)
                .Do(v =>
                {
                    r2 = true;
                });

            Assert.IsFalse(r1);
            Assert.IsTrue(r2);
        }
    }
}
