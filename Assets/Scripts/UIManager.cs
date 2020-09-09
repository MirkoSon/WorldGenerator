using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the UI Panel from where the player can generate the world
/// </summary>
public class UIManager : MonoBehaviour
{
    public Button button;
    public Text label;
    public Slider slider;
    public GameObject generatingLabel;

    private Generator _generator;

    public void Initialize(IDependencyContainer dependencyContainer)
    {
        button.onClick.AddListener(OnGenerateButtonClick);
        slider.onValueChanged.AddListener(delegate { OnSliderValueChange(); });

        _generator = dependencyContainer.Resolve<Generator>();

        slider.minValue = Main.Rules.radiusRange.x;
        slider.maxValue = Main.Rules.radiusRange.y;
        slider.value = Main.Rules.defaultRadius;
        OnSliderValueChange();
    }

    void OnGenerateButtonClick()
    {
        generatingLabel.SetActive(true);
        StartCoroutine(GenerateWithDelay());
    }

    // A delay is needed to make sure the label gets visualized.
    IEnumerator GenerateWithDelay()
    {
        yield return new WaitForFixedUpdate();
        _generator.Generate((int)slider.value);
    }

    void OnSliderValueChange()
    {
        label.text = string.Format("Radius: {0}", slider.value);
    }

    private void OnApplicationQuit()
    {
        button.onClick.RemoveAllListeners();
        slider.onValueChanged.RemoveAllListeners();
    }
}
