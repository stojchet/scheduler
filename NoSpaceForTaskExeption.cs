using System;

class NoSpaceForTaskExeption: Exception
{
    public Day Day { get; set; }
    public Task Task { get; set; }
    public int Hours { get; set; }
    public NoSpaceForTaskExeption() : base() { }
    public NoSpaceForTaskExeption(string message) : base(message) { }

    public NoSpaceForTaskExeption(string message, Day day, Task task, int hours): base(message)
    {
        this.Day = day;
        this.Task = task;
        this.Hours = hours;
    }

}

