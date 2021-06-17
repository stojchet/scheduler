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

    // Make an indexer?
    private Day getDayByDate(DateTime date)
    {
        foreach(Day day in days)
        {
            if(day.date.Date == date.Date)
            {
                return day;
            }
        }
        WriteLine("Day has not been added -> check for error");
        return null;
    }

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
        // make oneMore strive to gove equal working hours if some day has less working hours than some other day
        int oneMore = hours - hoursPerDay * days.Count - 1; // to start counter i from 0

        for (int i = 0; i < days.Count; ++i)
        {
            days[i].workingHours += hoursPerDay;
            if (i <= oneMore)
            {
                days[i].workingHours++;
            }
        }

        // call deleteReorderCalendar and send add +hours to the function's hours parameter
        // I need deleteReorderCalendar but in the opposite direction
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
            WriteLine(task.name);
            Day day = findTaskInDay(task);
            day.removeTask(task);
            deleteReorderCalendar(exception.day.nextDay, day.hoursToShift * -1, day.nextDay);
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

    // exeption handling
    // 1. Task is larger than working hours allowed
    // 2. There already are some fixed tasks in the day and hence there is no space to add the task
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
            for(int i = 0; i < numberOfDaysInRange(task.deadline, days[days.Count - 1].date) * -1 + 1; ++i)
            {
                Day newDay = new Day(days[days.Count - 1].date.AddDays(1), new List<Task>(), defaultWorkingHoursInterval, defaultWorkingHours);
                days[days.Count - 1].nextDay = newDay;
                days.Add(newDay);
            }
            day = getDayByDate(task.deadline);
            day.addTask(task, 0);
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
        Day dirDay = day.nextDay;

        if (dirDay == null || dirDay.Equals(returnPoint)) { return; }

        List<Task> tasks = new List<Task>();

        for (int i = day.tasks.Count - 1; i >= 0; --i)
        {
            if (day.tasks[i].type != Type.FIXED)
            {
                if (hours == 0) { break; }

                Task curTask = day.tasks[i];

                if (!isDateValid(dirDay.date, curTask.deadline))
                {
                    throw new NoSpaceForTaskExeption("There is no space to add the task, error occured during shifting the tasks", day, curTask, hours);
                }

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

        // TODO: When the task is split merge it
        foreach (Task task in tasks) { dirDay.addTask(task, 0); }

        reorderCalendar(dirDay, dirDay.hoursToShift, next, null);
    }

    void deleteReorderCalendar(Day day, int hours, Day returnPoint)
    {
        Day dirDay =  day.prevDay;

        if (dirDay == null || dirDay.Equals(returnPoint)) { return; }

        List<Task> tasks = new List<Task>();
        // for loop is retarded when I delete the element
        for (int i = 0; i < day.tasks.Count; ++i)
        {
            if (day.tasks[i].type != Type.FIXED)
            {
                if (hours == 0) { break; }

                Task curTask = day.tasks[i];

                if (curTask.duration > hours)
                {
                    WriteLine(dirDay.tasks[dirDay.tasks.Count - 1]);
                    // only the first element can be split
                    if (dirDay.tasks[dirDay.tasks.Count - 1].isSplit && i == 0)
                    {
                        curTask.duration -= hours;
                        dirDay.tasks[dirDay.tasks.Count - 1].duration += hours;
                        tasks.Add(dirDay.tasks[dirDay.tasks.Count - 1]);
                        dirDay.removeTask(dirDay.tasks[dirDay.tasks.Count - 1]);
                    }
                    else
                    {
                        Task shiftSplitTask = new Task(curTask.name, curTask.deadline, hours, curTask.type, true);
                        shiftSplitTask.splitTaskPtr = curTask;
                        curTask.duration -= hours;

                        tasks.Add(shiftSplitTask);
                    }
                    
                    hours = 0;
                }
                else
                {
                    int curTaskHours = curTask.duration;
                    if (dirDay.tasks[dirDay.tasks.Count - 1].isSplit && i == 0)
                    {
                        dirDay.tasks[dirDay.tasks.Count - 1].mergeTasks(dirDay.tasks[dirDay.tasks.Count - 1], dirDay.tasks[dirDay.tasks.Count - 1].splitTaskPtr);
                        curTask = dirDay.tasks[dirDay.tasks.Count - 1];
                        dirDay.removeTask(dirDay.tasks[dirDay.tasks.Count - 1]);
                    }
                    tasks.Add(curTask);
                    hours -= curTaskHours;
                    day.removeTask(curTask);
                    i--;
                }
            }
        }

        // or should I just check here if the first task in tasks is split with the last task in dirDay and them just add the hours and isSplit and remove the from tasks and don't add it to dirDay again?
        /*if (dirDay.tasks[dirDay.tasks.Count - 1].isSplit)
        {
            dirDay.tasks[dirDay.tasks.Count - 1].isSplit = tasks[0].isSplit;
            dirDay.tasks[dirDay.tasks.Count - 1].splitTaskPtr = tasks[0].splitTaskPtr;
            dirDay.tasks[dirDay.tasks.Count - 1].duration += tasks[0].duration;
            tasks.RemoveAt(0);
        }*/

        for (int i = 0; i < tasks.Count; ++i) 
        { 
            dirDay.addTask(tasks[i], dirDay.tasks.Count); 
        }

        deleteReorderCalendar(dirDay, dirDay.hoursToShift, null);
    }

    private Day findTaskInDay(Task task)
    {
        foreach (Day day in days)
        {
            for (int i = 0; i < day.tasks.Count; ++i)
            {
                if (day.tasks[i].Equals(task))
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
            if (day.tasks[i].Equals(task))
            {
                day.removeTask(task);
                deleteReorderCalendar(days[days.Count - 1], task.duration, day.prevDay);
            }
        }
    }

    public void deleteTaskRunHelper(Task task)
    {
        foreach (Day day in days)
        {
            for (int i = 0; i < day.tasks.Count; ++i)
            {
                if (day.tasks[i].Equals(task))
                {
                    deleteTask(day, task);
                }
            }
        }
    }

    public void modifyTask(Day day, Task task, string parameterType, string parameter)
    {
        for (int i = 0; i < day.tasks.Count; ++i)
        {
            if (day.tasks[i].Equals(task))
            {
                if (parameterType == "deadline" || parameterType == "duration")
                {
                    day.removeTask(task);
                    task.deadline = DateTime.Parse(parameter);
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