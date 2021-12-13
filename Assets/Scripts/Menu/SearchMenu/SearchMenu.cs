using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SearchMenu : MenuItem<SearchMenu>
{
    public static TargetSearch targetSearch;
    public Searcher searcher;
    [SerializeField] TextMeshProUGUI dateTxt;
    [SerializeField] TMP_InputField from;
    [SerializeField] TMP_InputField where;
    // Start is called before the first frame update
    void Start()
    {
        ChildDate = DateTime.Today.Date;
        dateTxt.text = DateTime.Today.Date.ToString("dd.MM.yyyy");
    }

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
        TicketsListMenu.Show();
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
