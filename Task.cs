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

    public Task(string name, DateTime deadline, Type type, bool isSplit) : this(name, deadline, 0, type, isSplit){}

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
        // TODO: Shouldn't it be deadline.AddDay(1) ? 
        task.splitTaskPtr = new Task(name, deadline, hours[1], type, false);
    }

    public void mergeTasks(Task firstTask, Task secondTask)
    {
        firstTask.duration += secondTask.duration;
        firstTask.isSplit = secondTask.isSplit;
        //day.removeTask(secondTask);
        firstTask.splitTaskPtr = secondTask.splitTaskPtr;
    }

    // TODO: Think of delete, should I have single function for merge and delete?
    public void splitAndMerge(Task task, int[] hours, int index) {
        if (task.isSplit) {
            //if(task.duration)
        }
        task.duration = hours[0];
    }

    public override string ToString()
    {
        return this.name + " " + new string('.', 20 - name.Length) + " " + duration + "h";
    }
}