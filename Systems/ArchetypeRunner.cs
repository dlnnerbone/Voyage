namespace Voyage.Operation;

public class ArchetypeRunner(Archetype arch)
{
    public delegate void UpdateSequence<TComp>(ref TComp component);

    internal readonly Archetype _archetype = arch;

    public void UpdateModule<TComp>(UpdateSequence<TComp> componentRunner)
    {
        var mod = _archetype.GetModule<TComp>();
        var denseLength = mod._denseSet.Length;
        if (mod.Length == 0 || denseLength == 0) return;

        for(int i = 0; i < denseLength; i++)
        {
            componentRunner(ref mod[mod._denseSet[i]]);
        }
    }
}