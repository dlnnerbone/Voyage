namespace Voyage.Operation;

public partial class Archetype
{
    public bool HasComponent<TComp>()
    {
        ushort compID = ComponentMetadata<TComp>.ID;
        return _typeSet.ToArray()[compID] == typeof(TComp);
    }

    public bool HasComponent<TComp>(out ushort componentID)
    {
        componentID = ComponentMetadata<TComp>.ID;
        return _typeSet.ToArray()[componentID] == typeof(TComp);
    }

    // Module Upfronts

    public void ToggleElement<T>(ushort index, bool toggle)
    {
        var module = GetModule<T>();
        module.ToggleElement(index, toggle);
    }

    public void ToggleElementSelection<T>(int[] indices, bool toggle)
    {
        var module = GetModule<T>();
        module.ToggleElementSelection(indices, toggle);
    }

    public bool IsElementActive<T>(int index)
    {
        var module = GetModule<T>();
        return module.IsActive(index);
    }
}