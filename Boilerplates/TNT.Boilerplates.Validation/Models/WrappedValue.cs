namespace TNT.Boilerplates.Validation.Models
{
    public class WrappedValue<TValue>
    {
        public WrappedValue(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }
    }
}