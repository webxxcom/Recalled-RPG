using System.Collections.Generic;

public abstract class ValueAggregator<T>
{
    readonly List<T> _values = new();
    public List<T> Values => _values;

    public void Add(T value) => _values.Add(value);
    public void Remove(T value) => _values.Remove(value);
    public abstract float Get();
}
