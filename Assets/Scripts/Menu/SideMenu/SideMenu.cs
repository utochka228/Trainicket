using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenu : MenuItem<SideMenu>
{
    [SerializeField] LayoutElement layoutElement;
    [Range(0f, 1f)]
    [SerializeField] float menuPerWidth;

    [SerializeField] Image darkRegion;
    // Start is called before the first frame update
    void Start()
    {
        ShowMenu();
    }
    void ShowMenu(bool hide = false) {
        float endWidth = 0f;
        float startWidth = 0f;

        float startAlpha = 0f;
        float endAlpha = 0f;
        if (hide) {
            startWidth = layoutElement.preferredHeight;
            endWidth = 0f;

            startAlpha = 0.5f;
            endAlpha = 0f;
        } else {
            startWidth = 0f;
            endWidth = Screen.width * menuPerWidth;

            startAlpha = 0f;
            endAlpha = 0.5f;
        }
        float duration = 0.2f;
        StartCoroutine(Lerp(startWidth, endWidth, duration, (lerpWidth, end) => {
            layoutElement.preferredWidth = lerpWidth;
            if (end && hide)
                Close();
        }));
        StartCoroutine(Lerp(startAlpha, endAlpha, duration, (lerpAlpha, end) => darkRegion.ChangeAlpha(lerpAlpha)));
    }
    IEnumerator Lerp(float startValue, float endValue, float duration, Action<float, bool> action) {
        float timeElapsed = 0;
        float valueToLerp = startValue;

        while (timeElapsed < duration) {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            action?.Invoke(valueToLerp, false);
            yield return null;
        }
        valueToLerp = endValue;
        action?.Invoke(valueToLerp, true);
    }
    public void HideMenu() => ShowMenu(true);
}
