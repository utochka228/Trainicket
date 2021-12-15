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

    [SerializeField] GameObject vanPrefab;
    public Transform vanHolder;

    [SerializeField] Tooltip toolTip;
    [SerializeField] Transform selectedSeatsHolder;
    [SerializeField] SelectedSeat selectedSeat;
    public List<UserSelectedSeat> selectedSeats = new List<UserSelectedSeat>();

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Transform mainVanHolder;

    [SerializeField] VanHandler platscartPrefab;
    [SerializeField] VanHandler kupePrefab;
    [SerializeField] VanHandler chairFirstPrefab;
    [SerializeField] VanHandler chairSecondPrefab;

    [SerializeField] Button continueBtn;

    private void Update() {
        if (selectedSeats.Count > 0)
            continueBtn.interactable = true;
        else
            continueBtn.interactable = false;
    }

    public static void ShowTrainDetailInfo(TrainDetailInfo detailInfo, Route trainRoute) {
        Show();
        i.trainRoute = trainRoute;
        i.trainDetailInfo = detailInfo;
        i.SetTrainInfoLables();
        i.SetVans();
        i.SetVanTypePrefab();
        i.UpdateVanInfo(i.trainDetailInfo.train.vans[0]);
        i.vanHolder.GetChild(0).GetComponent<Outline>().enabled = true;
    }
    void SetTrainInfoLables() {
        wayName.text = trainDetailInfo.train.name;
        vanType.text = "Van type - " + trainDetailInfo.train.type;
        dateFrom.text = DateTime.Parse(trainRoute.departureTime).ToString("dd.MM.yyyy(ddd), hh:mm tt");
        dateWhere.text = DateTime.Parse(trainRoute.arrivalTime).ToString("dd.MM.yyyy(ddd), hh:mm tt");
        from.text = trainRoute.from.name;
        where.text = trainRoute.to.name;
    }
    #region Van_setup

    void SetVans() {
        for (int i = 0; i < trainDetailInfo.train.vans.Length; i++) {
            var van = Instantiate(vanPrefab, vanHolder);
            var vanButton = van.GetComponent<VanButton>();
            vanButton.myVan = trainDetailInfo.train.vans[i];
            vanButton.number.text = trainDetailInfo.train.vans[i].number.ToString();
            vanButton.freeSeats.text = trainDetailInfo.train.vans[i].seats.Where(x => x.occupied == false).Count().ToString();
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
    public void UpdateVanInfo(Van van) {
        currentVanHandler.UpdateVan(van);
    }
    #endregion
    #region Seats_setup
    public void SeatPressed(Seat seat, bool selected, int number, bool isUp) {
        if (selected) {
            SpawnToSelectedList(seat, number, isUp);
        } else if(selectedSeats.Any(x => x.selectedSeat.VanNumber == seat.VanNumber)) {
            RemoveSelectedSeat(selectedSeats.Single(x => x.seat == seat && x.selectedSeat.VanNumber == seat.VanNumber).selectedSeat);
        }
    }
    void SpawnToSelectedList(Seat seat, int number, bool isUp) {
        var spawnedSeat = Instantiate(selectedSeat, selectedSeatsHolder);
        selectedSeats.Add(new UserSelectedSeat(seat, spawnedSeat));
        spawnedSeat.SeatNumber = number;
        spawnedSeat.VanNumber = seat.VanNumber;
        if (isUp)
            spawnedSeat.up.SetActive(true);
        else
            spawnedSeat.down.SetActive(true);
    }
    public void RemoveSelectedSeat(SelectedSeat selectedSeat) {
        var toRemove = selectedSeats.Single(x => x.selectedSeat == selectedSeat && x.selectedSeat.VanNumber == selectedSeat.VanNumber);
        toRemove.seat.ResetSeat();
        selectedSeats.Remove(toRemove);
        Destroy(selectedSeat.gameObject);
    }
    #endregion
    [Serializable]
    public struct UserSelectedSeat
    {
        public Seat seat;
        public SelectedSeat selectedSeat;
        public UserSelectedSeat(Seat seat, SelectedSeat selectedSeat) {
            this.seat = seat;
            this.selectedSeat = selectedSeat;
        }
    }
}
