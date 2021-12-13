using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarMenu : MenuItem<CalendarMenu>
{
    Action<DateTime> OnSelectedDate;
    public bool showAllDays;
    public static void ShowCalendar(Action<DateTime> OnSelectedDate, bool showAllDays = false) {
        Show();
        i.showAllDays = showAllDays;
        i.OnSelectedDate = OnSelectedDate;
    }
    public void ApplySelectedDate() {
        OnSelectedDate?.Invoke(Calendar.date);
        Close();
    }
}
