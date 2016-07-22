using System;

namespace PatternMatcher
{
    public static class Pattern
    {
        public static Case<T> Switch<T>(this T testObject)
        {
            return new Case<T>(testObject, CaseResult.Fault, CaseStatus.Statement);
        }

        //public static Case<T> Case<T>(this T testObject, Predicate<T> casePredicate)
        //{
        //    CaseResult result = casePredicate(testObject) ? CaseResult.Match : CaseResult.Fault;
        //    return new Case<T>(testObject, result, CaseStatus.Pattern);
        //}

        public static Case<T> Case<T>(this Case<T> patternCase, Predicate<T> casePredicate)
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
    }
}
