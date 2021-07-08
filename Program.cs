using System;
using System.Collections.Generic;
using System.Windows.Forms;
static class Program
{
    [STAThread]
    static void Main()
    {
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
