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
        currentDate = DateTime.Now; // format dd.mm.yyyy hh:mm:ss
    }

    public Calendar(List<Day> days, List<Task> allTasks)
    {
        this.days = days;
        this.allTasks = allTasks; 
        currentDate = DateTime.Now; // format dd.mm.yyyy hh:mm:ss
    }

    // TODO: Check the case when we add a task for today since this function will be returning 0
    private int numberOfDaysInRange(DateTime startingDate, DateTime endDate) => ((endDate.Date - startingDate.Date).Days);

    private bool shouldAddDay(Task task) => days[days.Count - 1].hoursToShift * -1 < task.duration;

    public List<Day> getRangeOfDaysForTask(Task task)
    {
        List<Day> validDays = new List<Day>();

        // if there are no days in the calendar add a new day and add the task there
        // TODO: don't add just one day, but add days according to the duration of the task
        // TODO: Should it be here? 
        Day newDay;
        if (days.Count == 0) { 
            newDay = new Day(currentDate, new List<Task>(), defaultWorkingHoursInterval, defaultWorkingHoursInterval);
            days.Add(newDay);
            validDays.Add(newDay);
            return validDays;
        }

        if (shouldAddDay(task)) {
            newDay = new Day(days[days.Count - 1].date.AddDays(1), new List<Task>(), defaultWorkingHoursInterval, defaultWorkingHoursInterval);
            days[days.Count - 1].nextDay = newDay;
            days.Add(newDay);
        }

        int numberOfValidDaysInRange = numberOfDaysInRange(currentDate, task.deadline);

        // TODO: what if there is exactly one day in the range?
        for (int i = 0; i < numberOfValidDaysInRange; ++i)
        {
            // TODO: is it <= or just < ? : <=
            // validDays.Count == days.Count 
            // should be checked after adding the task
            // can validDays.Count be 0?
            if (days.Count <= i)
            {
                // TODO: add the amount of days according to the duration of the task
                // TODO: Check date.AddDays(1)
                if(validDays[validDays.Count - 1].isDayFull(task.duration)) {
                    newDay = new Day(validDays[validDays.Count - 1].date.AddDays(1), new List<Task>(), defaultWorkingHoursInterval, defaultWorkingHoursInterval);
                    days[days.Count - 1].nextDay = newDay;
                    days.Add(newDay);
                    validDays.Add(newDay);
                }
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
            for(int j = validDays[i].tasks.Count - 1; j >=0; --j) {
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

    public void addTask(Task task)
    {
        // TODO: if we add a task with todays deadline
        List<Day> validDays = getRangeOfDaysForTask(task);
        
        if(!isThereEnoughSpace(validDays, task)) {
            WriteLine("Not enough space");
            return;
        }

        // TODO: Check the commented if else
        foreach (Day day in validDays)
        {
            //if (day.tasks.Count != 0) {
            //foreach (Task curTask in day.tasks)
            for (int i = 0; i < day.tasks.Count; ++i) {
                // TODO: If equality optimize according to duration

                // TODO: Decide what to do when the task with deadline x has to be added to a day where there are tasks with deadline x                  
                // should it be just < ? just to avoid the options below?

                // TODO: When will there be no space to add a task?
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
        days[days.Count - 1].addTask(task, days[days.Count - 1].tasks.Count);
    }

    // when reordering the calendar take care of the dates and if you are sending a task to a date 
    // when the deadline would be finished, then send an error
    // there must be space for reordering since that is handled in addTask
    void reorderCalendar(Day day, int hours)
    {
        Day nextDay = day.nextDay;
        if (nextDay == null) { return; }

        List<Task> tasks = getTasksGivenHours(day, hours);
        foreach(Task task in tasks)
        {
            nextDay.addTask(task, 0);
        }
        reorderCalendar(nextDay, nextDay.hoursToShift);
    }

    // get the tasks that need to be shifted and remove them from the day
    // TODO: should the removing part be done in the reorder caledar function?
    List<Task> getTasksGivenHours(Day day, int hours)
    {
        // I'm checking the task that needs to be added as well
        // TODO: Think about it!
        List<Task> tasks = new List<Task>();
        for (int i = day.tasks.Count - 1; i >= 0; --i)
        {
            // this should not be handles here - if a task has the same deadline as some other task
            // add the new task afterwards
            if(day.tasks[i].deadline == day.date)
            {
                // then there is no space to add the task we are adding
                /*
                 Options:
                    -Modify task
                    -Add hours to the day
                    -Add hours equally to each day from the current date up until the deadline of the task
                 */
            }
            if (day.tasks[i].type != Type.FIXED)
            {
                // should never get a negative value?
                if (hours == 0) { break; }

                Task curTask = day.tasks[i];

                if (curTask.duration > hours)
                {
                    int additionalHours = 0;
                    // merge then split the task
                    if (curTask.isSplit)
                    {
                        additionalHours = curTask.splitTaskPtr.duration;
                        // TODO: take care of the case when the task would be split into multiple days/tasks
                        day.nextDay.removeTask(curTask.splitTaskPtr);
                        curTask.mergeTasks(curTask, curTask.splitTaskPtr);
                    }
                    // TODO: if curTask.duration - hours is 0 then the task should be removed from day and the merged task should be added to the next day
                    // hours is the value that needs to be removed, so, in that day we're left with curTask.duration - hours
                    int[] splitHours = { curTask.duration - hours - additionalHours, hours + additionalHours};
                    curTask.splitTask(splitHours, 0, curTask);
                    hours = 0;
                    tasks.Add(curTask.splitTaskPtr);
                }
                else
                {
                    // && hours -= curTask.duration == 0?
                    int curTaskHours = curTask.duration;
                    // oone special case: when the last task of the day is split and we need to shift the whole thing
                    if (curTask.isSplit) {
                        //day.nextDay.removeTask(curTask.splitTaskPtr);
                        // the curtask should be deleted and the curTask.splitTaskPtr should be merged in one
                        curTask.splitTaskPtr.duration += curTask.duration;
                        // P:TODO: is this correct?
                        //curTask.splitTaskPtr = curTask;
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
        return tasks;
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