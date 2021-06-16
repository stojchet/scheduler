using System;
using System.Collections.Generic;
using static System.Console;

[Serializable]
public class Calendar
{
    public List<Day> days { get; set; }
    // make an indexer for tasks ind day
    public DateTime currentDate { get; set; }
    public (int, int) defaultWorkingHoursInterval { get; set; }
    public int defaultWorkingHours { get; set; }

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

    // TODO: Check the case when we add a task for today since this function will be returning 0
    private int numberOfDaysInRange(DateTime startingDate, DateTime endDate) => (endDate.Date - startingDate.Date).Days + 1;

    private bool isDateValid(DateTime startingDate, DateTime endDate) => numberOfDaysInRange(startingDate, endDate) > 0;

    private bool shouldAddDay(Task task) => days[days.Count - 1].hoursToShift * -1 < task.duration;

    public List<Day> getRangeOfDaysForTask(Task task)
    {
        List<Day> validDays = new List<Day>();

        // TODO: don't add just one day, but add days according to the duration of the task
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

        if (numberOfValidDaysInRange == 0)
        {
            WriteLine("The date is not valid!");
        }

        for (int i = 0; i < numberOfValidDaysInRange; ++i)
        {
            if (days.Count <= i)
            {
                break;
            }
            validDays.Add(days[i]);
        }

        return validDays;
    }

    public void handleException(Day day, Task task)
    {
        WriteLine("There is no space to add the task with there parameters and these working hours. Do you want make some changes to the task?");
        if (ReadLine() == "Y")
        {
            WriteLine("Which parameters do you wish to change?" +
                "1. Working hours -> automatically more working hours will be added from today till the task's deadline so that you have enough time to finish it" +
                "2. The deadline of the task" +
                "3. The duration of the task");

            string answer = ReadLine();
            if (answer == "1")
            {
                // make a function that will take care of this.
            }
            else if (answer == "2")
            {
                WriteLine("Enter new deadline");
                modifyTask(day, task, "deadline", ReadLine());
            }
            else if (answer == "3")
            {
                WriteLine("Enter new duration");
                modifyTask(day, task, "duration", ReadLine());
            }
            else
            {
                WriteLine("Wrong input");
            }
        }
        else
        {
            // remove task
        }
    }

    // we add tasks to day and remove them from nextDay
    public List<Task> updateDays(Day day, int hours)
    {
        Day nextDay = day.nextDay;
        List<Task> tasks = new List<Task>();

        while (nextDay != null)
        {
            if(day.tasks.Count == 0)
            {
                nextDay = nextDay.nextDay;
            }

            // from nextDay I remove and add tasks to day
            for(int i = 0; i < nextDay.tasks.Count; ++i)
            {
                if(hours == 0){ break; }

                Task curTask = nextDay.tasks[i];

                // take care of exception if date is not valid

                if (curTask.duration > hours)
                {
                    int additionalHours = 0;

                    if (curTask.isSplit)
                    {
                        additionalHours = curTask.splitTaskPtr.duration;
                        // TODO: take care of the case when the task would be split into multiple days/tasks
                        nextDay.nextDay.removeTask(curTask.splitTaskPtr);
                        curTask.mergeTasks(curTask, curTask.splitTaskPtr);
                    }

                    int[] splitHours = { curTask.duration - hours - additionalHours, hours + additionalHours };
                    curTask.splitTask(splitHours, 0, curTask, day);

                    hours = 0;
                    // day.nextDay.addTask(curTask.splitTaskPtr, 0);
                    // add currentTask to day and currentTask.splitTaskPtr to nextDay
                    tasks.Add(curTask);
                    nextDay.removeTask(curTask);
                    nextDay.addTask(curTask.splitTaskPtr, 0);
                }
                else
                {
                    int curTaskHours = curTask.duration;
                    if (curTask.isSplit)
                    {
                        curTask.splitTaskPtr.duration += curTask.duration;
                        nextDay.nextDay.removeTask(curTask.splitTaskPtr);
                        // TODO: Get back to the above case
                    }
                    else
                    {
                        tasks.Add(curTask);
                        //day.nextDay.addTask(curTask, 0);
                    }
                    hours -= curTaskHours;
                    nextDay.removeTask(curTask);
                }

            }
        }
        // delete empty days
        return tasks;
    }

    public void modifyWorkingHours(int hours)
    {
        int hoursPerDay = hours / days.Count;
        // make the oneMore give more hours as we're approaching the dealine?
        int oneMore = hours - hoursPerDay * days.Count - 1; // to start counter i from 0

        for (int i = 0; i < days.Count; ++i)
        {
            days[i].workingHours += hoursPerDay;
            if (i <= oneMore)
            {
                days[i].workingHours++;
            }
        }

        // now reorganize the tasks
        // use reorderCalendar somehow?
        // should iterate though the days resusivelt
        // it should start iterating through the days from days[0] and add x hours of tasks then as we iterate through the days we add i * x amount of tasks to the days
        // and if we gat to a point when for ex 16 hours need to be shifter we must recursively access the days to get the tasks
    }

    public void addTask(Task task)
    {
        try
        {
            _addTask(task);
        }
        catch (NoSpaceForTaskExeption exception)
        {
            WriteLine(task.name);
            Day day = findTaskInDay(task);
            day.removeTask(task);
            reorderCalendar(exception.day, task.duration, false, day);
            // from day in the addTask from when I thorow the exeption until the day that I have here from which I remove the task
            /*
             Options:
                -Modify task
                -Add hours to the day
                -Delete task
                -Add hours equally to each day from the current date up until the deadline of the task
             */
        }
    }

    public void temp(Task task, List<Day> validDays, ref Day d)
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
            WriteLine("No space to add task");
            throw new NoSpaceForTaskExeption("There is no space to add the task which needs to be added as the last task in the calendar", d, task, hours - task.duration);
        }
    }

    public void _addTask(Task task)
    {
        List<Day> validDays = getRangeOfDaysForTask(task);

        // Insert task
        foreach (Day day in validDays)
        {
            //if (day.tasks.Count != 0) {
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
            //}
            /*else {
                day.addTask(task, 0);
                return;
            }*/
        }

        // Add task at the end
        Day d = validDays[0];

        temp(task, validDays, ref d);

        d.addTask(task, d.tasks.Count);
        if (d.isDayFull(0))
        {
            reorderCalendar(d, d.hoursToShift, true, null);
        }
    }

    // TODO: Check every implementation of reorderCalendar and check if the next and returnPoint parameters are sent correctly
    // TODO: Split reorderCalendar into two functions: one that will return a list of tasks given hours and day
    // and one that will do the work of reordering the calendar
    // when reordering the calendar take care of the dates and if you are sending a task to a date 
    // when the deadline would be finished, then send an error
    void reorderCalendar(Day day, int hours, bool next, Day returnPoint)
    {
        Day dirDay;
        int indexDir;
        dirDay = (next ? day.nextDay : day.prevDay);
        indexDir = (next ? day.tasks.Count - 1 : 0);

        // Equals
        if (dirDay == null || dirDay.Equals(returnPoint)) { return; }

        List<Task> tasks = new List<Task>();

        for (int i = day.tasks.Count - 1; i >= 0; --i)
        {
            if (day.tasks[i].type != Type.FIXED)
            {
                if (hours == 0) { break; }

                Task curTask = day.tasks[i];

                // if the date of where I need to shift the task is not compatible throw an exception
                // curTask in in day an I want to shift it to dirDay
                // the question is can I shift curTask in dirDay
                // I have not yet shifted the curTask anywhere
                if (!isDateValid(dirDay.date, curTask.deadline))
                {
                    // TODO: Should I remove the task since -> the exception
                    // day.removeTask(curTask);
                    // call reorderCalendar
                    // now reverse all action done so far
                    /* for (int j = tasks.Count - 1; j >= 0; --j)
                    {
                        day.addTask(tasks[j], day.tasks.Count);
                        hours += tasks[j].duration;
                    } */
                    throw new NoSpaceForTaskExeption("There is no space to add the task, error occured during shifting the tasks", day, curTask, hours);
                }

                // add split tasks to "tasks" so that I can handle it during the exception
                if (curTask.duration > hours)
                {
                    int additionalHours = 0;

                    if (curTask.isSplit)
                    {
                        additionalHours = curTask.splitTaskPtr.duration;
                        // TODO: take care of the case when the task would be split into multiple days/tasks
                        dirDay.removeTask(curTask.splitTaskPtr);
                        curTask.mergeTasks(curTask, curTask.splitTaskPtr);
                    }

                    int[] splitHours = { curTask.duration - hours - additionalHours, hours + additionalHours };
                    curTask.splitTask(splitHours, 0, curTask, day);

                    hours = 0;
                    //day.nextDay.addTask(curTask.splitTaskPtr, 0);
                    tasks.Add(curTask.splitTaskPtr);
                }
                else
                {
                    int curTaskHours = curTask.duration;
                    if (curTask.isSplit)
                    {
                        curTask.splitTaskPtr.duration += curTask.duration;
                    }
                    else
                    {
                        tasks.Add(curTask);
                        //day.nextDay.addTask(curTask, 0);
                    }
                    hours -= curTaskHours;
                    day.removeTask(curTask);

                }
            }
        }

        foreach (Task task in tasks) { dirDay.addTask(task, 0); }

        reorderCalendar(dirDay, dirDay.hoursToShift, next, null);
    }

    private Day findTaskInDay(Task task)
    {
        foreach (Day day in days)
        {
            for (int i = 0; i < day.tasks.Count; ++i)
            {
                if (day.tasks[i] == task)
                {
                    return day;
                }
            }
        }
        WriteLine("Task doesn't exist");
        return null;
    }

    // NOTE: Helper function until I make the interface
    public void deleteTask(Day day, Task task)
    {
        // use findTaskInDay to find the day when you're given only the task
        for (int i = 0; i < day.tasks.Count; ++i)
        {
            if (day.tasks[i] == task)
            {
                day.removeTask(task);
                reorderCalendar(days[days.Count - 1], task.duration, false, day);
            }
        }
    }

    public void deleteTaskRunHelper(Task task)
    {
        foreach (Day day in days)
        {
            for (int i = 0; i < day.tasks.Count; ++i)
            {
                if (day.tasks[i] == task)
                {
                    deleteTask(day, task);
                }
            }
        }
    }

    public void modifyTask(Day day, Task task, string parameterType, string parameter)
    {
        // TODO: compare two tasks, overwrite compareTo method
        for (int i = 0; i < day.tasks.Count; ++i)
        {
            if (day.tasks[i] == task)
            {
                if (parameterType == "deadline")
                {
                    day.removeTask(task);
                    task.deadline = DateTime.Parse(parameter);
                    addTask(task);
                }
                else if (parameterType == "duration")
                {
                    // Option 1
                    if (task.duration > int.Parse(parameter))
                    {
                        task.duration = int.Parse(parameter);
                        reorderCalendar(day, int.Parse(parameter), true, null);
                    }
                    else if (task.duration < int.Parse(parameter))
                    {
                        task.duration = int.Parse(parameter);
                        reorderCalendar(day, int.Parse(parameter), false, day);
                    }

                    // Option 2
                    day.removeTask(task);
                    task.duration = int.Parse(parameter);
                    addTask(task);
                }
                else if (parameterType == "name")
                {
                    task.name = parameter;
                }
            }
        }
    }

    void loadFile()
    {

    }

    void updateFile()
    {

    }

    public void print()
    {
        foreach (Day day in days)
        {
            WriteLine(day.date.ToString());

            foreach (Task task in day.tasks)
            {
                WriteLine($"\tName: {task.name} Deadline: {task.deadline} Duration: {task.duration}");
            }
        }
    }
}