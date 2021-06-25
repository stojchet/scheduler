using System;


class Program
{
    static void Main(string[] args)
    {
        
    }

    // take tasks from day and shift it either to the next or previous day
    void reorderCalendar(Day day, int hours, bool next, Day returnPoint)
    {
        Day dirDay;
        if (next) { dirDay = day.nextDay; }
        else { dirDay = day.prevDay; }

        if (dirDay == returnPoint || dirDay == null) { return; }

        getTasks(day, hours);

        foreach (Task task in tasks) { dirDay.addTask(task, 0); }
        foreach (Task task in tasks) { day.removeTask(task); }

        reorderCalendar(dirDay, dirDay.hoursToShift, next, null);
    }

    // returns 
    public List<Task> getTasks(Day day, int hours, Day dirDay){
        List<Task> tasks = new List<Task>();

        for (int i = day.tasks.Count - 1; i >= 0; --i)
        {
            if (hours == 0) { break; }

            Task curTask = day.tasks[i];

            if (!isDateValid(dirDay.date, curTask.deadline))
            {
                day.removeTask(curTask);
                // now reverse all action done so far
                // should I recursively reverse all action that has been done?
                for (int j = tasks.Count - 1; j >= 0; --j)
                {
                    if(tasks[j].name == "SPECIAL"){
                        // then the task was split and we only played with their hours
                        // the special task has duration parameter ehich indicates how many hours we need to
                        // add to the task in daay and remove from the task in dirDay
                        // we know that the split tasks are the last task in day ad the first task in dirday
                        // TODO: for delete the + and - should be in reverse order - not comletly sure
                        day.tasks[tasks.Count - 1].duration += tasks[j].duration;
                        dirDay.tasks[0].duration -= tasks[j].duration;
                    }
                    day.addTask(tasks[j], day.tasks.Count);
                    hours += tasks[j].duration;
                }
                throw new NoSpaceForTaskExeption("There is no space to add the task, error occured during shifting the tasks", day, curTask, hours);
            }

            // add split tasks to "tasks" so that I can handle it during the exception
            if (curTask.duration > hours)
            {
                int additionalHours = 0;

                if (curTask.isSplit)
                {
                    curTask.splitTaskPtr.duration += hours;
                    curTask.duration -= hours;
                    additionalHours = curTask.splitTaskPtr.duration;
                    tasks.Add(new Task("SPECIAL", null, hours, null, true));
                    continue;
                    // dirDay.removeTask(curTask.splitTaskPtr);
                    // merged in curTask which is in day
                    // curTask.mergeTasks(curTask, curTask.splitTaskPtr);
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
            }
        }
        return tasks;
    }
}