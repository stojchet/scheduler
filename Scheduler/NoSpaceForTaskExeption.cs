using System;

class NoSpaceForTaskExeption: Exception
{
    public Day day { get; set; }
    public Task task { get; set; }
    public int hours { get; set; }
    public NoSpaceForTaskExeption() : base() { }
    public NoSpaceForTaskExeption(string message) : base(message) { }

    public NoSpaceForTaskExeption(string message, Day day, Task task, int hours): base(message)
    {
        this.day = day;
        this.task = task;
        this.hours = hours;
    }

}

