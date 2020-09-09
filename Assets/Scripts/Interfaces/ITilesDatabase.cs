using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface that handles loading of the assets into a tiles database.
/// </summary>
public interface ITilesDatabase
{
    Dictionary<Types, Tile> Tiles { get; }

    void Initialize(GameObject tilesLibrary);
}
