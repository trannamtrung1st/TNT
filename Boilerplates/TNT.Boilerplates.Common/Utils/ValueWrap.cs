namespace TNT.Boilerplates.Common.Utils
{
    public class ValueWrap<TValue>
    {
        public ValueWrap(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }
    }
}