using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Generates the world's tiles based on the content of <see cref="TilesDatabase"/>
/// </summary>
public class Generator : MonoBehaviour
{
    public UnityAction OnWorldGenerated;

    private Main _main;

    private TilesDatabase _db;

    private Vector3 _size;

    public void Initialize(IDependencyContainer dependencyContainer)
    {
        _main = dependencyContainer.Resolve<Main>();
        _db = dependencyContainer.Resolve<TilesDatabase>();
        MeasureTile();
        Generate(Main.Rules.defaultRadius);
    }

    public void EditModeInitialize()
    {
        _main = GetComponent<Main>();

        if (_db == null)
        {
            _db = new TilesDatabase();
            _db.Initialize(_main.tilesLibrary);
        }

        if (_size == Vector3.zero)
        {
            MeasureTile();
        }
    }

    public void Generate(int mapRadius)
    {
        DestroyPrevious();

        float _radius = _size.x / 2;
        float height = _radius * 2;
        float width = _radius * Mathf.Sqrt(3);

        Vector2 spacing = new Vector2(width / 2f, height * 0.75f);
        int xCount = Mathf.CeilToInt(mapRadius / width);
        int yCount = Mathf.CeilToInt(mapRadius / spacing.y);
        float circleRadiusSqr = mapRadius * mapRadius;

        for (int x = -xCount - 1; x <= xCount; x++)
        {
            for (int y = -yCount; y <= yCount; y++)
            {
                float yOffset = y * spacing.y;
                float xOffset = x * width + ((y % 2) * spacing.x);

                if (xOffset * xOffset + yOffset * yOffset <= circleRadiusSqr)
                {
                    Vector3 position = Vector3.zero + Vector3.left * xOffset;
                    position = position + Vector3.forward * yOffset;

                    int id = Random.Range(0, _db.Tiles.Count);

                    GameObject newTile =
                        Instantiate(_db.Tiles[(Types)id].hexagon, position, Quaternion.Euler(-90, 0, 90), _main.tileGroup.transform);

                    newTile.GetComponent<Hexagon>().Initialize(_db);
                }
            }
        }

        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();

        OnWorldGenerated?.Invoke();
    }

    private void MeasureTile()
    {
        GameObject centralTile = Instantiate(_db.Tiles[(Types)0].hexagon, Vector3.zero, Quaternion.Euler(-90, 0, 0));
        MeshCollider collider = centralTile.AddComponent<MeshCollider>();
        _size = collider.bounds.size;
        DestroyImmediate(centralTile);
    }

    private void DestroyPrevious()
    {
        Hexagon[] previousHexs = FindObjectsOfType<Hexagon>();

        if (previousHexs.Length != 0)
        {
            foreach (Hexagon h in previousHexs)
            {
                DestroyImmediate(h.gameObject);
            }
        }
    }
}
