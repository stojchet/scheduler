using System;
using System.Collections.Generic;
using static System.Console;

[Serializable]
public class Calendar
{
    public List<Day> days { get; set; }
    public DateTime currentDate { get; set; }
    public (int, int) defaultWorkingHoursInterval { get; set; }
    public int defaultWorkingHours { get; set; }

    public delegate void ErrorNotify();

    public Calendar()
    {
        this.days = new List<Day>();
        this.currentDate = DateTime.Now; // format dd.mm.yyyy hh:mm:ss
    }

    public Calendar(List<Day> days)
    {
        this.days = days;
        this.currentDate = DateTime.Now; // format dd.mm.yyyy hh:mm:ss
    }

    /* ----------------------------- Utilities ------------------------------ */

    private static int numberOfDaysInRange(DateTime startingDate, DateTime endDate) => (endDate.Date - startingDate.Date).Days + 1;

    private bool isDateValid(DateTime startingDate, DateTime endDate) => numberOfDaysInRange(startingDate, endDate) > 0;

    private bool shouldAddDay(Task task) => days[days.Count - 1].hoursToShift * -1 < task.duration;

    public Day getDayByDate(DateTime date)
    {
        foreach(Day day in days)
        {
            if(day.date.Date == date.Date)
            {
                return day;
            }
        }
        return null;
    }

    public Day findTaskInDay(Task task)
    {
        foreach(Day day in days)
        {
            foreach(Task t in day.tasks)
            {
                if (t.Equals(task))
                {
                    return day;
                }
            }
        }
        return null;
    }

    public static bool isDateValid(string date)
    {
        DateTime val;
        if (DateTime.TryParse(date, out val))
        {
            if (numberOfDaysInRange(DateTime.Now, val) > 0)
            {
                return true;
            }
        }
        return false;
    }

    public List<Day> getRangeOfDaysForTask(Task task)
    {
        List<Day> validDays = new List<Day>();

        Day newDay;
        if (days.Count == 0)
        {
            newDay = new Day(currentDate, new List<Task>(), defaultWorkingHoursInterval, defaultWorkingHours);
            days.Add(newDay);
            validDays.Add(newDay);
            return validDays;
        }

        if (shouldAddDay(task))
        {
            newDay = new Day(days[days.Count - 1].date.AddDays(1), new List<Task>(), defaultWorkingHoursInterval, defaultWorkingHours);
            newDay.prevDay = days[days.Count - 1];
            days[days.Count - 1].nextDay = newDay;
            days.Add(newDay);
        }

        int numberOfValidDaysInRange = numberOfDaysInRange(currentDate, task.deadline);

        // start from current date
        int startIndex = numberOfDaysInRange(days[0].date, currentDate) - 1;
        for (int i = 0; i < numberOfValidDaysInRange; ++i)
        {
            if (days.Count <= i)
            {
                break;
            }
            validDays.Add(days[startIndex + i]);
        }

        return validDays;
    }

    public event ErrorNotify ErrorInTaskParameters; 

    /* ---------------------------------- Exception Handling ----------------------------------- */

    public void errorCheck(Task task, List<Day> validDays, ref Day d)
    {
        for (int i = 0; i < validDays.Count; ++i)
        {
            if (!validDays[i].isDayFull(0))
            {
                break;
            }
            d = validDays[i];
        }

        int hours = 0;
        Day cur = d;
        while (cur != null && validDays.Contains(cur))
        {
            hours += cur.hoursToShift * -1;
            cur = cur.nextDay;
        }

        if (hours < task.duration)
        {
            validDays[validDays.Count - 1].addTask(task, validDays[validDays.Count - 1].tasks.Count);
            throw new NoSpaceForTaskExeption("There is no space to add the task which needs to be added as the last task in the calendar", d, task, hours - task.duration);
        }
    }

    /* ------------------------------- Add Task --------------------------------- */

    public void addTask(Task task)
    {
        try
        {
            if(task.type == Type.NORMAL)
            {
                _addTask(task);
            }
            else if(task.type == Type.FIXED)
            {
                addFixedTask(task);
            }
        }
        catch (NoSpaceForTaskExeption exception)
        {
            Day day = getDayByDate(task.deadline);
            day.removeTask(task);
            deleteReorderCalendar(exception.day.nextDay, day.hoursToShift * -1, day);
            ErrorInTaskParameters.Invoke();
        }
    }

    public void addFixedTask(Task task)
    {
        if (days.Count == 0)
        {
            days.Add(new Day(currentDate, new List<Task>(), defaultWorkingHoursInterval, defaultWorkingHours));
        }

        Day day;
        if (numberOfDaysInRange(task.deadline, days[days.Count - 1].date) > 0){
            day = getDayByDate(task.deadline);
            day.addTask(task, 0);
            reorderCalendar(day, task.duration, true, null);
        }
        else
        {
            while(numberOfDaysInRange(task.deadline, days[days.Count - 1].date) <= 0)
            {
                Day newDay = new Day(days[days.Count - 1].date.AddDays(1), new List<Task>(), defaultWorkingHoursInterval, defaultWorkingHours);
                days[days.Count - 1].nextDay = newDay;
                newDay.prevDay = days[days.Count - 1];
                days.Add(newDay);
            }
            day = getDayByDate(task.deadline);
            day.addTask(task, 0);
        }
    }

    public void _addTask(Task task)
    {
        List<Day> validDays = getRangeOfDaysForTask(task);

        foreach (Day day in validDays)
        {
            for (int i = 0; i < day.tasks.Count; ++i)
            {
                if (numberOfDaysInRange(day.tasks[i].deadline, task.deadline) <= 0 && day.tasks[i].type != Type.FIXED)
                {
                    day.addTask(task, i);
                    if (day.isDayFull(0))
                    {
                        reorderCalendar(day, day.hoursToShift, true, null);
                    }
                    return;
                }
            }
        }

        Day d = validDays[0];

        errorCheck(task, validDays, ref d);

        d.addTask(task, d.tasks.Count);
        if (d.isDayFull(0))
        {
            reorderCalendar(d, d.hoursToShift, true, null);
        }
    }

    private void reorderCalendar(Day day, int hours, bool next, Day returnPoint)
    {
        Day dirDay = day.nextDay;

        if (dirDay == null || day.hoursToShift <= 0) { return; }

        List<Task> tasks = new List<Task>();

        for (int i = day.tasks.Count - 1; i >= 0; --i)
        {
            if (day.tasks[i].type != Type.FIXED)
            {
                if (hours == 0) { break; }

                Task curTask = day.tasks[i];

                if (!isDateValid(dirDay.date, curTask.deadline))
                {
                    foreach (Task task in tasks) { day.addTask(task, day.tasks.Count); }
                    throw new NoSpaceForTaskExeption("There is no space to add the task, error occured during shifting the tasks", day, curTask, hours);
                }

                if (curTask.duration > hours)
                {
                    int additionalHours = 0;

                    if (curTask.isSplit)
                    {
                        additionalHours = curTask.splitTaskPtr.duration;
                        dirDay.removeTask(curTask.splitTaskPtr);
                        curTask.mergeTasks(curTask, curTask.splitTaskPtr);
                    }

                    int[] splitHours = { curTask.duration - hours - additionalHours, hours + additionalHours };
                    curTask.splitTask(splitHours, 0, curTask, day);

                    hours = 0;
                    tasks.Add(curTask.splitTaskPtr);
                }
                else
                {
                    int curTaskHours = curTask.duration;
                    if (curTask.isSplit)
                    {
                        dirDay.removeTask(curTask.splitTaskPtr);
                        curTask.mergeTasks(curTask, curTask.splitTaskPtr);
                    }
                    else
                    {
                        tasks.Add(curTask);
                    }
                    hours -= curTaskHours;
                    day.removeTask(curTask);

                }
            }
        }

        foreach (Task task in tasks) { dirDay.addTask(task, 0); }

        reorderCalendar(dirDay, dirDay.hoursToShift, next, null);
    }

    /* ------------------------------- Delete Task ----------------------------------- */

    private void deleteReorderCalendar(Day day, int hours, Day returnPoint)
    {
        int h = hours;
        Day dirDay =  day.prevDay;

        if (dirDay == null || day.Equals(returnPoint)) { return; }

        List<Task> tasks = new List<Task>();
        for (int i = 0; i < day.tasks.Count; ++i)
        {
            if (day.tasks[i].type != Type.FIXED)
            {
                if (hours == 0) { break; }

                Task curTask = day.tasks[i];

                if (curTask.duration > hours)
                {
                    Task shiftSplitTask = new Task(curTask.name, curTask.deadline, hours, curTask.type, true);
                    shiftSplitTask.splitTaskPtr = curTask;
                    curTask.duration -= hours;

                    tasks.Add(shiftSplitTask);   
                    hours = 0;
                }
                else
                {
                    int curTaskHours = curTask.duration;
                    tasks.Add(curTask);
                    hours -= curTaskHours;
                    day.removeTask(curTask);
                    i--;
                }
            }
        }

        if (dirDay.tasks.Count != 0 && dirDay.tasks[dirDay.tasks.Count - 1].isSplit)
        {
            dirDay.tasks[dirDay.tasks.Count - 1].isSplit = tasks[0].isSplit;
            dirDay.tasks[dirDay.tasks.Count - 1].splitTaskPtr = tasks[0].splitTaskPtr;
            dirDay.tasks[dirDay.tasks.Count - 1].duration += tasks[0].duration;
            tasks.RemoveAt(0);
        }

        for (int i = 0; i < tasks.Count; ++i) 
        { 
            dirDay.addTask(tasks[i], dirDay.tasks.Count); 
        }

        deleteReorderCalendar(dirDay, h, returnPoint);
    }

    public void deleteTask(Day day, Task task)
    {
        int hours = 0;
        if (task.isSplit)
        {
            hours += task.splitTaskPtr.duration;
            day.nextDay.removeTask(task.splitTaskPtr);
            task.isSplit = false;
            deleteReorderCalendar(days[days.Count - 1], task.splitTaskPtr.duration, day.nextDay);
        }

        if (day.prevDay != null && day.prevDay.tasks.Count != 0 && day.prevDay.tasks[day.prevDay.tasks.Count - 1].isSplit)
        {
            hours += day.prevDay.tasks[day.prevDay.tasks.Count - 1].duration;
            deleteTask(day.prevDay, day.prevDay.tasks[day.prevDay.tasks.Count - 1]);
        }

        hours += task.duration;
        day.removeTask(task);
        deleteReorderCalendar(days[days.Count - 1], task.duration, day);
        task.duration = hours;
    }
}