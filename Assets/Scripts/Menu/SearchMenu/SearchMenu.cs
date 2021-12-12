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
    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }
    public void ShowCalendar() => CalendarMenu.ShowCalendar((selectedDate) => ChildDate = selectedDate);

    public void Search() {

    }
}
[Serializable]
public struct TargetSearch
{
    public DateTime date;
    public string from;
    public string where;

    public TargetSearch(DateTime date, string from, string where) {
        this.date = date;
        this.from = from;
        this.where = where;
    }
}
