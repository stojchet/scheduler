using System;
using System.Collections.Generic;

[System.Serializable]
public class ProgramData
{
    public Dictionary<DateTime, Day> Days { get; set; }
    public (int, int) DefaultWorkingHoursInterval { get; set; }
    public DateTime LastAddedDay;

    public ProgramData(Calendar MyCalendar)
    {
        this.Days = MyCalendar.Days;
        this.DefaultWorkingHoursInterval = MyCalendar.DefaultWorkingHoursInterval;
        this.LastAddedDay = MyCalendar.LastAddedDay;
    }

    public Calendar loadCalendar() => new Calendar(Days, DefaultWorkingHoursInterval, LastAddedDay);
}