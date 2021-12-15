using System;
using UnityEngine;
using TMPro;
using TrainicketJSONStorage.FoundRoutes;
using TrainicketJSONStorage.PopularWays;
using UnityEngine.UI;

public class SearchMenu : MenuItem<SearchMenu>
{
    public TargetSearch targetSearch;
    public Searcher searcher;
    [SerializeField] TextMeshProUGUI dateTxt;
    public TMP_InputField from;
    public TMP_InputField where;

    [SerializeField] Transform popularHolder;
    [SerializeField] PopularWay popularPrefab;
    [SerializeField] GameObject head;
    [SerializeField] GameObject end;

    [SerializeField] Button searchBtn;
    private void Start() {
        SetPopularWays();
    }
    private void Update() {
        UpdateSearchButton();
    }
    public void ShowSideMenu() {
        SideMenu.Show();
    }
    DateTime childDate;
    public DateTime ChildDate {
        get {
            return childDate;
        }
        set {
            childDate = value;
            dateTxt.text = childDate.Date.ToString("dd.MM.yyyy");
            i.targetSearch.date = childDate;
            firstLaunch = false;
        }
    }
    bool firstLaunch = true;
    public void ShowCalendar() => CalendarMenu.ShowCalendar((selectedDate) => ChildDate = selectedDate);
    void UpdateSearchButton() {
        if (string.IsNullOrEmpty(targetSearch.from) || string.IsNullOrEmpty(targetSearch.where) || firstLaunch) {
            searchBtn.interactable = false;
            return;
        }
        searchBtn.interactable = true;
    }
    public void Search() {
        var url = "http://18.117.102.247:5000/api/route/search?date=" + targetSearch.date.ToString("yyyy-MM-dd") + "&from=" + targetSearch.from + "&to=" + targetSearch.where;
        StartCoroutine(RestAPI.GET(url, GetSearchedWays, null));
    }
    void GetSearchedWays(string json, long responseCode) {
        Debug.Log(json);
        switch (responseCode) {
            case 200:
                RoutesListMenu.ShowFoundRoutes(JsonUtility.FromJson<FoundRoutes>(json));
                break;
            default:
                break;
        }
    }
    public void SwapStations() {
        var from = targetSearch.from;
        var fromName = targetSearch.fromName;
        var temp = targetSearch.where;
        var tempName = targetSearch.whereName;
        targetSearch.where = from;
        targetSearch.whereName = fromName;
        targetSearch.from = temp;
        targetSearch.fromName = tempName;

        where.text = targetSearch.whereName;
        this.from.text = targetSearch.fromName;
    }
    void SetPopularWays() {
        StartCoroutine(RestAPI.GET("http://18.117.102.247:5000/api/route/popular", (string json, long responseCode) => {
            var popularWays = JsonUtility.FromJson<PopularWays>(json);
            for (int i = 0; i < popularWays.stations.Length; i++) {
                var station = popularWays.stations[i];
                var popularWay = Instantiate(popularPrefab, popularHolder).GetComponent<PopularWay>();
                popularWay.fromWhereTxt.text = station.from.name + " - " + station.to.name;
                popularWay.targetSearch = new TargetSearch(DateTime.Today, station.from._id, station.to._id, station.from.name, station.to.name);
            }
            head.SetActive(true);
            end.SetActive(true);
            end.transform.SetSiblingIndex(popularHolder.childCount - 1);
        }, null, false));
    }
    public override void OnBackPressed() {
        
    }
}
[Serializable]
public struct TargetSearch
{
    public DateTime date;
    public string from;
    public string fromName;
    public string where;
    public string whereName;

    public TargetSearch(DateTime date, string fromID, string whereID, string fromName, string whereName) {
        this.date = date;
        this.from = fromID;
        this.where = whereID;
        this.fromName = fromName;
        this.whereName = whereName;
    }
}
