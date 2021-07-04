using System;
using static System.Console;

public enum Type {FIXED, NORMAL}

[Serializable]
public class Task
{
    public string name { get; set; }
    
    public DateTime deadline { get; set; } // format dd.mm.yyyy hh:mm:ss
    public int duration { get; set; }
    public Type type { get; set; }
    public bool isSplit { get; set; }
    public Task splitTaskPtr { get; set; }


    public Task(string name, DateTime deadline, int duration, Type type, bool isSplit)
    {
        this.name = name;
        this.deadline = deadline;
        this.duration = duration;
        this.type = type;
        this.isSplit = isSplit;
        this.splitTaskPtr = null;
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
            return t.name == name;
        }
    }

    public override int GetHashCode() => name.GetHashCode();

    public void splitTask(int[] hours, int index, Task task, Day day)
    {
        task.duration = hours[0];
        task.isSplit = true;
        task.splitTaskPtr = new Task(name, deadline, hours[1], type, false);
    }

    public void mergeTasks(Task firstTask, Task secondTask)
    {
        firstTask.duration += secondTask.duration;
        firstTask.isSplit = secondTask.isSplit;
        firstTask.splitTaskPtr = secondTask.splitTaskPtr;
    }
}