using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AerostarLogbook.Models;

namespace AerostarLogbook.Services.DatabaseMain
{
    internal static class TeamMembersDatabaseService
    {
        public static async Task<ObservableCollection<string>> GetMembersAsync<T>() where T : new()
        {
            ObservableCollection<string> members = new ();

            // Retrieve the table name using the TableNameAttribute or default to the type name
            var tableNameAttribute = typeof(T).GetCustomAttribute<TableNameAttribute>();
            var tableName = tableNameAttribute != null ? tableNameAttribute.Name : typeof(T).Name;

            var query = $"SELECT * FROM {tableName}";


           
                // Initialize the database connection
                var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                await DatabaseConnection.Init();  // Ensure the database is initialized

                // Execute the query and get the result
              var results = await database.QueryAsync<T>(query);
                foreach (var result in results)
                {
                    if (result != null)
                    {
                        // Use reflection to access the "Name" property
                        var nameProperty = result.GetType().GetProperty("Name");
                        if (nameProperty != null)
                        {
                            var nameValue = nameProperty.GetValue(result);
                            if (nameValue != null)
                            {
                                members.Add(nameValue.ToString());
                            }
                        }
                    }
                }

            return members;
        }

        public static async Task AddMemberAsync<T>(T item) where T : new()
        {
            await DatabaseService.InsertServiceAsync<T>(item);

        }

        public static async Task DeleteMemberAsync<T>(T item) where T : new()
        {
            // Ensure that the database connection is initialized
            await DatabaseConnection.Init();

            // Create a new instance of SQLiteAsyncConnection
            var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            // Get the table name using the helper function
            string tableName = DatabaseService.GetTableName<T>();

            // Define the serial number property name (assumes serial number is unique per item)
            var name = $"Name";

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
            string deleteQuery = $"DELETE FROM {tableName} WHERE Name = '{safeNameValue}'";

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
