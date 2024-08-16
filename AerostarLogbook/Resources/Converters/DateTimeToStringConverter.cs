using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AerostarLogbook.Resources.Converters
{
    public class DateTimeToStringConverter
    {
        public static string ConvertDateToString(DateTime date)
        {
            try
            {
                return date.ToString("dd-MM-yyyy");
            }
            catch
            {

                throw new FormatException("Invalid time format. Expected format is dd-MM-yyyy");
            }
        }

        public static DateTime ConvertStringToDate(string dateString)
        {
            try
            {
                DateTime.TryParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                return date;
            }
            catch
            {
                throw new FormatException("Invalid time format. Expected format is dd-MM-yyyy");
            }
        }
    }
}
