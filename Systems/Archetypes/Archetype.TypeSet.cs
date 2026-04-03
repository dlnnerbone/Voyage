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

    
}