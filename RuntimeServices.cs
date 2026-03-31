using System.Reflection;
using Voyage.Operation;
namespace Voyage.Helper;

public static class RuntimeServices
{

    public static bool IsReference<T>()
    {
        return typeof(T).IsClass || typeof(T).IsArray;
    }

    public static bool IsReferenceOrContainsReferences<T>()
    {
        if (IsReference<T>()) return true;

        Type type = typeof(T);

        foreach(var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public))
        {
            if (field.GetType().IsClass) return true;
        }

        foreach(var prop in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public))
        {
            if (prop.GetType().IsClass) return true;
        }
        return false;
    }
}