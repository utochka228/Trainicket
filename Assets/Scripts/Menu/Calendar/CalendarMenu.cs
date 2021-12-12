using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarMenu : MenuItem<CalendarMenu>
{
    Action<DateTime> OnSelectedDate;
    public static void ShowCalendar(Action<DateTime> OnSelectedDate) {
        Show();
        i.OnSelectedDate = OnSelectedDate;
    }
    public void ApplySelectedDate() {
        OnSelectedDate?.Invoke(Calendar.date);
        Close();
    }
}
