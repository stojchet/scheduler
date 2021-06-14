using System;
using System.Collections.Generic;
using static System.Console;

[Serializable]
public class Calendar
{
    public List<Day> days { get; set; }
    // make an indexer for tasks ind day
    public List<Task> allTasks { get; set; }
    public DateTime currentDate { get; set; }
    public int defaultWorkingHoursInterval { get; set; }

    public Calendar()
    {
        this.days = new List<Day>();
        this.allTasks = new List<Task>();
        this.currentDate = DateTime.Now; // format dd.mm.yyyy hh:mm:ss
    }

    public Calendar(List<Day> days, List<Task> allTasks)
    {
        this.days = days;
        this.allTasks = allTasks; 
        this.currentDate = DateTime.Now; // format dd.mm.yyyy hh:mm:ss
    }

    // TODO: Check the case when we add a task for today since this function will be returning 0
    private int numberOfDaysInRange(DateTime startingDate, DateTime endDate) => (endDate.Date - startingDate.Date).Days + 1;

    private bool isDateValid(DateTime startingDate, DateTime endDate) => numberOfDaysInRange(startingDate, endDate) >= 0;

    private bool shouldAddDay(Task task) => days[days.Count - 1].hoursToShift * -1 < task.duration;

    public List<Day> getRangeOfDaysForTask(Task task)
    {
        List<Day> validDays = new List<Day>();

        // TODO: don't add just one day, but add days according to the duration of the task
        Day newDay;
        if (days.Count == 0) { 
            newDay = new Day(currentDate, new List<Task>(), defaultWorkingHoursInterval, defaultWorkingHoursInterval);
            days.Add(newDay);
            validDays.Add(newDay);
            return validDays;
        }

        if (shouldAddDay(task)) {
            newDay = new Day(days[days.Count - 1].date.AddDays(1), new List<Task>(), defaultWorkingHoursInterval, defaultWorkingHoursInterval);
            newDay.prevDay = days[days.Count - 1];
            days[days.Count - 1].nextDay = newDay;
            days.Add(newDay);
        }

        int numberOfValidDaysInRange = numberOfDaysInRange(currentDate, task.deadline);

        if(numberOfValidDaysInRange == 0)
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

    public bool isThereEnoughSpace(List<Day> validDays, Task task) {
        // check how much hours are left in the day
        int hoursLeft = validDays[validDays.Count - 1].workingHours - validDays[validDays.Count - 1].totalHoursTasks();
        int hours = task.duration - hoursLeft;
        if (hours <= 0) { return true; }

        for(int i = validDays.Count - 1; i >= 0; --i) {
            for(int j = validDays[i].tasks.Count - 1; j >= 0; --j) {
                //                     start date     end date
                if(numberOfDaysInRange(task.deadline, validDays[i].tasks[j].deadline) > 0) {
                    hours -= task.duration;
                }
                else {
                    break;
                }
            }
        }
        return hours <= 0;
    }

    public bool temp(Task task, List<Day> validDays, ref Day d)
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
            return false;
        }
        return true;

    }

    public void addTask(Task task)
    {
        List<Day> validDays = getRangeOfDaysForTask(task);
        
        if(!isThereEnoughSpace(validDays, task)) {
            WriteLine("Not enough space");
            return;
        }

        foreach (Day day in validDays)
        {
            // TODO: Handle it in a different way
            //if (day.tasks.Count != 0) {
            for (int i = 0; i < day.tasks.Count; ++i) {
                // TODO: If equality optimize according to duration
                if (numberOfDaysInRange(day.tasks[i].deadline, task.deadline) <= 0 && day.tasks[i].type != Type.FIXED) {
                    
                    day.addTask(task, i);
                    if (day.isDayFull(0)) {
                        reorderCalendar(day, day.hoursToShift);
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

        Day d = validDays[0];

        temp(task, validDays, ref d);

        d.addTask(task, d.tasks.Count);
        if (d.isDayFull(0))
        {
            reorderCalendar(d, d.hoursToShift);
        }
    }

    // when reordering the calendar take care of the dates and if you are sending a task to a date 
    // when the deadline would be finished, then send an error
    void reorderCalendar(Day day, int hours)
    {
        Day nextDay = day.nextDay;
        if (nextDay == null) { return; }
        List<Task> tasks = new List<Task>();

        for (int i = day.tasks.Count - 1; i >= 0; --i)
        {
            if (day.tasks[i].type != Type.FIXED)
            {
                if (hours == 0) { break; }

                Task curTask = day.tasks[i];

                if (curTask.duration > hours)
                {
                    int additionalHours = 0;

                    if (curTask.isSplit)
                    {
                        additionalHours = curTask.splitTaskPtr.duration;
                        // TODO: take care of the case when the task would be split into multiple days/tasks
                        day.nextDay.removeTask(curTask.splitTaskPtr);
                        curTask.mergeTasks(curTask, curTask.splitTaskPtr);
                    }

                    int[] splitHours = { curTask.duration - hours - additionalHours, hours + additionalHours};
                    // TODO: should I pass day or day.nextDay to splitTask as a parameter
                    curTask.splitTask(splitHours, 0, curTask, day);
                    if (!isDateValid(day.nextDay.date, curTask.splitTaskPtr.deadline))
                    {
                        WriteLine("There is no space to add task");
                        return;
                    }
                    hours = 0;
                    //day.nextDay.addTask(curTask.splitTaskPtr, 0);
                    tasks.Add(curTask.splitTaskPtr);
                }
                else
                {
                    int curTaskHours = curTask.duration;
                    if (curTask.isSplit) {
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

        foreach(Task task in tasks)
        {
            if (!isDateValid(day.nextDay.date, task.deadline))
            {
                WriteLine("There is no space to shift the tasks");
                // TODO: Throw exeption
                // then there is no space to add the task we are adding
                /*
                 Options:
                    -Modify task
                    -Add hours to the day
                    -Add hours equally to each day from the current date up until the deadline of the task
                 */
                return;
            }
            nextDay.addTask(task, 0);
        }
        
        reorderCalendar(nextDay, nextDay.hoursToShift);
    }

    void deleteTask(Day day, Task task)
    {
        for (int i = 0; i < day.tasks.Count; ++i)
        {
            if (day.tasks[i] == task)
            {
                day.removeTask(task);
                // Iterate the calendar backwards and remove tasks from day x and add them to day x - 1
                // until we have reached day "day"
                // Idea 
                // First - instaead of a "linked list" make the data structure a doubly linked list
                // Second - pass the "directon" of reordering the calendar as a parameter and use it both ways
                // i.e. modify reorder calendar
            }
        }
    }

    void modifyTask<T>(Day day, Task task, string parameterType, string parameter)
    {
        // TODO: Ask Bane => if I iterate with foreach will the changes be saved?
        // TODO: How to compare two tasks, should I overwrite compareTo method?
        for(int i = 0; i < day.tasks.Count; ++i)
        {
            if(day.tasks[i] == task)
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
                    task.duration = int.Parse(parameter);
                    reorderCalendar(day, int.Parse(parameter));

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

    public void print() {
        foreach(Day day in days) {
            WriteLine(day.date.ToString());

            foreach(Task task in day.tasks) {
                WriteLine($"    Name: {task.name} Deadline: {task.deadline} Duration: {task.duration}");
            }
        }
    }
}