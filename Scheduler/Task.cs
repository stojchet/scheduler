using System;
using static System.Console;

public enum Type {FIXED, NORMAL}

[Serializable]
public class Task
{
    string _name;
    DateTime _deadline;
    int _duration;
    Type _type;
    bool _isSplit;
    Task _splitTaskPtr;
    // modify it when the user check the checkbox
    bool done;
    public string name {
        get => _name;
        set { _name = value; }
    }
    public DateTime deadline {
        get => _deadline; 
        set { _deadline = value; }
    } // format dd.mm.yyyy hh:mm:ss
    public int duration {
        get => _duration; 
        set { _duration = value; }
    }
    public Type type {
        get => _type; 
        set { _type = value; }
    }
    public bool isSplit {
        get => _isSplit; 
        set { _isSplit = value; }
    }
    public Task splitTaskPtr {
        get => _splitTaskPtr; 
        set { _splitTaskPtr = value; }
    }


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

    public override int GetHashCode()
    {
        long hash = 0;

        foreach (char c in name)
        {
            hash = hash * 1000003 + c.GetHashCode();
        }
        return (int) hash;
    }

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