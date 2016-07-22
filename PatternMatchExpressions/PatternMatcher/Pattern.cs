using System;

namespace PatternMatcher
{
    /// <summary>
    /// Static pattern class, exposes case/do chain as extension methods.
    /// </summary>
    public static class Pattern
    {
        /// <summary>
        /// Switches on the specified test object.
        /// Starting point for building case. Accepts any test object and exposes CaseObject for
        /// extension methods to build cases on.
        /// </summary>
        /// <typeparam name="T">Case type</typeparam>
        /// <param name="testObject">The test object.</param>
        /// <returns></returns>
        public static CaseObject<T> Switch<T>(this T testObject)
        {
            ICase<T> caseObject = new CaseObject<T>();
            caseObject.TestObject = testObject;
            caseObject.Result = CaseResult.Fault;
            caseObject.Status = CaseStatus.Statement;
            caseObject.CaseRunned = false;

            return caseObject as CaseObject<T>;
        }

        /// <summary>
        /// Checks if specified case matches predicate
        /// </summary>
        /// <typeparam name="T">Case type</typeparam>
        /// <param name="patternCase">The pattern case.</param>
        /// <param name="casePredicate">The case predicate.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Executes code if case has succeded.
        /// </summary>
        /// <typeparam name="T">Case type</typeparam>
        /// <param name="patternCase">The pattern case.</param>
        /// <param name="statement">The statement. Exposes test object for statement.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Executes code if case has succeded.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="patternCase">The pattern case.</param>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
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

        /// <summary>
        /// If predecessing case matches and executes code as statement, no more cases will
        /// execute their code even if case is successfull.
        /// </summary>
        /// <typeparam name="T">Case type</typeparam>
        /// <param name="patternCase">The pattern case.</param>
        /// <returns></returns>
        public static CaseObject<T> BreakOnMatch<T>(this CaseObject<T> patternCase)
        {
            var explicitCase = patternCase as ICase<T>;
            if (explicitCase.Result == CaseResult.Match && explicitCase.Status == CaseStatus.Statement)
            {
                explicitCase.Status = CaseStatus.Break;
            }
            return patternCase;
        }

        /// <summary>
        /// If no case will run their statement code default statement will run.
        /// </summary>
        /// <typeparam name="T">Case type</typeparam>
        /// <param name="patternCase">The pattern case.</param>
        /// <param name="statement">The statement.</param>
        public static void Default<T>(this CaseObject<T> patternCase, Action statement)
        {
            var explicitCase = patternCase as ICase<T>;

            if (!explicitCase.CaseRunned)
            {
                statement();
            }
        }

        /// <summary>
        /// If no case will run their statement code default statement will run.
        /// </summary>
        /// <typeparam name="T">Case type</typeparam>
        /// <param name="patternCase">The pattern case.</param>
        /// <param name="statement">The statement. Exposes test object.</param>
        public static void Default<T>(this CaseObject<T> patternCase, Action<T> statement)
        {
            var explicitCase = patternCase as ICase<T>;

            if (!explicitCase.CaseRunned)
            {
                statement(explicitCase.TestObject);
            }
        }

        /// <summary>
        /// Case object used for case chain build up. Hides inner mechanisms using explicit interface
        /// implementation of private interface ICase.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class CaseObject<T> : ICase<T>
        {
            /// <summary>
            /// Gets or sets a value indicating whether case statement fired.
            /// </summary>
            bool ICase<T>.CaseRunned { get; set; }

            /// <summary>
            /// Gets or sets the case result.
            /// </summary>
            CaseResult ICase<T>.Result { get; set; }

            /// <summary>
            /// Gets or sets the case status.
            /// </summary>
            CaseStatus ICase<T>.Status { get; set; }

            /// <summary>
            /// Gets or sets the test object.
            /// </summary>
            T ICase<T>.TestObject { get; set; }
        }

        /// <summary>
        /// Private interface used as a key for exposing inner mechanism properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private interface ICase<T>
        {
            bool CaseRunned { get; set; }

            T TestObject { get; set; }

            CaseResult Result { get; set; }

            CaseStatus Status { get; set; }
        }

        private enum CaseResult
        {
            /// <summary>
            /// Case predicate returned true meaning case matches.
            /// </summary>
            Match,

            /// <summary>
            /// Case predicate returned false meaning case faulted.
            /// </summary>
            Fault
        }

        private enum CaseStatus
        {
            /// <summary>
            /// Last chain element was case pattern.
            /// </summary>
            Pattern,

            /// <summary>
            /// Last chain element was statement.
            /// </summary>
            Statement,

            /// <summary>
            /// Last chain element was break.
            /// </summary>
            Break
        }
    }
}
