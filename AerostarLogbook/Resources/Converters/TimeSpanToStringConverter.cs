using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Resources.Converters
{
    public class TimeSpanToStringConverter 
    {
        
        public static string ConvertTimeSpanToString(TimeSpan timeSpan)
        {
            // Calculate total hours and minutes, ignoring days
            int totalHours = timeSpan.Hours + (timeSpan.Days * 24);
            int totalMinutes = timeSpan.Minutes;

            // Format the result to "HH-mm", considering that total hours can exceed 24
            return $"{totalHours:00}:{totalMinutes:00}:00";

        }

        public static TimeSpan ConvertStringToTimeSpan(string timeString)
        {
            try 
            {
                string[] parts = timeString.Split(':');

                int h = int.Parse(parts[0]);
                int m = int.Parse(parts[1]);
                int s = int.Parse(parts[2]);

                TimeSpan timeSpan = new(h, m, s);
                return timeSpan;
            }
            catch
            {
                throw new FormatException("Invalid time format. Expected format is HH:mm:ss.");
            }
        }
    }
}
