using System.Collections.Generic;

[System.Serializable]
public class ProgramData
{
    public List<Day> Days { get; set; }
    public int DefaultWorkingHours { get; set; }
    public (int, int) DefaultWorkingHoursInterval { get; set; }

    public ProgramData(Calendar MyCalendar)
    {
        this.Days = MyCalendar.Days;
        this.DefaultWorkingHours = MyCalendar.DefaultWorkingHours;
        this.DefaultWorkingHoursInterval = MyCalendar.DefaultWorkingHoursInterval;
    }

    public Calendar loadCalendar()
    {
        Calendar MyCalendar = new Calendar();
        MyCalendar.Days = Days;
        MyCalendar.DefaultWorkingHours = DefaultWorkingHours;
        MyCalendar.DefaultWorkingHoursInterval = DefaultWorkingHoursInterval;

        return MyCalendar;
    }
}