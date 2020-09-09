using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GeneratorTester))]
public class CustomGeneratorTester : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GeneratorTester tester = (GeneratorTester)target;

        if(GUILayout.Button("Generate"))
        {
            tester.Generate();
        }
    }
}
