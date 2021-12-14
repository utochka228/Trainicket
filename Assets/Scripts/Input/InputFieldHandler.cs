using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InputFieldHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lable;
    [SerializeField] RectTransform parentToReplacing;
    [SerializeField] bool removeOnPressingBack;
    public UnityEvent OnRemoveFocusZoneActions;

    bool replaced;
    bool removed;
    public void ReplaceToFocusZone() {
        removed = false;
        if (replaced)
            return;
        RectTransform target = parentToReplacing == null ? GetComponent<RectTransform>() : parentToReplacing;
        InputSelector.ShowFocusZone(target, lable);
        InputSelector.i.removeOnPressingBack = removeOnPressingBack;
        replaced = true;
    }
    public void RemoveFromFocusZone() {
        replaced = false;
        if (removed)
            return;
        InputSelector.i.OnBackPressed();
        removed = true;
    }
}
