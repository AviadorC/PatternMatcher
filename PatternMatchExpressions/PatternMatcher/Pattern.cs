using System;

namespace PatternMatcher
{
    public static class Pattern
    {
        public static CaseObject<T> Switch<T>(this T testObject)
        {
            ICase<T> caseObject = new CaseObject<T>();
            caseObject.TestObject = testObject;
            caseObject.Result = CaseResult.Fault;
            caseObject.Status = CaseStatus.Statement;
            caseObject.CaseRunned = false;

            return caseObject as CaseObject<T>;
        }

        public static CaseObject<T> Case<T>(this CaseObject<T> patternCase, Predicate<T> casePredicate)
        {
            var explicitCase = patternCase as ICase<T>;
            if (explicitCase.Status == CaseStatus.Break)
            {
                return patternCase;
            }

            if (explicitCase.Status == CaseStatus.Statement ||
                (explicitCase.Status == CaseStatus.Pattern && !(explicitCase.Result == CaseResult.Match)))
            {
                explicitCase.Result = casePredicate(explicitCase.TestObject) ? CaseResult.Match : CaseResult.Fault;
            }

            explicitCase.Status = CaseStatus.Pattern;
            return patternCase;
        }

        public static CaseObject<T> Do<T>(this CaseObject<T> patternCase, Action<T> statement)
        {
            var explicitCase = patternCase as ICase<T>;
            if (explicitCase.Status == CaseStatus.Break)
            {
                return patternCase;
            }

            if (explicitCase.Result == CaseResult.Match)
            {
                explicitCase.CaseRunned = true;
                statement(explicitCase.TestObject);
            }

            explicitCase.Status = CaseStatus.Statement;
            return patternCase;
        }

        public static CaseObject<T> Do<T>(this CaseObject<T> patternCase, Action statement)
        {
            var explicitCase = patternCase as ICase<T>;
            if (explicitCase.Status == CaseStatus.Break)
            {
                return patternCase;
            }

            if (explicitCase.Result == CaseResult.Match)
            {
                explicitCase.CaseRunned = true;
                statement();
            }

            explicitCase.Status = CaseStatus.Statement;
            return patternCase;
        }

        public static CaseObject<T> BreakOnMatch<T>(this CaseObject<T> patternCase)
        {
            var explicitCase = patternCase as ICase<T>;
            if (explicitCase.Result == CaseResult.Match && explicitCase.Status == CaseStatus.Statement)
            {
                explicitCase.Status = CaseStatus.Break;
            }
            return patternCase;
        }

        public static void Default<T>(this CaseObject<T> patternCase, Action statement)
        {
            var explicitCase = patternCase as ICase<T>;

            if (!explicitCase.CaseRunned)
            {
                statement();
            }
        }

        public static void Default<T>(this CaseObject<T> patternCase, Action<T> statement)
        {
            var explicitCase = patternCase as ICase<T>;

            if (!explicitCase.CaseRunned)
            {
                statement(explicitCase.TestObject);
            }
        }

        public class CaseObject<T> : ICase<T>
        {
            bool ICase<T>.CaseRunned { get; set; }

            CaseResult ICase<T>.Result { get; set; }

            CaseStatus ICase<T>.Status { get; set; }

            T ICase<T>.TestObject { get; set; }
        }

        private interface ICase<T>
        {
            bool CaseRunned { get; set; }

            T TestObject { get; set; }

            CaseResult Result { get; set; }

            CaseStatus Status { get; set; }
        }

        private enum CaseResult
        {
            Match,
            Fault
        }

        private enum CaseStatus
        {
            Pattern,
            Statement,
            Break
        }
    }
}
