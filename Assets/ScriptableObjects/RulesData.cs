using UnityEngine;

[CreateAssetMenu(fileName = "Rules", menuName = "WorldGenerator/Rules", order = 0)]
public class RulesData : ScriptableObject
{
    [Header("Generator")]
    public int defaultRadius;

    [Header("Hexagon")]
    [Range(1,10)]
    public int propProbability;

    [Header("UIManager")]
    public Vector2 radiusRange;

    [Header("WanderBehaviour")]
    [Range(1, 10)]
    public int idleProbability;
    public Vector2 idleTimeRange;
}
