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

    public enum CaseStatus
    {
        Pattern,
        Statement,
        Break
    }

    public class Case<T>
    {
        public Case(T testObject, CaseResult result, CaseStatus status)
        {
            TestObject = testObject;
            Result = result;
            Status = status;
        }

        public T TestObject { get; set; }

        public CaseResult Result { get; set; }

        public CaseStatus Status { get; set; }
    }

    public static class Switch
    {
        public static Case<T> Case<T>(this T testObject, Predicate<T> casePredicate)
        {
            CaseResult result = casePredicate(testObject) ? CaseResult.Match : CaseResult.Fault;
            return new Case<T>(testObject, result, CaseStatus.Pattern);
        }

        public static Case<T> Case<T>(this Case<T> patternCase, Predicate<T> casePredicate)
        {
            if(patternCase.Status == CaseStatus.Break)
            {
                return patternCase;
            }

            if(patternCase.Status == CaseStatus.Statement ||
                (patternCase.Status == CaseStatus.Pattern && !(patternCase.Result == CaseResult.Match)))
            {
                patternCase.Result = casePredicate(patternCase.TestObject) ? CaseResult.Match : CaseResult.Fault;
            }

            patternCase.Status = CaseStatus.Pattern;
            return patternCase;
        }

        public static Case<T> Do<T>(this Case<T> patternCase, Action<T> statement)
        {
            if (patternCase.Status == CaseStatus.Break)
            {
                return patternCase;
            }

            if (patternCase.Result == CaseResult.Match)
            {
                statement(patternCase.TestObject);
            }

            patternCase.Status = CaseStatus.Statement;
            return patternCase;
        }

        public static Case<T> Do<T>(this Case<T> patternCase, Action statement)
        {
            if (patternCase.Status == CaseStatus.Break)
            {
                return patternCase;
            }

            if (patternCase.Result == CaseResult.Match)
            {
                statement();
            }

            patternCase.Status = CaseStatus.Statement;
            return patternCase;
        }

        public static Case<T> Break<T>(this Case<T> patternCase)
        {
            if(patternCase.Result == CaseResult.Match && patternCase.Status == CaseStatus.Statement)
            {
                patternCase.Status = CaseStatus.Break;
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
                .Case((t) => t.A > 0 && t.B < 6)
                .Do((s) =>
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
                .Do((s) =>
                {
                    Console.WriteLine("Statement2");
                    s.C = "QWER";
                });

            Console.ReadLine();
        }
    }
}