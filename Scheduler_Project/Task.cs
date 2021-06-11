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

    // TODO: Problem
    public void splitTask(int[] hours, int index, Task task)
    {
        task.duration = hours[0];
        task.isSplit = true;
        task.splitTaskPtr = new Task(name, deadline, hours[1], type, false);
    }

    public void mergeTasks(Task firstTask, Task secondTask)
    {
        firstTask.duration += secondTask.duration;
        firstTask.isSplit = secondTask.isSplit;
        //day.removeTask(secondTask);
        firstTask.splitTaskPtr = secondTask.splitTaskPtr;
    }
}