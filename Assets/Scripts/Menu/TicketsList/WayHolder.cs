using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayHolder : MonoBehaviour
{
    [SerializeField] Button expandBtn;
    [SerializeField] GameObject collapseIcon;
    [SerializeField] GameObject expandIcon;
    RectTransform detailInfo;
    bool collapse = false;

    float maxHeight = 600f;
    // Start is called before the first frame update
    void Start()
    {
        //Calculate maxHeight!
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ExpandWay() {
        expandBtn.interactable = false;
        if (collapse == false) {
            collapseIcon.SetActive(true);
            expandIcon.SetActive(false);
            if(detailInfo == null) {
                CreateAndFill();
            }
            StartCoroutine(Lerp(0, maxHeight));
            collapse = true;
        } else {
            collapseIcon.SetActive(false);
            expandIcon.SetActive(true);
            StartCoroutine(Lerp(maxHeight, 0f, true));
            collapse = false;
        }
    }

    void CreateAndFill() {
        detailInfo = Instantiate(TicketsListMenu.i.detailInfoPrefab, TicketsListMenu.i.wayHolder).GetComponent<RectTransform>();
        detailInfo.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
        //Fill info
    }
    
    float lerpDuration = 0.2f;
    float valueToLerp;
    IEnumerator Lerp(float startValue, float endValue, bool destroyAfter = false) {
        float timeElapsed = 0;
        valueToLerp = startValue;

        while (timeElapsed < lerpDuration) {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            detailInfo.sizeDelta = new Vector2(detailInfo.rect.width, valueToLerp);
            yield return null;
        }
        valueToLerp = endValue;
        expandBtn.interactable = true;

        if (destroyAfter)
            Destroy(detailInfo.gameObject);
    }
}
