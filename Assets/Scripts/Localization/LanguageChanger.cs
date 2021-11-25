using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageChanger : MonoBehaviour
{
    [SerializeField] TMP_Dropdown language;
    public void ChangeLocalization() {
        switch (language.value) {
            case 0:
                Debug.Log("Localized to English.");
                UIMenuManager.Instance.ChangeLocalization("localizedText_en");
                break;
            case 1:
                Debug.Log("Localized to Ukrainian.");
                UIMenuManager.Instance.ChangeLocalization("localizedText_ua");
                break;
            case 2:
                Debug.Log("Localized to Russian.");
                UIMenuManager.Instance.ChangeLocalization("localizedText_ru");
                break;
            default:
                break;
        }
    }
}
