using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lable;
    [SerializeField] RectTransform parentToReplacing;
    [SerializeField] bool removeOnPressingBack;
    public void ReplaceToFocusZone() {
        RectTransform target = parentToReplacing == null ? GetComponent<RectTransform>() : parentToReplacing;
        InputSelector.ShowFocusZone(target, lable);
        InputSelector.i.removeOnPressingBack = removeOnPressingBack;
    }
    public void RemoveFromFocusZone() {
        InputSelector.i.OnBackPressed();
    }
}
