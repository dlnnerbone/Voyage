namespace Voyage.Operation;

public class RefDictionary<TKey, TValue> where TKey : notnull
{
    internal readonly Dictionary<TKey, Ref<TValue>> _dictionary;

    public int Count => _dictionary.Count;
    public RefDictionary()
    {
        _dictionary = [];
    }

    public ref TValue? this[TKey key] => ref _dictionary[key].AsRef();

    public void Add(TKey key, TValue? val) => _dictionary.Add(key, val);
    public bool Remove(TKey key) => _dictionary.Remove(key);

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);
    public bool ContainsValue(TValue? val) => _dictionary.ContainsValue(val);
    public bool TryAdd(TKey key, TValue? val) => _dictionary.TryAdd(key, val);
}