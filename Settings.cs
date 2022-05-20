using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public delegate void loadFile();

public static class Settings
{
    public static Calendar MyCalendar { get; set; }

    public static event loadFile newFileLoaded;

    public static void Load(string path = "data.bin")
    {
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                ProgramData data = (ProgramData)formatter.Deserialize(stream);
                Settings.MyCalendar = data.loadCalendar();
                newFileLoaded?.Invoke();
            }
            catch (Exception)
            {
                Settings.MyCalendar = new Calendar();
                Settings.MyCalendar.DefaultWorkingHoursInterval = (9, 17);
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
        }
    }

    public static void Save(string path = "data.bin")
    {
        ProgramData data = new ProgramData(Settings.MyCalendar);
        FileStream stream = new FileStream(path, FileMode.Create);

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
        }
        catch (Exception)
        {
            Console.WriteLine("Problem while saving file");
        }
        finally
        {
            stream.Close();
        }
    }
}