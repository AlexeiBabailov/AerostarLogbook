using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AerostarLogbook.Resources.Converters
{
    public class IntToBoolConverter
    {

        public static bool ConvertIntToBool(int value)
        {
            try
            {
                if (value.GetType() == typeof(int) && value != null)
                {
                    if (value > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch 
            {
                return false;
            }
        }

        public static int ConvertBoolToInt(bool value)
        {
            try
            {
                if (value.GetType() == typeof(bool) && value != null)
                {
                    return value ? 1 : 0;          
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
