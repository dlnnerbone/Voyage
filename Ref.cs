namespace Voyage;

// a custom boxer that stores an underlying object with Ref.
public class Ref<T>(T? value)
{
    public T? boxedValue = value;

    public ref T? AsRef() => ref boxedValue;
    public T? Get() => boxedValue;
    public static implicit operator T?(Ref<T> refObj) => refObj.boxedValue;
    public static implicit operator Ref<T>(T? valueToBox) => new(valueToBox);
}