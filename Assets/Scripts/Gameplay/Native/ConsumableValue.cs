public struct ConsumableValue<T> where T : struct
{
    public T? Value { get; set; }

    public T? Consume()
    {
        T? cpy = Value;
        Value = null;
        return cpy;
    }
}
