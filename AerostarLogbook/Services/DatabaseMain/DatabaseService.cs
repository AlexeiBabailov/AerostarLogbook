using AerostarLogbook.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Reflection;
using AerostarLogbook.Models.TeamMembers;


namespace AerostarLogbook.Services.DatabaseMain
{
    public static class DatabaseService
    {

        public static async Task<ObservableCollection<T>> GetItemsByTailAsync<T>(string tail) where T : class, new()
        {
            await DatabaseConnection.Init();  // Make sure everything is initialized.
            
            ObservableCollection<T> temp = new ();
            
            string tableName = GetTableName<T>();

            // Update the query to filter by 'tail'
            var query = $"SELECT * FROM {tableName} WHERE Tail = ?";

            // Ensure that database connection is initialized
            var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            // Execute the query with a parameter for safety
            List<T> tempList = await database.QueryAsync<T>(query, tail);

            foreach (var item in tempList)
            {
                temp.Insert(0,item);
            }
            return temp;
        }

        public static async Task InsertServiceAsync<T>(T item) where T : new()
        {
            // Ensure that the database connection is initialized
            await DatabaseConnection.Init();

            // Create a new instance of SQLiteAsyncConnection
            var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            // Get the table name using the helper function
            string tableName = GetTableName<T>();

            // Extract property names and values for SQL parameters
            var properties = typeof(T).GetProperties();
            var columnNames = properties.Select(p => p.Name).ToList();
            var paramNames = properties.Select(p => $"@{p.Name}").ToList();
            var parameters = new List<object>();

            // Construct SQL Insert Statement
            string insertQuery = $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", paramNames)})";

            // Fill parameters list with actual values from the item
            foreach (var property in properties)
            {
                object value = property.GetValue(item);
                parameters.Add(value ?? DBNull.Value);
            }

            // Execute the insert command with parameters
            await database.ExecuteAsync(insertQuery, parameters.ToArray());
        }

        public static async Task DeleteServiceAsync<T>(T item) where T : new()
        {
            // Ensure that the database connection is initialized
            await DatabaseConnection.Init();

            // Create a new instance of SQLiteAsyncConnection
            var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            // Get the table name using the helper function
            string tableName = GetTableName<T>();

            // Define the serial number property name (assumes serial number is unique per item)
            var serialNumberPropertyName = $"{tableName}SN";

            // Get the serial number value from the item
            var serialNumberProperty = typeof(T).GetProperty(serialNumberPropertyName);
            if (serialNumberProperty == null)
            {
                throw new InvalidOperationException($"Property '{serialNumberPropertyName}' not found on type {typeof(T).Name}.");
            }
            var serialNumberValue = serialNumberProperty.GetValue(item);
            if (serialNumberValue == null)
            {
                throw new InvalidOperationException($"Serial number cannot be null for deletion.");
            }

            // Construct SQL Delete Statement
            string deleteQuery = $"DELETE FROM {tableName} WHERE {serialNumberPropertyName} = @SerialNumber";

            try
            {
                int parameter = (int)serialNumberValue ;
                await database.ExecuteAsync(deleteQuery, parameter);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing delete: {ex.Message}");
            }
        }

        public static async Task UpdateServiceAsync<T>(T item) where T : new()
        {
            // Ensure that the database connection is initialized
            await DatabaseConnection.Init();

            // Create a new instance of SQLiteAsyncConnection
            var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            // Get the table name using the helper function
            string tableName = GetTableName<T>();

            // Define the serial number property name (assumes serial number is unique per item)
            var serialNumberPropertyName = $"{tableName}SN";

            // Extract property names and values for SQL parameters
            var properties = typeof(T).GetProperties();
            var parameters = new List<object>();
            var setClauses = new List<string>();

            foreach (var property in properties)
            {
                string name = property.Name;
                object value = property.GetValue(item) ?? DBNull.Value;
                if (name != serialNumberPropertyName)  // Exclude the serial number from the SET clauses
                {
                    setClauses.Add($"{name} = @{name}");
                    parameters.Add(value);
                }
            }

            // Get the serial number value for the WHERE clause
            var serialNumberProperty = typeof(T).GetProperty(serialNumberPropertyName);
            if (serialNumberProperty == null)
            {
                throw new InvalidOperationException($"Property '{serialNumberPropertyName}' not found on type {typeof(T).Name}.");
            }
            object serialNumberValue = serialNumberProperty.GetValue(item);
            if (serialNumberValue == null)
            {
                throw new InvalidOperationException("Serial number cannot be null for updating.");
            }

            // Construct SQL Update Statement
            string updateQuery = $"UPDATE {tableName} SET {string.Join(", ", setClauses)} WHERE {serialNumberPropertyName} = @SerialNumber";

            // Add serial number to parameters at the end
            parameters.Add(serialNumberValue);


            try
            {
                await database.ExecuteAsync(updateQuery, parameters.ToArray());

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing delete: {ex.Message}");
            }
        }

        public static string GetTableName<T>()
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var tableNameAttribute = typeInfo.GetCustomAttribute<TableNameAttribute>();
            return tableNameAttribute != null ? tableNameAttribute.Name : typeof(T).Name;
        }
    }
}
