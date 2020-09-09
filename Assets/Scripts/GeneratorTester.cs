using UnityEngine;

/// <summary>
/// Small utility, out of the game logics. It enables testing of the <see cref="Generator"/> functionality in Edit Mode.
/// </summary>
public class GeneratorTester : MonoBehaviour
{
    [Range(1,10)]
    public int radius;

    private Generator _generator;

    public void OnValidate()
    {
        _generator = null ?? GetComponent<Generator>();
        Main.Rules = null ?? GetComponent<Main>().rules;
    }

    public void Generate()
    {
        _generator.EditModeInitialize();
        _generator.Generate(radius);
    }
}
