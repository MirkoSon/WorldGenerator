using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extracts tiles and props from the tilesLibrary prefab and sorts them in a dictionary using <see cref="Hexagon"/> and <see cref="Prop"/> combined in <see cref="Tile"/>
/// </summary>
public class TilesDatabase : ITilesDatabase
{
    private Dictionary<Types, Tile> _tiles;
    private Hexagon[] _hexagons;
    private Prop[] _props;

    public Dictionary<Types, Tile> Tiles
    {
        get
        {
            return _tiles;
        }
    }

    public void Initialize(GameObject tilesLibrary)
    {
        _tiles = new Dictionary<Types, Tile>();

        LoadAssets(tilesLibrary);
        GenerateTilesFromAssets();
    }

    private void LoadAssets(GameObject tilesLibrary)
    {
        _hexagons = tilesLibrary.GetComponentsInChildren<Hexagon>();
        _props = tilesLibrary.GetComponentsInChildren<Prop>();
    }

    private void GenerateTilesFromAssets()
    {
        foreach (int i in Enum.GetValues(typeof(Types)))
        {
            GameObject hexagon = _hexagons.FirstOrDefault(x => x.type == (Types)i)?.gameObject;

            List<GameObject> propsForType = new List<GameObject>();

            foreach (Prop p in _props)
                if (p.type == (Types)i)
                    propsForType.Add(p.gameObject);

            AddTileToDictionary((Types)i, hexagon, propsForType);
        }
    }

    private void AddTileToDictionary(Types type, GameObject hexagon, List<GameObject> props)
    {
        Tile tile = new Tile()
        {
            hexagon = hexagon,
            props = props
        };

        _tiles[type] = tile;
    }
}