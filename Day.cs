using System;
using System.Collections.Generic;
using static System.Console;

[Serializable]
public class Day
{
    public DateTime date { get; set; }
    public List<Task> tasks { get; set; }
    // format xx:yy
    public (int, int) workingHoursInterval { get; set; }
    public int workingHours { get; set; }
    public Day nextDay { get; set; }
    public Day prevDay { get; set; }

    public Day(DateTime date, List<Task> tasks, (int, int) workingHoursInterval, int workingHours)
    {
        this.date = date;
        this.tasks = tasks;
        this.workingHoursInterval = workingHoursInterval;
        this.workingHours = workingHours;
        this.nextDay = null;
        this.prevDay = null;
    }

    public override bool Equals(Object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Day d = (Day)obj;
            return d.date == date;
        }
    }

    public override int GetHashCode() => date.GetHashCode();

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

    public void changeTask(Task task, Task changedTask) {
        for (int i = 0; i < tasks.Count; ++i) {
            if (tasks[i] == task) {
                tasks[i] = changedTask;
                return;
            }
        }
        WriteLine("Problem while changing the task - task not found in day");
    }

    public void removeTask(Task task)
    {
        tasks.Remove(task);
    }

    public int getTaskIndex(Task task) {
        for(int i = 0; i < tasks.Count; ++i) {
            if(tasks[i] == task) {
                return i;
            }
        }
        WriteLine("No such task exists");
        return -1;
    }
}