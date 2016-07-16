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
            if (patternCase.Status == CaseStatus.Break)
            {
                return patternCase;
            }

            if (patternCase.Status == CaseStatus.Statement ||
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
            if (patternCase.Result == CaseResult.Match && patternCase.Status == CaseStatus.Statement)
            {
                patternCase.Status = CaseStatus.Break;
            }
            return patternCase;
        }
    }
}
