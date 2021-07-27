using System;
using static System.Console;
using System.Windows.Forms;
static class Program
{
    private const int totalTests = 13;
    private static int totalPassed = 0;

    static void assert(bool b)
    {
        if (!b)
            throw new Exception("assertion failed");
        
        totalPassed++;
    }

    static void test()
    {
        Calendar cal = new Calendar();
        Settings.MyCalendar = cal;

        Task t = new Task("mow the lawn", DateTime.Today.AddDays(3), 1, Type.NORMAL);
        cal.addTask(t);

        Day current = cal.getDayByDate(DateTime.Today);
        assert(current.Tasks.Count == 1);
        assert(current.Tasks[0] == t);

        cal.deleteTask(current, t);
        assert(current.Tasks.Count == 0);

        t = new Task("a long task", DateTime.Today.AddDays(30), 100, Type.NORMAL);
        cal.addTask(t);
        assert(current.Tasks.Count == 1);
        cal.deleteTask(current, t);
        assert(current.Tasks.Count == 0);

        // Error - No space in the day
        t = new Task("error", DateTime.Now, 20, Type.NORMAL);
        assert(current.Tasks.Count == 0);

        // Error
        t = new Task("buy groceries", DateTime.Today, 5, Type.NORMAL);
        cal.addTask(t);
        Task t1 = new Task("clean room", DateTime.Today.AddDays(2), 5, Type.NORMAL);
        cal.addTask(t1);
        // will throw error 
        /*******************Not throwing an exeption******************/
        Task t2 = new Task("do homework", DateTime.Today, 5, Type.NORMAL);
        cal.addTask(t2);
        assert(current.Tasks.Count == 2);
        assert(current.NextDay.Tasks.Count == 1);
        t2 = new Task("do homework", DateTime.Today.AddDays(1), 5, Type.NORMAL);
        cal.addTask(t2);

        // delete split task 
        cal.deleteTask(cal.getDayByDate(DateTime.Now), t2);

        assert(current.Tasks.Count == 2);
        assert(current.NextDay.Tasks.Count == 1);

        t2 = new Task("do homework", DateTime.Today.AddDays(2), 7, Type.NORMAL);
        cal.addTask(t2);
        Task t3 = new Task("make lunch", DateTime.Today.AddDays(2), 2, Type.NORMAL);
        cal.addTask(t3);
        Task t4 = new Task("go to the mall", DateTime.Today.AddDays(4), 4, Type.NORMAL);
        cal.addTask(t4);
        // will throw error since there is no space while reordering
        Task t5 = new Task("fix bike", DateTime.Today.AddDays(2), 6, Type.NORMAL);
        cal.addTask(t5);

        assert(current.Tasks.Count == 2);
        assert(current.NextDay.Tasks.Count == 2);
        assert(current.NextDay.NextDay.Tasks.Count == 3);
    }

    [STAThread]
    static void Main()
    {
        try
        {
            test();
            WriteLine($"{totalPassed}/{totalTests} Passed!\n" +
                $"{totalTests - totalPassed} remain unexecuted!\n");
        }
        catch(Exception)
        {
            WriteLine($"{totalPassed}/{totalTests} Passed!\n" +
                $"{totalTests - totalPassed} remain unexecuted!\n" +
                $"Failed at assert number: {totalPassed + 1}");
        }
        Settings.Load();
        if (Environment.OSVersion.Version.Major >= 6)
            SetProcessDPIAware();

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Scheduler.Forms.CalendarView());
    }

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();
}