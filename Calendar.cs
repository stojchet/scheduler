using System;
using System.Collections.Generic;
using System.Linq;

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
        this.DefaultWorkingHours = 8;
        this.DefaultWorkingHoursInterval = (9, 17);
        Days.Add(new Day(DateTime.Now, new List<Task>(), DefaultWorkingHoursInterval));
        this.CurrentDate = DateTime.Now; // format dd.mm.yyyy hh:mm:ss
    }

    public Calendar(List<Day> days, (int, int) workingHoursInterval, int workingHours)
    {
        this.Days = days;
        this.DefaultWorkingHoursInterval = workingHoursInterval;
        this.DefaultWorkingHours = workingHours;
        this.CurrentDate = DateTime.Now; // format dd.mm.yyyy hh:mm:ss
        addDaysUpToDate(DateTime.Now);
    }

    private static int numberOfDaysInRange(DateTime startingDate, DateTime endDate) => (endDate.Date - startingDate.Date).Days + 1;

    public static bool isDateValid(DateTime startingDate, DateTime endDate) => numberOfDaysInRange(startingDate, endDate) > 0;

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

    private int shouldAddDay(int hours)
    {
        Day day = Days[Days.Count - 1];
        while (day != null && !day.isDayFull(0) && isDateValid(DateTime.Now, day.Date))
        {
            hours += day.hoursToShift;
            day = day.PrevDay;
        }
        return hours;
    }

    public void addDaysUpToDate(DateTime date)
    {
        int daysToAdd = numberOfDaysInRange(Days[Days.Count - 1].Date, date);
        for (int i = 0; i < daysToAdd; ++i)
        {
            Day newDay = new Day(Days[Days.Count - 1].Date.AddDays(1), new List<Task>(), DefaultWorkingHoursInterval);
            newDay.PrevDay = Days[Days.Count - 1];
            Days[Days.Count - 1].NextDay = newDay;
            Days.Add(newDay);
        }
    }

    public List<Day> getRangeOfDaysForTask(Task task)
    {
        List<Day> validDays = new List<Day>();

        int shouldAddDayHours = shouldAddDay(task.Duration);
        if (shouldAddDayHours > 0)
        {
            addDaysUpToDate(Days[Days.Count - 1].Date.AddDays((int)Math.Ceiling((double)shouldAddDayHours / DefaultWorkingHours)));
        }

        int numberOfValidDaysInRange = numberOfDaysInRange(CurrentDate, task.Deadline);
        int startIndex = numberOfDaysInRange(Days[0].Date, CurrentDate) - 1;

        for (int i = 0; i < numberOfValidDaysInRange; ++i)
        {
            if (Days.Count - 1 < startIndex + i)
            {
                break;
            }

            validDays.Add(Days[startIndex + i]);
        }

        return validDays;
    }

    public bool doesTaskExist(Task t)
    {
        foreach(Day day in Days)
        {
            foreach(Task task in day.Tasks)
            {
                if (t.Equals(task))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public event ErrorNotify ErrorInTaskParameters;

    /* ----------------------------- Utility for add and delete task ----------------------------- */

    public bool checkForCondition(int i, int end, Direction dir) => dir == Direction.NEXT ? i >= end : i < end;

    public void reorderCalendar(Day day, int hoursToShift, Day returnPoint, Direction dir)
    {
        int hours = hoursToShift;
        Day dirDay = dir == Direction.NEXT ? day.NextDay : day.PrevDay;

        if (dirDay == null || (dirDay.Equals(returnPoint) && dir == Direction.PREVIOUS) || (day.hoursToShift <= 0 && dir == Direction.NEXT)) { return; }
        
        if (numberOfDaysInRange(DateTime.Now, dirDay.Date) <= 0) { return; }

        List<Task> tasks = new List<Task>();

        for (int i = dir == Direction.NEXT ? day.Tasks.Count - 1 : 0; checkForCondition(i, (dir == Direction.NEXT ? 0 : day.Tasks.Count), dir); i += (int)dir)
        {
            if (day.Tasks[i].Type != Type.FIXED)
            {
                if (hours == 0) { break; }

                Task curTask = day.Tasks[i];
                Task curTaskDir = curTask.getTaskByDirection(dir);

                if (!isDateValid(dirDay.Date, curTask.Deadline))
                {
                    for (int q = tasks.Count - 1; q >= 0; --q) {
                        day.addTask(tasks[q], day.Tasks.Count); 
                    }
                    throw new NoSpaceForTaskException("There is no space to add the Task, error occured during shifting the Tasks", day, curTask, hours);
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
                            curTaskDir.mergeTasks(curTaskDir, curTask, dir);
                            tasks.Add(curTaskDir);
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

        reorderCalendar(dirDay, dir == Direction.NEXT ? dirDay.hoursToShift < 0 ? hoursToShift : dirDay.hoursToShift : hoursToShift, returnPoint, dir);
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
            addDaysUpToDate(Days[Days.Count - 1].Date.AddDays((int)Math.Ceiling((double)shouldAddDayHours / DefaultWorkingHours)));
        }
        if(numberOfDaysInRange(task.Deadline, Days[Days.Count - 1].Date) <= 0)
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
                if (numberOfDaysInRange(day.Tasks[i].Deadline, task.Deadline) <= 0 && day.Tasks[i].Type != Type.FIXED)
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
            day = day.NextDay;
        }

        while (task != null)
        {
            day.removeTask(task);
            task.NextSplitTaskPtr = null;

            reorderCalendar(Days[Days.Count - 1], task.Duration, day.PrevDay, Direction.PREVIOUS);

            task = task.PrevSplitTaskPtr;
            day = day.PrevDay;
        }
    }

    /* ------------------------------ Modify Task ---------------------------------- */
    public void modifyTask(Task task, DateTime newDeadline, String newName)
    {
        Task t = task;
        while (t != null)
        {
            t.Name = newName;
            t.Deadline = newDeadline;
        }

        t = task.NextSplitTaskPtr;
        while (t != null)
        {
            t.Name = newName;
            t.Deadline = newDeadline;
        }
    }

    /* --------------------------- Change Working Hours ------------------------------- */
    public bool changeWorkingHours(DateTime date, (int, int) previousWorkingHoursInterval)
    {
        Day day = getDayByDate(date);
        int previousWorkingHours = previousWorkingHoursInterval.Item2 - previousWorkingHoursInterval.Item1;
        if(previousWorkingHours - day.WorkingHours < 0)
        {
            reorderCalendar(Days[Days.Count - 1], Math.Abs(previousWorkingHours - day.WorkingHours), day, Direction.PREVIOUS);
        }
        else if(previousWorkingHours - day.WorkingHours > 0)
        {
            int hoursNeeded = shouldAddDay(day.hoursToShift);
            if (hoursNeeded > 0)
            {
                int daysNeeded = (int)Math.Ceiling((double)hoursNeeded / day.WorkingHours);
                addDaysUpToDate(Days[Days.Count - 1].Date.AddDays(daysNeeded));
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
                        reorderCalendar(exception.Day, exception.Day.hoursToShift - (previousWorkingHours - tryWorkingHours), null, Direction.NEXT);
                    }
                    reorderCalendar(exception.Day, shiftHours, day.PrevDay, Direction.PREVIOUS);
                }
                
                ErrorInTaskParameters?.Invoke();
                return false;
            }
        }
        return true;
    }

    public bool changeDefaultWorkingHours((int, int) workingHoursInterval)
    {
        this.DefaultWorkingHours = workingHoursInterval.Item2 - workingHoursInterval.Item1;
        this.DefaultWorkingHoursInterval = workingHoursInterval;

        Day day = getDayByDate(DateTime.Now);

        while (day != null)
        {
            (int, int) previousWorkingHoursInterval = day.WorkingHoursInterval;
            day.WorkingHoursInterval = this.DefaultWorkingHoursInterval;

            if(!changeWorkingHours(day.Date, previousWorkingHoursInterval))
            {
                while(day != null && numberOfDaysInRange(DateTime.Now, day.Date) > 0)
                {
                    day.WorkingHoursInterval = previousWorkingHoursInterval;
                    reorderCalendar(Days[Days.Count - 1], day.hoursToShift * -1, day.PrevDay, Direction.PREVIOUS);
                    day = day.PrevDay;
                }
                return false;
            }
            day = day.NextDay;
        }
        return true;
    }
}