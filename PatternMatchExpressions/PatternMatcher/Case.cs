namespace PatternMatcher
{
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
}
