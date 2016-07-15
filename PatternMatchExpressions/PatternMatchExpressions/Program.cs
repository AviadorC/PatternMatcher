using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternMatchExpressions
{
    public class ABC
    {
        public int A { get; set; }
        public int B { get; set; }
        public string C { get; set; }
    }

    public enum CaseResult
    {
        Match,
        Fault
    }

    public class Case<T>
    {
        public Case(T testObject, CaseResult result)
        {
            TestObject = testObject;
            Result = result;
        }

        public T TestObject { get; set; }

        public CaseResult Result { get; set; }
    }

    public static class Switch
    {
        public static Case<T> Case<T>(this T testObject, Predicate<T> casePredicate)
        {
            CaseResult result = casePredicate(testObject) ? CaseResult.Match : CaseResult.Fault;
            return new Case<T>(testObject, result);
        }

        public static Case<T> Case<T>(this Case<T> patternCase, Predicate<T> casePredicate)
        {
            CaseResult result = casePredicate(patternCase.TestObject) ? CaseResult.Match : CaseResult.Fault;
            patternCase.Result = result;
            return patternCase;
        }

        public static Case<T> Statement<T>(this Case<T> patternCase, Action<T> statement)
        {
            if(patternCase.Result == CaseResult.Match)
            {
                statement(patternCase.TestObject);
            }

            return patternCase;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var test = new ABC()
            {
                A = 0,
                B = 7,
                C = "ABC"
            };

            test
                .Case((t) =>
                {
                    Console.WriteLine("Case1");
                    var result = false;
                    if (t.A > 0 && t.B < 6)
                    {
                        result = true;
                    }
                    Console.WriteLine($"Case1 : {result}");
                    return result;
                })
                .Statement((s) =>
                {
                    Console.WriteLine("Statement1");
                    s.A++;
                    s.B--;
                    s.C += "D";
                })
                .Case((t) =>
                {
                    Console.WriteLine("Case2");
                    bool result = false;
                    if (t.C.Contains("AB"))
                    {
                        result = true;
                    }
                    return result;
                })
                .Statement((s) =>
                {
                    Console.WriteLine("Statement2");
                    s.C = "QWER";
                });

            Console.ReadLine();
        }
    }
}
