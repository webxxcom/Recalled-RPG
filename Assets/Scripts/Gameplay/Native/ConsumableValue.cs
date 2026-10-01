public struct ConsumableValue<T>
{
    public T Value { get; set; }

    public T Consume()
    {
        T cpy = Value;
        Value = default;
        return cpy;
    }
}
