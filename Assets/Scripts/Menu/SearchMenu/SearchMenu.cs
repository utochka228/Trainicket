using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TrainicketJSONStorage.FoundRoutes;
using Newtonsoft.Json;

public class SearchMenu : MenuItem<SearchMenu>
{
    public static TargetSearch targetSearch;
    public Searcher searcher;
    [SerializeField] TextMeshProUGUI dateTxt;
    [SerializeField] TMP_InputField from;
    [SerializeField] TMP_InputField where;
    
    // Update is called once per frame
    void Update()
    {
        
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
            targetSearch.date = childDate;
        }
    }
    public void ShowCalendar() => CalendarMenu.ShowCalendar((selectedDate) => ChildDate = selectedDate);

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
