namespace Voyage;

internal static class ComponentCounter 
{
  internal static ushort _totalTypesCounted = 0;
}

public static class ComponentMetadata<T> 
{
  public static readonly ushort ID = ComponentCache.GetID<T>();
  public static readonly Type Type = typeof(T);
  public static readonly object[] Attributes = typeof(T).GetCustomAttributes(false);
}

public static class ComponentCache
{
  internal static readonly Dictionary<Type, ushort> _entries = new();
  public static ushort GetID<T>() => _entries[typeof(T)]; 
  public static ushort GetID(Type type) => _entries[type];

  public static void Add(Type type)
  {
    if (_entries.ContainsKey(type)) return;

    _entries.Add(type, ComponentCounter._totalTypesCounted++);
  }

  public static void Add<T>() => Add(typeof(T));

  public static void AddRange(IEnumerable<Type> types)
  {
    var toArr = types.ToArray();
    
    for(int i = 0; i < toArr.Length; i++) Add(toArr[i]);
  }
}
