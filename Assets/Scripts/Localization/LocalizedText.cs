using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]

[RequireComponent(typeof(ContentSizeFitter))]
public class LocalizedText : MonoBehaviour
{
    public string localizationKey;
    public TextMeshProUGUI text;
    private void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = LocalizationManager.instance.GetLocalizedValue(localizationKey);
        LocalizationManager.OnLocalizationChanged += ChangeLocalization;
    }
    private void OnDisable()
    {
        LocalizationManager.OnLocalizationChanged -= ChangeLocalization;
    }
    public void ChangeLocalization() {
        text.text = LocalizationManager.instance.GetLocalizedValue(localizationKey);
    }
}
