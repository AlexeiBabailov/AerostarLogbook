using AerostarLogbook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace AerostarLogbook.Services.Helpers
{
    public static class ReflectionHelper
    {
        public static string GetTableName<T>()
        {
            // Get the type of the class
            Type type = typeof(T);

            // Retrieve the TableName attribute from the type
            var tableNameAttribute = type.GetCustomAttribute<TableNameAttribute>();

            // Return the name specified in the TableName attribute, or null if not found
            if (tableNameAttribute != null)
            {
                return tableNameAttribute?.Name;
            }
            else
            {
                return "Default Table Name";
            }

        }
    }
}
