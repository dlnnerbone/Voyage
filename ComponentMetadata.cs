namespace Voyage;

internal static class ComponentCounter 
{
  internal static ushort _totalTypesCounted = 0;
}

public static class ComponentMetadata<T> 
{
  public static readonly ushort ID = ComponentCounter._totalTypesCounted++;
  public static readonly Type ComponentType = typeof(T);
  public static readonly object[] Attributes = typeof(T).GetCustomAttributes(false);
  public static readonly bool IsReference = typeof(T).IsClass;
}
