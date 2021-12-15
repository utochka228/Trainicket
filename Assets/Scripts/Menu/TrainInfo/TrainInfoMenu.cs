using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TrainicketJSONStorage.TrainDetailInfo;
using TrainicketJSONStorage.FoundRoutes;
using System;
using System.Linq;

public class TrainInfoMenu : MenuItem<TrainInfoMenu>
{
    public TrainDetailInfo trainDetailInfo { get; private set; }
    Route trainRoute;
    [SerializeField] TextMeshProUGUI wayName;
    [SerializeField] TextMeshProUGUI vanType;

    [SerializeField] TextMeshProUGUI dateFrom;
    [SerializeField] TextMeshProUGUI dateWhere;

    [SerializeField] TextMeshProUGUI from;
    [SerializeField] TextMeshProUGUI where;

    [SerializeField] TextMeshProUGUI price;

    [SerializeField] GameObject vanPrefab;
    [SerializeField] Transform vanHolder;

    [SerializeField] Tooltip toolTip;
    [SerializeField] Transform selectedSeatsHolder;
    [SerializeField] GameObject selectedSeat;

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Transform mainVanHolder;

    [SerializeField] VanHandler platscartPrefab;
    [SerializeField] VanHandler kupePrefab;
    [SerializeField] VanHandler chairFirstPrefab;
    [SerializeField] VanHandler chairSecondPrefab;

    [SerializeField] Button continueBtn;

    public static void ShowTrainDetailInfo(TrainDetailInfo detailInfo, Route trainRoute) {
        Show();
        i.trainRoute = trainRoute;
        i.trainDetailInfo = detailInfo;
        i.SetTrainInfoLables();
        i.SetVans();
        i.SetVanTypePrefab();
    }
    void SetTrainInfoLables() {
        wayName.text = trainDetailInfo.train.name;
        vanType.text = "Van type - " + trainDetailInfo.train.type;
        dateFrom.text = DateTime.Parse(trainRoute.departureTime).ToString("dd.MM.yyyy(ddd), hh:mm tt");
        dateWhere.text = DateTime.Parse(trainRoute.arrivalTime).ToString("dd.MM.yyyy(ddd), hh:mm tt");
        from.text = trainRoute.from.name;
        where.text = trainRoute.to.name;
    }
    void SetVans() {
        for (int i = 0; i < trainDetailInfo.train.vans.Length; i++) {
            var van = Instantiate(vanPrefab, vanHolder).gameObject;
            van.GetComponent<Button>().onClick.AddListener(()=>UpdateVanInfo(i));
            van.transform.Find("VanNumber").GetComponent<TextMeshProUGUI>().text = trainDetailInfo.train.vans[i].number.ToString();
            van.transform.Find("FreeSeats").GetComponent<TextMeshProUGUI>().text = trainDetailInfo.train.vans[i].seats.Where(x => x.occupied == false).Count().ToString();
        }
    }
    VanHandler currentVanHandler;
    void SetVanTypePrefab() {
        GameObject prefab = null;
        switch (trainDetailInfo.train.vans[0].vanClass) {
            case "first":
                prefab = kupePrefab.gameObject;
                break;
            case "second":
                prefab = platscartPrefab.gameObject;
                break;
            case "chairFirst":
                prefab = chairFirstPrefab.gameObject;
                break;
            case "chairSecond":
                prefab = chairSecondPrefab.gameObject;
                break;
        }
        currentVanHandler = Instantiate(prefab, mainVanHolder).GetComponent<VanHandler>();
        scrollRect.content = currentVanHandler.GetComponent<RectTransform>();
    }

    void UpdateVanInfo(int index) {
        currentVanHandler.UpdateVan(trainDetailInfo.train.vans[index]);
    }
}
