using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]

[RequireComponent(typeof(ContentSizeFitter))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string localizationKey;
    public TextMeshProUGUI text;
    private void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = LocalizationManager.instance.GetLocalizedValue(localizationKey);
        LocalizationManager.OnLocalizationChanged += changeLocalization;
    }
    private void OnDisable()
    {
        LocalizationManager.OnLocalizationChanged -= changeLocalization;
    }
    private void changeLocalization() {
        text.text = LocalizationManager.instance.GetLocalizedValue(localizationKey);
    }
}
