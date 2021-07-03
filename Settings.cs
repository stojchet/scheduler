using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class Settings
{
    public static Calendar MyCalendar { get; set; }

    public static void Load()
    {
        if (File.Exists("data.bin"))
        {
            FileStream stream = new FileStream("data.bin", FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                ProgramData data = (ProgramData)formatter.Deserialize(stream);
                Settings.MyCalendar = data.MyCalendar;
                Settings.MyCalendar.currentDate = DateTime.Now;
            }
            catch (Exception)
            {
                Settings.MyCalendar = new Calendar();
                Settings.MyCalendar.defaultWorkingHours = 12;
                Console.WriteLine("Error: Cannot properly parse data!");
            }
            finally
            {
                stream.Close();
            } 
        }
        else
        {
            Settings.MyCalendar = new Calendar();
            Settings.MyCalendar.defaultWorkingHours = 12;
        }
    }

    public static void Save()
    {
        ProgramData data = new ProgramData();
        data.MyCalendar = Settings.MyCalendar;
        FileStream stream = new FileStream("data.bin", FileMode.Create);

        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, data);
        stream.Close();
    }
}