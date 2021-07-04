using System;
using System.Collections.Generic;
using static System.Console;

[Serializable]
public class Day
{
    public DateTime date { get; set; }
    public List<Task> tasks { get; set; }
    public (int, int) workingHoursInterval { get; set; }
    public int workingHours { get; set; }
    public Day nextDay { get; set; }
    public Day prevDay { get; set; }

    public Task this[int i] => tasks[i];

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

    public string getTimeRangeForTask(Task task)
    {
        int hours = workingHoursInterval.Item1;
        foreach (Task t in tasks)
        {
            if (t.Equals(task))
            {
                return $"{hours} : {hours + task.duration}";
            }
            hours += t.duration;
        }
        return null;
    }

    public bool isDayFull(int hours) => totalHoursTasks() + hours >= workingHours;

    public int hoursToShift => totalHoursTasks() - workingHours;

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