namespace Voyage;

internal static class PrimaryWorld
{
    internal static World _world = null!;

    internal static bool IsNull() => _world == null;
}