using System;
using System.Collections.Generic;
using static System.Console;
static class Program
{
    static void Main()
    {
        Calendar calendar = new Calendar();
        calendar.defaultWorkingHoursInterval = 12;
        Task task = new Task("kdb", new DateTime(2021, 07, 08), 5, Type.NORMAL, false);
        Task t = new Task("Test", new DateTime(2021, 07, 07), 3, Type.NORMAL, false);
        Task t1 = new Task("Test1", new DateTime(2021, 06, 14), 5, Type.NORMAL, false);
        Task t2 = new Task("Test2", new DateTime(2021, 06, 15), 2, Type.NORMAL, false);
        Task t3 = new Task("Test3", new DateTime(2021, 06, 15), 6, Type.NORMAL, false);
        Task t4 = new Task("Test4", new DateTime(2021, 06, 15), 2, Type.NORMAL, false);
        Task t5 = new Task("Test5", new DateTime(2021, 06, 15), 3, Type.NORMAL, false);
        Task t6 = new Task("Test6", new DateTime(2021, 06, 14), 10, Type.NORMAL, false);

        calendar.addTask(task);
        calendar.addTask(t);
        calendar.addTask(t1);
        /*calendar.addTask(t2);
        calendar.addTask(t3);
        calendar.addTask(t4);
        calendar.addTask(t5);*/
        calendar.addTask(t6);

        calendar.print();
    }
}