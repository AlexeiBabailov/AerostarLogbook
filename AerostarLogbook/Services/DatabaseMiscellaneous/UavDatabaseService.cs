using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AerostarLogbook.Models;
using AerostarLogbook.Services;
using AerostarLogbook.Services.DatabaseMain;
using SQLite;

namespace AerostarLogbook.Services.DatabaseMiscellaneous
{
    public static class UavDatabaseService
    {

        public static async Task<ObservableCollection<UavModel>> GetUavsAsync()
        {
            await DatabaseConnection.Init();  // Make sure everything is initialized.
            ObservableCollection<UavModel> temp = new();

            var tableNameAttribute = typeof(UavModel).GetCustomAttribute<TableNameAttribute>();
            var tableName = tableNameAttribute != null ? tableNameAttribute.Name : typeof(UavModel).Name;

            var query = $"SELECT * FROM {tableName}";

            // Ensure that database connection is initialized
            var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            // Execute the query and retrieve the list of items
            List<UavModel> tempList = await database.QueryAsync<UavModel>(query);

            foreach (var item in tempList)
            {
                temp.Add(item);
            }
            return temp;
        }

     
        public static async Task AddUavAsync<T>(T item) where T : new()
        {
            await DatabaseService.InsertServiceAsync<T>(item);
        }

        public static async Task DeleteUavAsync<T>(T item) where T : new()
        {
            // Ensure that the database connection is initialized
            await DatabaseConnection.Init();

            // Create a new instance of SQLiteAsyncConnection
            var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            // Get the table name using the helper function
            string tableName = DatabaseService.GetTableName<T>();

            // TailNum is unique per item
            var name = $"TailNum";

            // Get the serial number value from the item
            var nameProperty = typeof(T).GetProperty(name);
            if (nameProperty == null)
            {
                throw new InvalidOperationException($"Property '{name}' not found on type {typeof(T).Name}.");
            }
            var nameValue = nameProperty.GetValue(item);
            if (nameValue == null)
            {
                throw new InvalidOperationException($"Name cannot be null for deletion.");
            }
            var safeNameValue = nameValue.ToString().Replace("'", "''");

            // Construct SQL Delete Statement using the safe value
            string deleteQuery = $"DELETE FROM {tableName} WHERE TailNum = '{safeNameValue}'";

            try
            {
                await database.ExecuteAsync(deleteQuery);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing delete: {ex.Message}");
            }
        }
      
    }
}
