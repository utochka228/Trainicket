using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SearchedDropdown : MonoBehaviour
{
    public static bool wasPressed;
    public Button button;

    [SerializeField] TextMeshProUGUI text;

    bool from;
    TMP_InputField myInputField;
    string id;
    string city;
    public void Initialize(string _id, string _city, bool from, TMP_InputField inputField) {
        this.from = from;
        myInputField = inputField;
        id = _id;
        city = _city;
        text.text = city;
    }
    public void OnPressed() {
        wasPressed = true;

        if (from) {
            SearchMenu.i.targetSearch.from = id;
            SearchMenu.i.targetSearch.fromName = city;
        } else {
            SearchMenu.i.targetSearch.where = id;
            SearchMenu.i.targetSearch.whereName = city;
        }
        myInputField.text = city;
        //SearchMenu.i.searcher.DestroyDropdowns();
        //SearchMenu.i.searcher.ShowHiddenElements();
        myInputField.GetComponent<InputFieldHandler>().OnRemoveFocusZoneActions?.Invoke();

        wasPressed = false;
    }
}
