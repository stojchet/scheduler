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
        Days.Add(new Day(DateTime.Now, new List<Task>(), DefaultWorkingHoursInterval, DefaultWorkingHours));
        this.CurrentDate = DateTime.Now; // format dd.mm.yyyy hh:mm:ss
    }

    public Calendar(List<Day> days, (int, int) workingHoursInterval, int workingHours)
    {
        this.Days = days;
        this.DefaultWorkingHoursInterval = workingHoursInterval;
        this.DefaultWorkingHours = workingHours;
        this.CurrentDate = DateTime.Now; // format dd.mm.yyyy hh:mm:ss
        addDaysUpToDate(DateTime.Now, -1);
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

    private int shouldAddDay(Task task)
    {
        int hours = task.Duration;
        Day day = Days[Days.Count - 1];
        while (day != null && !day.isDayFull(0) && isDateValid(DateTime.Now, day.Date))
        {
            hours += day.hoursToShift;
            day = day.PrevDay;
        }
        return hours;
    }

    // TODO: change funtion let it just accept parameter date
    public void addDaysUpToDate(DateTime date, int duration)
    {
        int daysToAdd;
        daysToAdd = (int)Math.Ceiling((double)duration / DefaultWorkingHours);

        for(int i = 0; i < daysToAdd; ++i)
        {
            if (duration == 0)
            {
                break;
            }

            Day newDay = new Day(Days[Days.Count - 1].Date.AddDays(1), new List<Task>(), DefaultWorkingHoursInterval, DefaultWorkingHours);
            newDay.PrevDay = Days[Days.Count - 1];
            Days[Days.Count - 1].NextDay = newDay;
            Days.Add(newDay);
            duration = duration >= newDay.WorkingHours || duration < 0 ? (duration - newDay.WorkingHours) : 0;
        }
    }

    public List<Day> getRangeOfDaysForTask(Task task)
    {
        List<Day> validDays = new List<Day>();

        int shouldAddDayHours = shouldAddDay(task);
        if (shouldAddDayHours > 0)
        {
            addDaysUpToDate(task.Deadline, shouldAddDayHours);
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
                _addFixedTask(task);
            }
        }
        catch (NoSpaceForTaskExeption exception)
        {
            if(exception.Day != null)
            {
                // ???????????????
                // getDayByDate(task.Deadline)
                Day day = exception.Day;
                day.removeTask(task);
                if (day.hoursToShift > 0)
                {
                    reorderCalendar(day, day.hoursToShift, null, Direction.NEXT);
                }
                reorderCalendar(exception.Day.NextDay, day.hoursToShift * -1, day, Direction.PREVIOUS);
            }

            ErrorInTaskParameters?.Invoke();
        }
    }

    // have tasks  add fixed ex 27 and add long 
    // now fixed is entangled between the long
    // also try to have a task whose deadline is after the long tasks deadline and try deleting the long task
    public void _addFixedTask(Task task)
    {
        int shouldAddDayHours = shouldAddDay(task);
        if (shouldAddDayHours > 0)
        {
            addDaysUpToDate(task.Deadline, shouldAddDayHours);
        }
        if(numberOfDaysInRange(task.Deadline, Days[Days.Count - 1].Date) <= 0)
        {
            addDaysUpToDate(task.Deadline, numberOfDaysInRange(Days[Days.Count - 1].Date, task.Deadline));
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
    }

    public bool checkForCondition(int i, int end, Direction dir) => dir == Direction.NEXT ? i >= end : i < end;

    public void reorderCalendar(Day day, int hours, Day returnPoint, Direction dir)
    {
        Day dirDay = dir == Direction.NEXT ? day.NextDay : day.PrevDay;

        if (dirDay == null || (dirDay.Equals(returnPoint) && dir == Direction.PREVIOUS) || (day.hoursToShift <= 0 && dir == Direction.NEXT)) { return; }

        List<Task> tasks = new List<Task>();

        for (int i = dir == Direction.NEXT ? day.Tasks.Count - 1 : 0; checkForCondition(i, (dir == Direction.NEXT ? 0 : day.Tasks.Count), dir); i += (int)dir)
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

                Task curTaskDir = curTask.getTaskByDirection(dir);
                if (curTask.Duration > hours)
                {
                    int additionalHours = 0;

                    if (curTaskDir != null)
                    {
                        additionalHours = curTaskDir.Duration;
                        dirDay.removeTask(curTaskDir);
                        curTask.mergeTasks(curTask, curTaskDir);
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
                            curTask.mergeTasks(curTask, curTaskDir);
                            tasks.Add(curTask);
                        }
                        else
                        {
                            curTaskDir.mergeTasks(curTaskDir, curTask);
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

        reorderCalendar(dirDay, dirDay.hoursToShift, returnPoint, dir);
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

    /* --------------------------- Change Working Hours ------------------------------- */

    public void changeWorkingHours(DateTime date, int previousWorkingHours)
    {
        Day day = getDayByDate(date);
        if(previousWorkingHours - day.WorkingHours < 0)
        {
            reorderCalendar(Days[Days.Count - 1], Math.Abs(previousWorkingHours - day.WorkingHours), day, Direction.PREVIOUS);
        }
        else if(previousWorkingHours - day.WorkingHours > 0)
        {
            // call add days upto date
            // shrinking the working day, may need to add days
            for (int i = 0; i < (int)Math.Ceiling((double)previousWorkingHours / day.WorkingHours); ++i)
            {
                Days.Add(new Day(Days[Days.Count].Date.AddDays(1), new List<Task>(), DefaultWorkingHoursInterval, DefaultWorkingHours));
            }

            try
            {
                reorderCalendar(day, previousWorkingHours - day.WorkingHours, null, Direction.NEXT);
            }
            catch(NoSpaceForTaskExeption exception)
            {
                day.WorkingHours = previousWorkingHours;
                if (exception.Day != null)
                {
                    reorderCalendar(exception.Day.NextDay, day.hoursToShift * -1, day, Direction.PREVIOUS);
                }
                ErrorInTaskParameters?.Invoke();
            }
        }
    }

    // check if default wokring hours is used somewhere
    // also if the addition of days is handled in the above function
    // adding days uses default working hours
    // when i get to adding days its good to have the default working hours set right
    // reorder assumes it has enough days
    // i should add new days in changeWokringHours
    public void changeDefaultWorkingHours((int, int) workingHoursInterval)
    {
        this.DefaultWorkingHours = workingHoursInterval.Item2 - workingHoursInterval.Item1;
        this.DefaultWorkingHoursInterval = workingHoursInterval;

        Day day = getDayByDate(DateTime.Now);
        day.WorkingHours = this.DefaultWorkingHours;
        day.WorkingHoursInterval = this.DefaultWorkingHoursInterval;

        while (day != null)
        {
            changeWorkingHours(day.Date, day.WorkingHours);
            day = day.NextDay;
        }
    }
}