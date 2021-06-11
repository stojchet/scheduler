using System;
using System.Collections.Generic;
using static System.Console;

[Serializable]
public class Day
{
    public DateTime date { get; set; }
    public List<Task> tasks { get; set; }
    // format xx:yy
    public int workingHoursInterval { get; set; }
    public int workingHours { get; set; }
    public Day nextDay { get; set; }

    public Day(DateTime date, List<Task> tasks, int workingHoursInterval, int workingHours)
    {
        this.date = date;
        this.tasks = tasks;
        this.workingHoursInterval = workingHoursInterval;
        this.workingHours = workingHours;
        this.nextDay = null;
    }

    public int totalHoursTasks() {
        int totalHours = 0;
        foreach (Task task in tasks) {
            totalHours += task.duration;
        }
        return totalHours;
    }

    // hours - optional parameter when adding the task plus the day's totoal hours - will there be space?
    public bool isDayFull(int hours) => totalHoursTasks() + hours >= workingHours;

    // the negative value represents the number of free hours in the day
    public int hoursToShift => totalHoursTasks() - workingHours;

    // TODO: what happens if we try to insert at the last position 
    // i.e. we have 3 elems : 7, 1, 2 and we want to add a 6 after the 2
    public void addTask(Task task, int index)
    {
        tasks.Insert(index, task);
    }

    public void removeTask(Task task)
    {
        tasks.Remove(task);
    }
}