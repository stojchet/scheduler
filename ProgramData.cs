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
        return new Calendar(Days, DefaultWorkingHoursInterval, DefaultWorkingHours);
    }
}