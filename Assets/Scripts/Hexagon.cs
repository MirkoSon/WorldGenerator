using UnityEngine;

/// <summary>
/// Tags the asset with a hexagon type to be classified by <see cref="ITilesDatabase"/> and handles its decoration accordingly, following <see cref="RulesData"/>
/// </summary>
public class Hexagon : MonoBehaviour
{
    public Types type;

    private TilesDatabase _db;

    private int _propProbability;

    public void Initialize(TilesDatabase db)
    {
        _db = db;
        PropProbabilityRoll();

        // Water tiles need UV maps for the animated shader to work.
        if (type == Types.Water)
            GenerateUVs();
    }

    private void PropProbabilityRoll()
    {
        int roll = Random.Range(1, 10);

        //Prop probability are fixed to 5 in Edit Mode
        _propProbability = Main.Rules != null ? Main.Rules.propProbability : 5;

        if (roll > _propProbability)
        {
            GenerateProp();
        }
    }

    private void GenerateProp()
    {
        int propID = Random.Range(0, _db.Tiles[type].props.Count);
        GameObject prop =
            Instantiate(_db.Tiles[type].props[propID], transform.localPosition, transform.localRotation, transform);

        prop.transform.localScale = Vector3.one;
    }

    private void GenerateUVs()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        Vector2[] newUVs = UnityEditor.Unwrapping.GeneratePerTriangleUV(mesh);

        Vector2[] uvs = new Vector2[mesh.vertices.Length];

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            for (int j = 0; j < mesh.triangles.Length; j++)
            {
                if (mesh.triangles[j] == i)
                {
                    uvs[i] = newUVs[j];
                }
            }
        }

        mesh.uv = uvs;
    }
}
