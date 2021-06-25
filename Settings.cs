using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Settings
{
    public static Calendar MyCalendar { get; set; }

    public static void Load()
    {
        if (File.Exists("data.bin"))
        {
            FileStream stream = new FileStream("data.bin", FileMode.Open);

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                ProgramData data = (ProgramData)formatter.Deserialize(stream);
                Settings.MyCalendar = data.MyCalendar;
            }
            catch (System.Exception)
            {
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

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
        }
        catch(System.Exception)
        {
        }
        finally
        {
            stream.Close();
        }
    }
}