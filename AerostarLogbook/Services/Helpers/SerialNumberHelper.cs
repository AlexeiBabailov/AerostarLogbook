using System;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AerostarLogbook.Models;
using AerostarLogbook.Services.DatabaseMain;

namespace AerostarLogbook.Services.Helpers
{
    internal class SerialNumberHelper
    {
        public static async Task<int> GetMaxSerialNumberAsync(string tableName)
        {
            await DatabaseConnection.Init();
            string serialNumberColumn = tableName + "SN";

            var query = $"SELECT MAX({serialNumberColumn}) FROM {tableName}";

            var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            // Execute the query to get the maximum serial number
            var maxSerialNumber = await database.ExecuteScalarAsync<int?>(query);

            return maxSerialNumber ?? 0;  // Return 0 if null (i.e., no entries in the table)
        }
    }
}
