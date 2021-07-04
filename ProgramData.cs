using System.Collections.Generic;

[System.Serializable]
public class ProgramData
{
    public List<Day> Days { get; set; }
    public int defaultWorkingHours { get; set; }
    public (int, int) defaultWorkingHoursInterval { get; set; }

    public ProgramData(Calendar MyCalendar)
    {
        this.Days = MyCalendar.days;
        this.defaultWorkingHours = MyCalendar.defaultWorkingHours;
        this.defaultWorkingHoursInterval = MyCalendar.defaultWorkingHoursInterval;
    }

    public Calendar loadCalendar()
    {
        Calendar MyCalendar = new Calendar();
        MyCalendar.days = Days;
        MyCalendar.defaultWorkingHours = defaultWorkingHours;
        MyCalendar.defaultWorkingHoursInterval = defaultWorkingHoursInterval;

        return MyCalendar;
    }
}