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
        StartCoroutine(Utils.Lerp(startWidth, endWidth, duration, (lerpWidth, end) => {
            layoutElement.preferredWidth = lerpWidth;
            if (end && hide)
                Close();
        }));
        StartCoroutine(Utils.Lerp(startAlpha, endAlpha, duration, (lerpAlpha, end) => darkRegion.ChangeAlpha(lerpAlpha)));
    }
    
    public void HideMenu() => ShowMenu(true);

    public void ShowTicketsMenu() {

    }
}
