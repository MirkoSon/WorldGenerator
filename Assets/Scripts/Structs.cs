using System.Collections.Generic;
using UnityEngine;

public struct Tile
{
    public GameObject hexagon;
    public List<GameObject> props;
}

public enum Types
{
    Forest,
    Meadow,
    Park,
    Sand,
    Water
}