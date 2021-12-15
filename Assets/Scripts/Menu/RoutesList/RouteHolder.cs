using System.Collections;
using System.Collections.Generic;
using TrainicketJSONStorage.FoundRoutes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static VanTypeButton;

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

    [SerializeField] VanTypeButton vanClassBtn;
    RectTransform detailInfo;
    bool collapse = false;

    float maxHeight = 0f;

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
        //Calculate maxHeight!
        float oneButtonHeight = vanClassBtn.GetComponent<RectTransform>().sizeDelta.y;
        float spaceBetween = detailInfo.GetComponent<VerticalLayoutGroup>().spacing;
        maxHeight = 100f;
        for (int i = 0; i < myRoute.train.seats.Length; i++) {
            int thisClassSeats = myRoute.train.seats[i];
            if (thisClassSeats == 0)
                continue;
            maxHeight += oneButtonHeight + spaceBetween;
            AddVansClass(i, "free/seats:" + myRoute.train.freeSeats[i] + "/" + myRoute.train.seats[i]);
        }
    }
    void AddVansClass(int vanClass, string seatsInfo) {
        var vanType = Instantiate(vanClassBtn, detailInfo.transform).GetComponent<VanTypeButton>();
        VanTypeData vanTypeData;
        vanTypeData.typeName = "none";
        vanTypeData.seatsInfo = seatsInfo;
        vanTypeData.trainId = myRoute.train._id;
        switch (vanClass) {
            case 0:
                vanTypeData.typeName = "first";
                break;
            case 1:
                vanTypeData.typeName = "second";
                break;
            case 2:
                vanTypeData.typeName = "chairFirst";
                break;
            case 3:
                vanTypeData.typeName = "chairSecond";
                break;
        }
        vanType.SetInfo(vanTypeData, myRoute);
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
