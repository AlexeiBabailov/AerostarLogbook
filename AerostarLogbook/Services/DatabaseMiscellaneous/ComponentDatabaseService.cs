using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AerostarLogbook.Models;
using SQLite;
using AerostarLogbook.Services.DatabaseMain;

namespace AerostarLogbook.Services.DatabaseMiscellaneous
{
    // Unique Database Service for components - requires different behavior of object and lists
    public static class ComponentDatabaseService
    {
        public static async Task<ObservableCollection<ComponentModel>> GetComponentsFromDBAsync()
        {
            await DatabaseConnection.Init();  // Make sure everything is initialized.

            ObservableCollection<ComponentModel> components = new();

            string tableName = "Components";

            var query = $"SELECT * FROM {tableName}";

            // Ensure that database connection is initialized
            var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            // Execute the query with a parameter for safety
            List<ComponentModel> tempList = await database.QueryAsync<ComponentModel>(query);

            foreach (var item in tempList)
            {
                components.Insert(0, item);
            }
            return components;
        }

        public static async Task AddComponentAsync(ComponentModel component)
        {

            // Create a new instance of SQLiteAsyncConnection
            var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            // Define the table name
            string tableName = "Components";

            // Prepare column names and parameter placeholders
            var columnNames = new List<string>() { "Type", "SN", "Tail" };
            var paramNames = new List<string>() { "@Type", "@SN", "@Tail" };


            // Construct the SQL Insert Statement
            string insertQuery = $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", paramNames)})";


            // Create a dynamic object to hold the parameters
            var parameters = new List<string>() { component.Type, component.SN, string.Empty };
           

            // Execute the insert command with parameters
            await database.ExecuteAsync(insertQuery, parameters.ToArray());
        }
    

        public static async Task DeleteComponentAsync<T>(T item) where T : new()
        {
            // Ensure that the database connection is initialized
            await DatabaseConnection.Init();

            // Create a new instance of SQLiteAsyncConnection
            var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            // Get the table name using the helper function
            string tableName = DatabaseService.GetTableName<T>();

            // TailNum is unique per item
            var name = $"SN";

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
            string deleteQuery = $"DELETE FROM {tableName} WHERE SN = '{safeNameValue}'";

            try
            {
                await database.ExecuteAsync(deleteQuery);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing delete: {ex.Message}");
            }
        }

        public static async Task<ObservableCollection<ComponentModel>> GetComponentSNByTypeAsync(string type)
        {
            ObservableCollection<ComponentModel> allComponents = await GetComponentsFromDBAsync();
            // Filter the components based on the type and convert the result to a list
            var filteredComponents = allComponents.Where(c => c.Type.Equals(type)).ToList();

            // Convert the list of filtered components back into an ObservableCollection
            return new ObservableCollection<ComponentModel>(filteredComponents);
        }

        public static async Task<ObservableCollection<string>> GetComponentTypesAsync()
        {
            ObservableCollection<ComponentModel> allComponents = await GetComponentsFromDBAsync();
            
            var distinctTypes = allComponents.Select(c => c.Type).Distinct().ToList();

            // Convert the list of strings to an ObservableCollection
            ObservableCollection<string> types = new ObservableCollection<string>(distinctTypes);

            return types;
        }
    }
}
