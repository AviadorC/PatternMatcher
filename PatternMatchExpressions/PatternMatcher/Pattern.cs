using System;

namespace PatternMatcher
{
    public static class Pattern
    {
        public static CaseObject<T> Switch<T>(this T testObject)
        {
            return new CaseObject<T>(testObject, CaseResult.Fault, CaseStatus.Statement);
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
                statement();
            }

            explicitCase.Status = CaseStatus.Statement;
            return patternCase;
        }

        public static CaseObject<T> Break<T>(this CaseObject<T> patternCase)
        {
            var explicitCase = patternCase as ICase<T>;
            if (explicitCase.Result == CaseResult.Match && explicitCase.Status == CaseStatus.Statement)
            {
                explicitCase.Status = CaseStatus.Break;
            }
            return patternCase;
        }

        public class CaseObject<T> : ICase<T>
        {
            public CaseObject(T testObject, CaseResult result, CaseStatus status)
            {
                var self = this as ICase<T>;
                self.TestObject = testObject;
                self.Result = result;
                self.Status = status;
            }

            CaseResult ICase<T>.Result { get; set; }

            CaseStatus ICase<T>.Status { get; set; }

            T ICase<T>.TestObject { get; set; }
        }

        private interface ICase<T>
        {
            T TestObject { get; set; }

            CaseResult Result { get; set; }

            CaseStatus Status { get; set; }
        }
    }
}
