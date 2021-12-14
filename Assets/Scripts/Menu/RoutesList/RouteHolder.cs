using System.Collections;
using System.Collections.Generic;
using TrainicketJSONStorage.FoundRoutes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RouteHolder : MonoBehaviour
{
    public Route myRoute;

    [SerializeField] TextMeshProUGUI fromLable;
    [SerializeField] TextMeshProUGUI whereLable;
    [SerializeField] TextMeshProUGUI dateFrom;
    [SerializeField] TextMeshProUGUI timeFrom;
    [SerializeField] TextMeshProUGUI dateWhere;
    [SerializeField] TextMeshProUGUI timeWhere;
    [SerializeField] TextMeshProUGUI totalTime;

    [SerializeField] Button expandBtn;
    [SerializeField] GameObject collapseIcon;
    [SerializeField] GameObject expandIcon;
    RectTransform detailInfo;
    bool collapse = false;

    float maxHeight = 600f;

    public void InitializeRoute(Route route) {
        myRoute = route;

        fromLable.text = myRoute.from.name;
        whereLable.text = myRoute.to.name;
        var dateTime = DateTime.Parse(myRoute.departureTime.ToString());
        dateFrom.text = DateTime.Parse(myRoute.departureTime).ToString("dddd") + " " + DateTime.Parse(myRoute.departureTime).ToString("dd.MM.yyyy");
        dateWhere.text = DateTime.Parse(myRoute.arrivalTime).ToString("dddd") + " " + DateTime.Parse(myRoute.arrivalTime).ToString("dd.MM.yyyy");

        timeFrom.text = DateTime.Parse(myRoute.departureTime).ToString("hh:mm tt");
        timeWhere.text = DateTime.Parse(myRoute.arrivalTime).ToString("hh:mm tt");

        totalTime.text = myRoute.time;
    }

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
        detailInfo = Instantiate(RoutesListMenu.i.detailInfoPrefab, RoutesListMenu.i.wayHolder).GetComponent<RectTransform>();
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
