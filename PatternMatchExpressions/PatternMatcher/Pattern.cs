using System;

namespace PatternMatcher
{
    public static class Pattern
    {
        public static Case<T> Switch<T>(this T testObject)
        {
            return new Case<T>(testObject, CaseResult.Fault, CaseStatus.Statement);
        }

        public static Case<T> PatternCase<T>(this Case<T> patternCase, Predicate<T> casePredicate)
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

        public static Case<T> Do<T>(this Case<T> patternCase, Action<T> statement)
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

        public static Case<T> Do<T>(this Case<T> patternCase, Action statement)
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

        public static Case<T> Break<T>(this Case<T> patternCase)
        {
            var explicitCase = patternCase as ICase<T>;
            if (explicitCase.Result == CaseResult.Match && explicitCase.Status == CaseStatus.Statement)
            {
                explicitCase.Status = CaseStatus.Break;
            }
            return patternCase;
        }

        public class Case<T> : ICase<T>
        {
            public Case(T testObject, CaseResult result, CaseStatus status)
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
