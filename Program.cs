using System;
using System.Management;

public class MonitorBrightnessController
{
    public static void SetMonitorBrightness(int deltaBrightness)
    {
        try
        {
            // Get the current brightness level
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM WmiMonitorBrightness");
            ManagementObjectCollection brightnessCollection = searcher.Get();
            byte currentBrightness = 0;
            foreach (ManagementObject mObj in brightnessCollection)
            {
                currentBrightness = (byte)mObj["CurrentBrightness"];
                break;
            }

            // Calculate the new brightness level
            int newBrightness = currentBrightness + deltaBrightness;

            // Ensure the new brightness is within valid range (0 to 100)
            newBrightness = Math.Max(0, Math.Min(100, newBrightness));

            // Set the brightness using the WmiSetBrightness method
            ManagementObjectSearcher methodSearcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM WmiMonitorBrightnessMethods");
            ManagementObjectCollection methodCollection = methodSearcher.Get();
            foreach (ManagementObject mObj in methodCollection)
            {
                mObj.InvokeMethod("WmiSetBrightness", new object[] { (UInt32)1, (byte)newBrightness });
            }

            Console.WriteLine($"Brightness set to {newBrightness}%.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting brightness: {ex.Message}");
        }
    }

    public static void Main(string[] args)
    {
        int deltaBrightness = 0;
        if (args.Length > 0 && int.TryParse(args[0], out deltaBrightness))
        {
            SetMonitorBrightness(deltaBrightness);
        }
        else
        {
            Console.WriteLine("Invalid or missing delta brightness value.");
        }
    }
}
