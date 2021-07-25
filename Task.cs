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
    public bool IsSplit { get; set; }
    public Task NextSplitTaskPtr { get; set; }
    public Task PrevSplitTaskPtr { get; set; }


    public Task(string name, DateTime deadline, int duration, Type type, bool isSplit)
    {
        this.Name = name;
        this.Deadline = deadline;
        this.Duration = duration;
        this.Type = type;
        this.IsSplit = isSplit;
        this.NextSplitTaskPtr = null;
    }

    public override bool Equals(Object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Task t = (Task)obj;
            return t.Name == Name;
        }
    }

    public override int GetHashCode() => Name.GetHashCode();

    public void splitTask(int[] hours, int index, Task task, Day day, Direction dir)
    {
        task.Duration = hours[0];
        task.IsSplit = true;
        if(dir == Direction.NEXT)
        {
            task.NextSplitTaskPtr = new Task(Name, Deadline, hours[1], Type, false);
            task.NextSplitTaskPtr.PrevSplitTaskPtr = task;
        }
        else
        {
            task.PrevSplitTaskPtr = new Task(Name, Deadline, hours[1], Type, false);
            task.PrevSplitTaskPtr.NextSplitTaskPtr= task;
        }
        
    }

    public void mergeTasks(Task firstTask, Task secondTask)
    {
        firstTask.Duration += secondTask.Duration;
        firstTask.IsSplit = secondTask.IsSplit;
        firstTask.NextSplitTaskPtr = secondTask.NextSplitTaskPtr;
    }

    public Task getTaskByDirection(Direction dir)
    {
        if(dir == Direction.PREVIOUS)
        {
            return this.PrevSplitTaskPtr;
        }
        return this.NextSplitTaskPtr;
    }
}