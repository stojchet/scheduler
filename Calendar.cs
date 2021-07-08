using System;
using System.Collections.Generic;
using static System.Console;

[Serializable]
public class Calendar
{
    public List<Day> Days { get; set; }
    public DateTime CurrentDate { get; set; }
    public (int, int) DefaultWorkingHoursInterval { get; set; }
    public int DefaultWorkingHours { get; set; }

    public delegate void ErrorNotify();

    public Calendar()
    {
        this.Days = new List<Day>();
        this.CurrentDate = DateTime.Now; // format dd.mm.yyyy hh:mm:ss
    }

    public Calendar(List<Day> days)
    {
        this.Days = days;
        this.CurrentDate = DateTime.Now; // format dd.mm.yyyy hh:mm:ss
    }

    /* ----------------------------- Utilities ------------------------------ */

    private static int numberOfDaysInRange(DateTime startingDate, DateTime endDate) => (endDate.Date - startingDate.Date).Days + 1;

    private bool isDateValid(DateTime startingDate, DateTime endDate) => numberOfDaysInRange(startingDate, endDate) > 0;

    public static bool isDateValid(string date)
    {
        DateTime val;
        return (DateTime.TryParse(date, out val)) && (numberOfDaysInRange(DateTime.Now, val) > 0) ? true : false;
    }

    private bool shouldAddDay(Task task) => Days[Days.Count - 1].hoursToShift * -1 < task.Duration;

    public Day getDayByDate(DateTime date)
    {
        foreach (Day day in Days)
        {
            if (day.Date.Date == date.Date)
            {
                return day;
            }
        }
        return null;
    }

    public bool addDaysUpToDate(DateTime date, int duration)
    {
        int daysToAdd;
        if (Days.Count == 0)
        {
            daysToAdd = numberOfDaysInRange(date, DateTime.Now);
            Days.Add(new Day(DateTime.Now, new List<Task>(), DefaultWorkingHoursInterval, DefaultWorkingHours));
        }
        else
        {
            daysToAdd = numberOfDaysInRange(Days[Days.Count - 1].Date, date) - 1;
        }
        for(int i = 0; i < daysToAdd; ++i)
        {
            if(duration == 0)
            {
                break;
            }
            Day newDay = new Day(Days[Days.Count - 1].Date.AddDays(1), new List<Task>(), DefaultWorkingHoursInterval, DefaultWorkingHours);
            newDay.PrevDay = Days[Days.Count - 1];
            Days[Days.Count - 1].NextDay = newDay;
            Days.Add(newDay);
            duration = duration >= newDay.WorkingHours || duration < 0 ? (duration - newDay.WorkingHours) : 0;
        }
        return duration <= 0;
    }

    // TODO: Fix
    public List<Day> getRangeOfDaysForTask(Task task)
    {
        List<Day> validDays = new List<Day>();

        Day newDay;
        // calculate duration
        if (Days.Count == 0)
        {
            newDay = new Day(CurrentDate, new List<Task>(), DefaultWorkingHoursInterval, DefaultWorkingHours);
            Days.Add(newDay);
            validDays.Add(newDay);
        }

        if (shouldAddDay(task))
        {
            // TODO: Calculate correctly the task duration left
            Day day = Days[Days.Count - 1];
            int durationLeft = task.Duration;
            while (day.PrevDay != null && !day.isDayFull(0))
            {
                durationLeft += day.hoursToShift;
                day = day.PrevDay;
            }
            addDaysUpToDate(task.Deadline, durationLeft);
        }

        int numberOfValidDaysInRange = numberOfDaysInRange(CurrentDate, task.Deadline);

        // start from current Date
        int startIndex = numberOfDaysInRange(Days[0].Date, CurrentDate) - 1;
        for (int i = 0; i < numberOfValidDaysInRange; ++i)
        {
            // TODO: First Condition?
            if (Days.Count <= i || Days.Count - 1 < startIndex + i)
            {
                break;
            }
            validDays.Add(Days[startIndex + i]);
        }

        return validDays;
    }

    public event ErrorNotify ErrorInTaskParameters; 

    /* ------------------------------- Add Task --------------------------------- */

    public void addTask(Task task)
    {
        try
        {
            if(task.Type == Type.NORMAL)
            {
                _addTask(task);
            }
            else if(task.Type == Type.FIXED)
            {
                addFixedTask(task);
            }
        }
        catch (NoSpaceForTaskExeption exception)
        {
            Day day = getDayByDate(task.Deadline);
            day.removeTask(task);
            deleteReorderCalendar(exception.Day.NextDay, day.hoursToShift * -1, day);
            ErrorInTaskParameters.Invoke();
        }
    }

    public void addFixedTask(Task task)
    {
        if (Days.Count == 0)
        {
            Days.Add(new Day(CurrentDate, new List<Task>(), DefaultWorkingHoursInterval, DefaultWorkingHours));
        }

        Day day;
        if (numberOfDaysInRange(task.Deadline, Days[Days.Count - 1].Date) > 0){
            day = getDayByDate(task.Deadline);
            day.addTask(task, 0);
            reorderCalendar(day, task.Duration);
        }
        else
        {
            while(numberOfDaysInRange(task.Deadline, Days[Days.Count - 1].Date) <= 0)
            {
                Day newDay = new Day(Days[Days.Count - 1].Date.AddDays(1), new List<Task>(), DefaultWorkingHoursInterval, DefaultWorkingHours);
                Days[Days.Count - 1].NextDay = newDay;
                newDay.PrevDay = Days[Days.Count - 1];
                Days.Add(newDay);
            }
            day = getDayByDate(task.Deadline);
            day.addTask(task, 0);
        }
    }

    public void _addTask(Task task)
    {
        List<Day> validDays = getRangeOfDaysForTask(task);

        foreach (Day day in validDays)
        {
            for (int i = 0; i < day.Tasks.Count; ++i)
            {
                if (numberOfDaysInRange(day.Tasks[i].Deadline, task.Deadline) <= 0 && day.Tasks[i].Type != Type.FIXED)
                {
                    day.addTask(task, i);
                    if (day.isDayFull(0))
                    {
                        reorderCalendar(day, day.hoursToShift);
                    }
                    return;
                }
            }
            if (!day.isDayFull(0))
            {
                day.addTask(task, day.Tasks.Count);
                if (day.isDayFull(0))
                {
                    reorderCalendar(day, day.hoursToShift);
                }
                return;
            }
        }

    }

    private void reorderCalendar(Day day, int hours)
    {
        Day dirDay = day.NextDay;

        if (dirDay == null || day.hoursToShift <= 0) { return; }

        List<Task> tasks = new List<Task>();

        for (int i = day.Tasks.Count - 1; i >= 0; --i)
        {
            if (day.Tasks[i].Type != Type.FIXED)
            {
                if (hours == 0) { break; }

                Task curTask = day.Tasks[i];

                if (!isDateValid(dirDay.Date, curTask.Deadline))
                {
                    foreach (Task task in tasks) { day.addTask(task, day.Tasks.Count); }
                    throw new NoSpaceForTaskExeption("There is no space to add the Task, error occured during shifting the Tasks", day, curTask, hours);
                }

                if (curTask.Duration > hours)
                {
                    int additionalHours = 0;

                    if (curTask.IsSplit)
                    {
                        additionalHours = curTask.SplitTaskPtr.Duration;
                        dirDay.removeTask(curTask.SplitTaskPtr);
                        curTask.mergeTasks(curTask, curTask.SplitTaskPtr);
                    }

                    int[] splitHours = { curTask.Duration - hours - additionalHours, hours + additionalHours };
                    curTask.splitTask(splitHours, 0, curTask, day);

                    hours = 0;
                    tasks.Add(curTask.SplitTaskPtr);
                }
                else
                {
                    int curTaskHours = curTask.Duration;
                    if (curTask.IsSplit)
                    {
                        dirDay.removeTask(curTask.SplitTaskPtr);
                        curTask.mergeTasks(curTask, curTask.SplitTaskPtr);
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

        reorderCalendar(dirDay, dirDay.hoursToShift);
    }

    /* ------------------------------- Delete Task ----------------------------------- */

    private void deleteReorderCalendar(Day day, int hours, Day returnPoint)
    {
        int h = hours;
        Day dirDay =  day.PrevDay;

        if (dirDay == null || day.Equals(returnPoint)) { return; }

        List<Task> tasks = new List<Task>();
        for (int i = 0; i < day.Tasks.Count; ++i)
        {
            if (day.Tasks[i].Type != Type.FIXED)
            {
                if (hours == 0) { break; }

                Task curTask = day.Tasks[i];

                if (curTask.Duration > hours)
                {
                    Task shiftSplitTask = new Task(curTask.Name, curTask.Deadline, hours, curTask.Type, true);
                    shiftSplitTask.SplitTaskPtr = curTask;
                    curTask.Duration -= hours;

                    tasks.Add(shiftSplitTask);   
                    hours = 0;
                }
                else
                {
                    int curTaskHours = curTask.Duration;
                    tasks.Add(curTask);
                    hours -= curTaskHours;
                    day.removeTask(curTask);
                    i--;
                }
            }
        }

        if (dirDay.Tasks.Count != 0 && dirDay.Tasks[dirDay.Tasks.Count - 1].IsSplit && tasks.Count > 0)
        {
            dirDay.Tasks[dirDay.Tasks.Count - 1].IsSplit = tasks[0].IsSplit;
            dirDay.Tasks[dirDay.Tasks.Count - 1].SplitTaskPtr = tasks[0].SplitTaskPtr;
            dirDay.Tasks[dirDay.Tasks.Count - 1].Duration += tasks[0].Duration;
            tasks.RemoveAt(0);
        }

        for (int i = 0; i < tasks.Count; ++i) 
        { 
            dirDay.addTask(tasks[i], dirDay.Tasks.Count); 
        }

        deleteReorderCalendar(dirDay, h, returnPoint);
    }

    public void deleteTaskHelper(Day day, Task task)
    {
        int hours = 0;
        
        if (task.IsSplit)
        {
            hours += task.SplitTaskPtr.Duration;
            day.NextDay.removeTask(task.SplitTaskPtr);
            task.IsSplit = false;
            deleteReorderCalendar(Days[Days.Count - 1], task.SplitTaskPtr.Duration, day.NextDay);
        }

        while (day.PrevDay != null && day.PrevDay.Tasks.Count != 0 && day.PrevDay.Tasks[day.PrevDay.Tasks.Count - 1].IsSplit)
        {
            hours += day.PrevDay.Tasks[day.PrevDay.Tasks.Count - 1].Duration;
            deleteTaskHelper(day.PrevDay, day.PrevDay.Tasks[day.PrevDay.Tasks.Count - 1]);
        }

        hours += task.Duration;
        day.removeTask(task);
        deleteReorderCalendar(Days[Days.Count - 1], task.Duration, day);
        task.Duration = hours;
    }

    public void deleteTask(Day day, Task task)
    {
        while (task.IsSplit)
        {
            task = task.SplitTaskPtr;
            day = day.NextDay;
        }

        deleteTaskHelper(day, task);
    }

    /* --------------------------- Change Working Hours ------------------------------- */

    public void changeWorkingHours(DateTime date, int previousWorkingHours)
    {
        Day day = getDayByDate(date);
        if(previousWorkingHours - day.WorkingHours < 0)
        {
            deleteReorderCalendar(Days[Days.Count - 1], Math.Abs(previousWorkingHours - day.WorkingHours), day);
        }
        else if(previousWorkingHours - day.WorkingHours > 0)
        {
            reorderCalendar(day, previousWorkingHours - day.WorkingHours);
        }
    }
}