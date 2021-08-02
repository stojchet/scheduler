using System;
using System.Collections.Generic;
using static System.Console;

[Serializable]
public class Day
{
    public DateTime Date { get; set; }
    public List<Task> Tasks { get; set; }
    public (int, int) WorkingHoursInterval { get; set; }
    public int WorkingHours => WorkingHoursInterval.Item2 - WorkingHoursInterval.Item1;
    public Day NextDay { get; set; }
    public Day PrevDay { get; set; }

    public Task this[int i] => Tasks[i];

    public Day(DateTime date, List<Task> tasks, (int, int) workingHoursInterval)
    {
        this.Date = date;
        this.Tasks = tasks;
        this.WorkingHoursInterval = workingHoursInterval;
        this.NextDay = null;
        this.PrevDay = null;
    }

    public override bool Equals(Object obj) => obj is Day d && d.Date == Date;

    public override int GetHashCode() => Date.GetHashCode();

    public int totalHoursTasks() {
        int totalHours = 0;
        foreach (Task task in Tasks) {
            totalHours += task.Duration;
        }
        return totalHours;
    }

    public string getTimeRangeForTask(Task task)
    {
        int hours = WorkingHoursInterval.Item1;
        foreach (Task t in Tasks)
        {
            if (t.Equals(task))
            {
                return $"{hours} : {hours + task.Duration}";
            }
            hours += t.Duration;
        }
        return null;
    }

    public bool isDayFull(int hours) => totalHoursTasks() + hours >= WorkingHours;

    public int hoursToShift => totalHoursTasks() - WorkingHours;

    public void addTask(Task task, int index)
    {
        Tasks.Insert(index, task);
    }

    public void removeTask(Task task)
    {
        Tasks.Remove(task);
    }
}