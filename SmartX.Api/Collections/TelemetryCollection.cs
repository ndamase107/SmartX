namespace SmartX.Api.Collections;

public class TelemetryCollection<T>
{
    private readonly List<T> _items = [];

    public int Count => _items.Count;

    public void Add(T item)
    {
        _items.Add(item);
    }

    public void AddRange(IEnumerable<T> items)
    {
        _items.AddRange(items);
    }

    public T this[int index]
    {
        get => _items[index];
    }

    public IReadOnlyList<T> Items => _items;

    public IEnumerable<T> Where(
        Func<T, bool> predicate)
    {
        return _items.Where(predicate);
    }

    public void Clear()
    {
        _items.Clear();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }
}