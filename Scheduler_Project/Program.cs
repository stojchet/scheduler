using System;
using System.Windows.Forms;
using static System.Console;
static class Program
{
    static void Main()
    {
        // TODO: Have separate default working hours for weekends
        Calendar calendar = new Calendar();
        calendar.defaultWorkingHours = 12;
        /*Task task = new Task("kdb", new DateTime(2021, 07, 08), 5, Type.NORMAL, false);
        Task t = new Task("Test", new DateTime(2021, 07, 07), 3, Type.NORMAL, false);
        Task t1 = new Task("Test1", new DateTime(2021, 06, 15), 5, Type.NORMAL, false);
        Task t2 = new Task("Test2", new DateTime(2021, 06, 16), 7, Type.NORMAL, false);
        Task t3 = new Task("Test3", new DateTime(2021, 06, 15), 6, Type.NORMAL, false);
        Task t4 = new Task("Test4", new DateTime(2021, 06, 16), 3, Type.NORMAL, false);
        Task t5 = new Task("Test5", new DateTime(2021, 06, 16), 5, Type.NORMAL, false);
        Task t6 = new Task("Test6", new DateTime(2021, 06, 15), 3, Type.NORMAL, false);*/

        Task task = new Task("kdb", new DateTime(2021, 07, 08), 5, Type.NORMAL, false);
        Task t = new Task("Test", new DateTime(2021, 07, 07), 3, Type.NORMAL, false);
        Task t1 = new Task("Test1", new DateTime(2021, 06, 17), 5, Type.NORMAL, false);
        Task t2 = new Task("Test2", new DateTime(2021, 06, 19), 7, Type.NORMAL, false);
        Task t3 = new Task("Test3", new DateTime(2021, 06, 19), 6, Type.NORMAL, false);
        Task t4 = new Task("Test4", new DateTime(2021, 06, 18), 3, Type.NORMAL, false);
        Task t5 = new Task("Test5", new DateTime(2021, 06, 18), 5, Type.NORMAL, false);
        Task t6 = new Task("Test6", new DateTime(2021, 06, 18), 3, Type.NORMAL, false);
        // Make it FIXED
        Task t7 = new Task("Test7", new DateTime(2021, 06, 18), 3, Type.NORMAL, false);

        calendar.addTask(task);
        calendar.addTask(t);
        calendar.addTask(t7);
        calendar.addTask(t1);
        calendar.addTask(t2);
        calendar.addTask(t3);
        calendar.addTask(t4);
        calendar.addTask(t5);
        calendar.addTask(t6);
        calendar.print();
        WriteLine();
        calendar.deleteTaskRunHelper(t7);
        calendar.print();
        // TODO: When deleting a task you should check whether it is split
        // In both directions i.e. from both days
        calendar.deleteTaskRunHelper(t5);
        calendar.print();
    }
}