using System;

class NoSpaceForTaskException: Exception
{
    public Day Day { get; set; }
    public Task Task { get; set; }
    public int Hours { get; set; }
    public NoSpaceForTaskException() : base() { }
    public NoSpaceForTaskException(string message) : base(message) { }

    public NoSpaceForTaskException(string message, Day day, Task task, int hours): base(message)
    {
        this.Day = day;
        this.Task = task;
        this.Hours = hours;
    }

}

