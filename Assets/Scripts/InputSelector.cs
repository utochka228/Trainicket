using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputSelector : MenuItem<InputSelector>
{
    [SerializeField] TextMeshProUGUI fieldLable;
    [SerializeField] Transform holder;
    [SerializeField] Image background;
    [SerializeField] LocalizedText localizedText;
    [SerializeField] string defaultFieldValue;
    [SerializeField] CanvasGroup raycastBlocker;

    public bool removeOnPressingBack;
    void Start()
    {
        LerpBackAlpha(0f, 0.9f);
        LerpLableAlpha(0f, 1f);
        LerpAnchoredPos(target.anchoredPosition, Vector2.zero);
    }
    void LerpBackAlpha(float start, float end) {
        StartCoroutine(Utils.Lerp(start, end, 0.2f, (lerpValue, isEnd) => {
            background.ChangeAlpha(lerpValue);
        }));
    }
    void LerpLableAlpha(float start, float end) {
        StartCoroutine(Utils.Lerp(start, end, 0.2f, (lerpValue, isEnd) => {
            Color newColor = new Color(fieldLable.color.r, fieldLable.color.g, fieldLable.color.b, lerpValue);
            fieldLable.color = newColor;
        }));
    }
    void LerpAnchoredPos(Vector2 start, Vector2 end) {
        StartCoroutine(Utils.Lerp(start.x, end.x, 0.2f, (lerpedX, isEnd) => {
            target.anchoredPosition = new Vector2(lerpedX, target.anchoredPosition.y);
        }));
        StartCoroutine(Utils.Lerp(start.y, end.y, 0.2f, (lerpedY, isEnd) => {
            target.anchoredPosition = new Vector2(target.anchoredPosition.x, lerpedY);
        }));
    }
    bool xBackDone;
    bool yBackDone;
    void LerpAnchoredStartPos(Vector2 start, Vector2 end) {
        StartCoroutine(Utils.Lerp(start.x, end.x, 0.2f, (lerpedX, isEnd) => {
            target.anchoredPosition = new Vector2(lerpedX, target.anchoredPosition.y);
            if (isEnd)
                xBackDone = true;
        }));
        StartCoroutine(Utils.Lerp(start.y, end.y, 0.2f, (lerpedY, isEnd) => {
            target.anchoredPosition = new Vector2(target.anchoredPosition.x, lerpedY);
            if (isEnd)
                yBackDone = true;
        }));
    }
    IEnumerator EndFocus() {
        raycastBlocker.blocksRaycasts = true;
        LerpAnchoredStartPos(target.anchoredPosition, startPosInThisParent);
        yield return new WaitUntil(() => xBackDone && yBackDone);
        yield return StartCoroutine(Utils.Lerp(0.9f, 0f, 0.1f, (lerpValue, isEnd) => {
            background.ChangeAlpha(lerpValue);
        }));
        yield return StartCoroutine(Utils.Lerp(1f, 0f, 0.1f, (lerpValue, isEnd) => {
            Color newColor = new Color(fieldLable.color.r, fieldLable.color.g, fieldLable.color.b, lerpValue);
            fieldLable.color = newColor;
        }));
        target.transform.SetParent(previousParent);
        lableOrigin.SetActive(true);
        Close();
    }
    public override void OnBackPressed() {
        StartCoroutine(EndFocus());
    }
    public void OnBackgroundPressed() {
        if(removeOnPressingBack)
            StartCoroutine(EndFocus());
    }
    static Transform previousParent;
    static RectTransform target;
    static Vector2 startPosInThisParent;
    static GameObject lableOrigin;
    public static void ShowFocusZone(RectTransform targetInput, TextMeshProUGUI lable) {
        Show();
        target = targetInput;
        previousParent = targetInput.transform.parent;
        targetInput.transform.SetParent(i.holder);
        startPosInThisParent = target.anchoredPosition;
        if (lable != null) {
            lableOrigin = lable.gameObject;
            lableOrigin.SetActive(false);
            i.fieldLable.text = lable.text;
        }
        else
            i.fieldLable.text = i.defaultFieldValue;
    }
}
