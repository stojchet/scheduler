using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Calendar
{
    public Dictionary<DateTime, Day> Days { get; set; }
    public DateTime CurrentDate { get; set; }
    public DateTime LastAddedDay { get; set; }
    public (int, int) DefaultWorkingHoursInterval { get; set; }
    public int DefaultWorkingHours => DefaultWorkingHoursInterval.Item2 - DefaultWorkingHoursInterval.Item1;

    public delegate void ErrorNotify();

    public Calendar()
    {
        this.Days = new Dictionary<DateTime, Day>();
        this.DefaultWorkingHoursInterval = (9, 17);
        Days[DateTime.Today] = new Day(DateTime.Now.Date, new List<Task>(), DefaultWorkingHoursInterval);
        this.CurrentDate = DateTime.Today; // format dd.mm.yyyy hh:mm:ss
        this.LastAddedDay = DateTime.Today;
    }

    public Calendar(Dictionary<DateTime, Day> days, (int, int) workingHoursInterval, DateTime lastAddedDay)
    {
        this.Days = days;
        this.DefaultWorkingHoursInterval = workingHoursInterval;
        this.LastAddedDay = lastAddedDay;
        this.CurrentDate = DateTime.Today; // format dd.mm.yyyy hh:mm:ss
        addDaysUpToDate(DateTime.Today);
    }

    private static int numberOfDaysInRange(DateTime startingDate, DateTime endDate) 
        => (endDate.Date - startingDate.Date).Days + 1;

    public Day getDayByDate(DateTime date) => Days.ContainsKey(date) ? Days[date] : null;

    private int shouldAddDay(int hours)
    {
        Day day = Days[LastAddedDay];
        while (day != null && !day.isDayFull(0) && DateTime.Today <= day.Date)
        {
            hours += day.hoursToShift;
            day = Days.ContainsKey(day.Date.AddDays(1).Date) ? Days[day.Date.AddDays(1).Date] : null;
        }
        return hours;
    }

    public void addDaysUpToDate(DateTime date)
    {
        int daysToAdd = numberOfDaysInRange(Days[LastAddedDay].Date, date);
        for (int i = 0; i < daysToAdd; ++i)
        {
            Day newDay = new Day(Days[LastAddedDay].Date.AddDays(1).Date, new List<Task>(), DefaultWorkingHoursInterval);
            Days[LastAddedDay.AddDays(1).Date] = newDay;
            LastAddedDay = newDay.Date;
        }
    }

    public List<Day> getRangeOfDaysForTask(Task task)
    {
        List<Day> validDays = new List<Day>();

        int shouldAddDayHours = shouldAddDay(task.Duration);
        if (shouldAddDayHours > 0)
        {
            addDaysUpToDate(Days[LastAddedDay].Date.AddDays((int)Math.Ceiling((double)shouldAddDayHours /DefaultWorkingHours)));
        }

        int numberOfValidDaysInRange = numberOfDaysInRange(CurrentDate, task.Deadline);

        for (int i = 0; i < numberOfValidDaysInRange; ++i)
        {
            if (LastAddedDay < DateTime.Now.AddDays(i).Date)
            {
                break;
            }

            validDays.Add(Days[DateTime.Now.AddDays(i).Date]);
        }

        return validDays;
    }

    public bool doesTaskExist(Task t)
    {
        Day day = Days[DateTime.Today];
        while(day != null)
        {
            foreach(Task task in day.Tasks)
            {
                if (t.Equals(task))
                {
                    return true;
                }
            }

            day = Days.ContainsKey(day.Date.AddDays(1).Date) ? Days[day.Date.AddDays(1).Date] : null;
        }
        return false;
    }

    public event ErrorNotify ErrorInTaskParameters;

    /* ----------------------------- Utility for add and delete task ----------------------------- */

    public bool checkForCondition(int i, int end, Direction dir) => dir == Direction.NEXT ? i >= end : i < end;

    public void reorderCalendar(Day day, int hoursToShift, Day returnPoint, Direction dir)
    {
        int hours = hoursToShift;
        Day dirDay = dir == Direction.NEXT ? 
            (Days.ContainsKey(day.Date.AddDays(1).Date)) ? Days[day.Date.AddDays(1).Date] : null
            : (Days.ContainsKey(day.Date.AddDays(-1).Date)) ? Days[day.Date.AddDays(-1).Date] : null;        

        if (dirDay == null || (dirDay.Equals(returnPoint) && dir == Direction.PREVIOUS) 
            || (day.hoursToShift <= 0 && dir == Direction.NEXT)) { return; }
        
        if (DateTime.Today > dirDay.Date) { return; }

        List<Task> tasks = new List<Task>();

        for (int i = dir == Direction.NEXT ? day.Tasks.Count - 1 : 0; 
            checkForCondition(i, (dir == Direction.NEXT ? 0 : day.Tasks.Count), dir); 
            i += (int)dir)
        {
            if (day.Tasks[i].Type != Type.FIXED)
            {
                if (hours == 0) { break; }

                Task curTask = day.Tasks[i];
                Task curTaskDir = curTask.getTaskByDirection(dir);

                if (dirDay.Date > curTask.Deadline)
                {
                    for (int q = tasks.Count - 1; q >= 0; --q) {
                        day.addTask(tasks[q], day.Tasks.Count); 
                    }
                    throw new NoSpaceForTaskException("There is no space to add the Task, " +
                        "error occured during shifting the Tasks", day, curTask, hours);
                }

                if (curTask.Duration > hours)
                {
                    int additionalHours = 0;

                    if (curTaskDir != null)
                    {
                        additionalHours = curTaskDir.Duration;
                        dirDay.removeTask(curTaskDir);
                        curTask.mergeTasks(curTask, curTaskDir, dir);
                    }

                    int[] splitHours = { curTask.Duration - hours - additionalHours, hours + additionalHours };
                    curTask.splitTask(splitHours, 0, curTask, day, dir);
                    hours = 0;
                    tasks.Add(curTask.getTaskByDirection(dir));
                }
                else
                {
                    int curTaskHours = curTask.Duration;
                    if (curTaskDir != null)
                    {
                        dirDay.removeTask(curTaskDir);
                        if (dir == Direction.NEXT)
                        {
                            curTask.mergeTasks(curTask, curTaskDir, dir);
                            tasks.Add(curTask);
                        }
                        else
                        {
                            curTaskDir.mergeTasks(curTask, curTaskDir, dir);
                            tasks.Add(curTask);
                        }
                    }
                    else
                    {
                        tasks.Add(curTask);
                    }
                    hours -= curTaskHours;
                    day.removeTask(curTask);

                    if (dir == Direction.PREVIOUS) { i--; }
                }
            }
        }

        foreach (Task task in tasks) { dirDay.addTask(task, dir == Direction.NEXT ? 0 : dirDay.Tasks.Count); }

        reorderCalendar(dirDay, dir == Direction.NEXT ? dirDay.hoursToShift < 0 ? hoursToShift 
            : dirDay.hoursToShift : hoursToShift, returnPoint, dir);
    }

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
                _addFixedTask(task);
            }
        }
        catch (NoSpaceForTaskException exception)
        {
            if (task.Type == Type.FIXED)
            {
                exception.Day.removeTask(task);
            }
            else if (task.Type == Type.NORMAL && exception.Day != null)
            {
                Day day = exception.Day;
                deleteTask(day, exception.Task);

                if (day.hoursToShift > 0)
                {
                    reorderCalendar(day, day.hoursToShift, null, Direction.NEXT);
                }
            }
            ErrorInTaskParameters?.Invoke();
        }
    }

    public void _addFixedTask(Task task)
    {
        int shouldAddDayHours = shouldAddDay(task.Duration);
        if (shouldAddDayHours > 0)
        {
            addDaysUpToDate(Days[LastAddedDay].Date.AddDays((int)Math.Ceiling((double)shouldAddDayHours / DefaultWorkingHours)));
        }
        if(task.Deadline > Days[LastAddedDay].Date)
        {
            addDaysUpToDate(task.Deadline);
        }

        Day day = getDayByDate(task.Deadline);
        day.addTask(task, 0);
        if (day.isDayFull(0))
        {
            reorderCalendar(day, day.hoursToShift, null, Direction.NEXT);
        }
    }

    public void _addTask(Task task)
    {
        List<Day> validDays;
        validDays = getRangeOfDaysForTask(task);

        foreach (Day day in validDays)
        {
            for (int i = 0; i < day.Tasks.Count; ++i)
            {
                if (day.Tasks[i].Deadline > task.Deadline && day.Tasks[i].Type != Type.FIXED)
                {
                    day.addTask(task, i);
                    if (day.isDayFull(0))
                    {
                        reorderCalendar(day, day.hoursToShift, null, Direction.NEXT);
                    }
                    return;
                }
            }
            if (!day.isDayFull(0))
            {
                day.addTask(task, day.Tasks.Count);
                if (day.isDayFull(0))
                {
                    reorderCalendar(day, day.hoursToShift, null, Direction.NEXT);
                }
                return;
            }
        }
        throw new NoSpaceForTaskException("There is no space to add the Task", null, null, 0);
    }

    /* ------------------------------- Delete Task ----------------------------------- */
    public void deleteTask(Day day, Task task)
    {
        while (task.NextSplitTaskPtr != null) 
        {
            task = task.NextSplitTaskPtr;
            day = Days[day.Date.AddDays(1).Date];
        }

        while (task != null)
        {
            day.removeTask(task);
            task.NextSplitTaskPtr = null;

            reorderCalendar(Days[LastAddedDay], task.Duration, 
                Days.ContainsKey(day.Date.AddDays(-1).Date) ? Days[day.Date.AddDays(-1).Date] : null,
                Direction.PREVIOUS);

            task = task.PrevSplitTaskPtr;
            day = Days.ContainsKey(day.Date.AddDays(-1).Date) ? Days[day.Date.AddDays(-1).Date] : null;
        }
    }

    /* --------------------------- Change Working Hours ------------------------------- */
    public bool changeWorkingHours(DateTime date, (int, int) previousWorkingHoursInterval)
    {
        Day day = getDayByDate(date);
        int previousWorkingHours = previousWorkingHoursInterval.Item2 - previousWorkingHoursInterval.Item1;
        if(previousWorkingHours - day.WorkingHours < 0)
        {
            reorderCalendar(Days[LastAddedDay], Math.Abs(previousWorkingHours - day.WorkingHours), 
                day, Direction.PREVIOUS);
        }
        else if(previousWorkingHours - day.WorkingHours > 0)
        {
            int hoursNeeded = shouldAddDay(day.hoursToShift);
            if (hoursNeeded > 0)
            {
                int daysNeeded = (int)Math.Ceiling((double)hoursNeeded / day.WorkingHours);
                addDaysUpToDate(Days[LastAddedDay].Date.AddDays(daysNeeded));
            }

            int shiftHours = day.hoursToShift;
            try
            {
                reorderCalendar(day, shiftHours, null, Direction.NEXT);
            }
            catch(NoSpaceForTaskException exception)
            {
                int tryWorkingHours = day.WorkingHours;
                day.WorkingHoursInterval = previousWorkingHoursInterval;

                if (exception.Day != null)
                {
                    if (exception.Day.hoursToShift - (previousWorkingHours - tryWorkingHours) > 0)
                    {
                        reorderCalendar(exception.Day, 
                            exception.Day.hoursToShift - (previousWorkingHours - tryWorkingHours), 
                            null, Direction.NEXT);
                    }
                    reorderCalendar(exception.Day, shiftHours, 
                        Days.ContainsKey(day.Date.AddDays(-1).Date) ? Days[day.Date.AddDays(-1).Date] : null, 
                        Direction.PREVIOUS);
                }
                
                ErrorInTaskParameters?.Invoke();
                return false;
            }
        }
        return true;
    }

    public bool changeDefaultWorkingHours((int, int) workingHoursInterval)
    {
        this.DefaultWorkingHoursInterval = workingHoursInterval;

        Day day = getDayByDate(DateTime.Today);

        while (day != null)
        {
            (int, int) previousWorkingHoursInterval = day.WorkingHoursInterval;
            day.WorkingHoursInterval = this.DefaultWorkingHoursInterval;

            if(!changeWorkingHours(day.Date, previousWorkingHoursInterval))
            {
                while(day != null && DateTime.Today <= day.Date)
                {
                    day.WorkingHoursInterval = previousWorkingHoursInterval;
                    reorderCalendar(Days[LastAddedDay], day.hoursToShift * -1, 
                        Days.ContainsKey(day.Date.AddDays(-1).Date) ? Days[day.Date.AddDays(-1).Date] : null, Direction.PREVIOUS);
                    day = Days.ContainsKey(day.Date.AddDays(-1).Date) ? Days[day.Date.AddDays(-1).Date] : null;
                }
                return false;
            }
            day = Days.ContainsKey(day.Date.AddDays(1).Date) ? Days[day.Date.AddDays(1).Date] : null;
        }
        return true;
    }
}