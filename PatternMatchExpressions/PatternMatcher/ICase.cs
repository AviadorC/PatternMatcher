namespace PatternMatcher
{
    interface ICase<T>
    {
        T TestObject { get; set; }

        CaseResult Result { get; set; }

        CaseStatus Status { get; set; }
    }
}
