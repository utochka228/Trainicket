using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchMenu : MenuItem<SearchMenu>
{
    public static TargetSearch targetSearch;
    public Searcher searcher;
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
    public void ShowCalendar() => CalendarMenu.Show();

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
