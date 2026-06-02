using System.Runtime.CompilerServices;
using System.Collections.Immutable;
namespace Voyage.Operation;

public class Module2<T> : IModule<T>
{
    // an array of the chosen components.
    private T[] _compBuffer;
    // Sparse Set values are used to access dense set indices
    private ushort[] _activeComponentsSparse;
    // dense set that access active components
    private ushort[] _activeComponentsDense = [];
    // hat components are active.
    private bool[] _activatedElements;
    private int denseCount = 0;

    public ref T this[ushort id] => ref _compBuffer[id];
    public ref T AttainByDense(ushort id) => ref _compBuffer[_activeComponentsDense[id]];

    public int Length => _compBuffer.Length;
    public int DenseCount => denseCount;
    public T[] GetBuffer() => _compBuffer;

    public ImmutableArray<ushort> GetSparseSet() => [.. _activeComponentsSparse ];
    public ImmutableArray<ushort> GetDenseSet() => [ .. _activeComponentsDense ];
    public ImmutableArray<bool> ActivatedElements() => [.. _activatedElements ];

    public Module2(ushort initLength)
    {
        _compBuffer = new T[initLength];
        _activeComponentsSparse = new ushort[initLength];
        _activatedElements = new bool[initLength];
    }

    public Module2(T[] initBuffer)
    {
        _compBuffer = initBuffer;
        _activeComponentsSparse = new ushort[initBuffer.Length];
        _activatedElements = new bool[initBuffer.Length];
    }

    public bool IsActive(ushort index) => _activatedElements[index] == true;

    public void ToggleElement(ushort index, bool mode)
    {
        if (index > Length - 1) throw new ArgumentOutOfRangeException($"index: {index} is out of bounds of the module.");

        bool canIncrement = false;
        if (_activatedElements[index] != mode)
        {
            _activatedElements[index] = mode;
            canIncrement = true;
        }

        if (canIncrement && mode)
        {
            ++denseCount;
            Refresh();
        }
        else if (canIncrement && !mode)
        {
            --denseCount;
            Refresh();
        }
        
    }

    public void Refresh()
    {
        _activeComponentsDense = new ushort[denseCount];

        ushort _denseCounter = 0;
        for(ushort i = 0; i < Length; i++)
        {
            if (!_activatedElements[i]) continue;

            var next = _denseCounter++;
            _activeComponentsDense[next] = i;
            _activeComponentsSparse[i] = next;
        }
    }

    public void ResizeModule(ushort newLength)
    {
        denseCount = 0;
        bool isLower = newLength < Length;
        var min = Math.Min(Length, newLength);

        T[] newBuffer = new T[newLength];
        _activeComponentsSparse = new ushort[newLength];
        bool[] _activatedElems = new bool[newLength];

        for(ushort i = 0; i < min; i++)
        {
            newBuffer[i] = _compBuffer[i];

            if (!_activatedElements[i]) continue;

            ++denseCount;
            _activatedElems[i] = _activatedElements[i];
        }

        _compBuffer = newBuffer;
        _activatedElements = _activatedElems;
        if (isLower) Refresh();
    }

    public void ToggleElementSelection(ushort[] indices, bool mode)
    {
        throw new NotImplementedException();
    }

}