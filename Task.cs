using System;
using static System.Console;

public enum Type {FIXED, NORMAL}
public enum Direction { PREVIOUS = 1, NEXT = -1 }

[Serializable]
public class Task
{
    public string Name { get; set; }

    public DateTime Deadline { get; set; } // format dd.mm.yyyy hh:mm:ss
    public int Duration { get; set; }
    public Type Type { get; set; }
    public Task NextSplitTaskPtr { get; set; }
    public Task PrevSplitTaskPtr { get; set; }


    public Task(string name, DateTime deadline, int duration, Type type)
    {
        this.Name = name;
        this.Deadline = deadline;
        this.Duration = duration;
        this.Type = type;
        this.NextSplitTaskPtr = null;
    }

    public int getFullTaskDuration()
    {
        int taskDuration = 0;
        Task t = this;
        while (t != null)
        {
            taskDuration += t.Duration;
            t = t.PrevSplitTaskPtr;
        }

        t = this.NextSplitTaskPtr;
        while (t != null)
        {
            taskDuration += t.Duration;
            t = t.NextSplitTaskPtr;
        }

        return taskDuration;
    }

    public override bool Equals(Object obj) => obj is Task t 
        && t.Name == Name && t.getFullTaskDuration() == getFullTaskDuration() && t.Deadline == Deadline && t.Type == Type;

    public override int GetHashCode() => Name.GetHashCode() + getFullTaskDuration().GetHashCode()
        + Deadline.GetHashCode() + Type.GetHashCode();

    public void splitTask(int[] hours, int index, Task task, Day day, Direction dir)
    {
        task.Duration = hours[0];
        if (dir == Direction.NEXT)
        {
            Task newTask = new Task(Name, Deadline, hours[1], Type);
            newTask.NextSplitTaskPtr = task.NextSplitTaskPtr;
            if (task.NextSplitTaskPtr != null)
            {
                task.NextSplitTaskPtr.PrevSplitTaskPtr = newTask;
            }
            task.NextSplitTaskPtr = newTask;
            task.NextSplitTaskPtr.PrevSplitTaskPtr = task;
        }
        else
        {
            Task newTask = new Task(Name, Deadline, hours[1], Type);
            newTask.PrevSplitTaskPtr = task.PrevSplitTaskPtr;
            if (task.PrevSplitTaskPtr != null)
            {
                task.PrevSplitTaskPtr.NextSplitTaskPtr = newTask;
            }
            task.PrevSplitTaskPtr = newTask;
            task.PrevSplitTaskPtr.NextSplitTaskPtr = task;
        }

    }

    public void mergeTasks(Task firstTask, Task secondTask, Direction dir)
    {
        firstTask.Duration += secondTask.Duration;
        if(dir == Direction.NEXT)
        {
            firstTask.NextSplitTaskPtr = secondTask.NextSplitTaskPtr;
            if (firstTask.NextSplitTaskPtr != null)
            {
                firstTask.NextSplitTaskPtr.PrevSplitTaskPtr = firstTask;
            }
        }
        else
        {
            firstTask.PrevSplitTaskPtr = secondTask.PrevSplitTaskPtr;
            if (firstTask.PrevSplitTaskPtr != null)
            {
                firstTask.PrevSplitTaskPtr.NextSplitTaskPtr = firstTask;
            }
        }        
    }

    public Task getTaskByDirection(Direction dir) =>
        dir == Direction.PREVIOUS ? this.PrevSplitTaskPtr : this.NextSplitTaskPtr;
}