using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AerostarLogbook.Models;
using AerostarLogbook.Resources.Converters;
using AerostarLogbook.Services.DatabaseMain;
using AerostarLogbook.Views;
using SQLite;

namespace AerostarLogbook.Services.DatabaseMiscellaneous
{
    public static class EngineDatabaseService
    {

        public static ObservableCollection<EngineModel> engines = new();

        public static async Task<ObservableCollection<EngineModel>> GetEnginesFromDB()
        {
            await DatabaseConnection.Init();  // Make sure everything is initialized.

            ObservableCollection<EngineModel> temp = new();

            string tableName = "Engines";

            var query = $"SELECT * FROM {tableName}";

            // Ensure that database connection is initialized
            var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            // Execute the query with a parameter for safety
            List<EngineModel> tempList = await database.QueryAsync<EngineModel>(query);

            foreach (var item in tempList)
            {
                temp.Insert(0, item);
            }
            return temp;
        }

        public static async Task AddEngineAsync<T>(T item) where T : new()
        {
            await DatabaseService.InsertServiceAsync<T>(item);
        }

        /// <summary>
        /// currently not in use
        /// </summary>
        //public static async Task DeleteEngineAsync<T>(T item) where T : new()
        //{
        //    // Ensure that the database connection is initialized
        //    await DatabaseConnection.Init();

        //    // Create a new instance of SQLiteAsyncConnection
        //    var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

        //    // Get the table name using the helper function
        //    string tableName = DatabaseService.GetTableName<T>();

        //    // TailNum is unique per item
        //    var name = $"Engine";

        //    // Get the serial number value from the item
        //    var nameProperty = typeof(T).GetProperty(name);
        //    if (nameProperty == null)
        //    {
        //        throw new InvalidOperationException($"Property '{name}' not found on type {typeof(T).Name}.");
        //    }
        //    var nameValue = nameProperty.GetValue(item);
        //    if (nameValue == null)
        //    {
        //        throw new InvalidOperationException($"Name cannot be null for deletion.");
        //    }
        //    var safeNameValue = nameValue.ToString().Replace("'", "''");

        //    // Construct SQL Delete Statement using the safe value
        //    string deleteQuery = $"DELETE FROM {tableName} WHERE Engine = '{safeNameValue}'";

        //    try
        //    {
        //        await database.ExecuteAsync(deleteQuery);

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error executing delete: {ex.Message}");
        //    }
        //}


        public static async Task RecalculateEnginesHours()
        {
            List<FlightActivityModel> allFlights = new List<FlightActivityModel>();

            await DatabaseConnection.Init();
            // Assuming 'DatabasePath' is a string containing the path to your SQLite database
            var database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            string tableName = "Flights";

            // Update the query to filter by 'tail'
            var query = $"SELECT * FROM {tableName}";

            // Execute the query with a parameter for safety
            allFlights = await database.QueryAsync<FlightActivityModel>(query);

            //Sorting all flights by Date and Startup time
            var sortedFlights = allFlights.Select(item => new
            {
                OriginalItem = item,
                CombinedDateTime = DateTime.ParseExact(item.FlightDate + " " + item.Startup, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture)
            })
            .OrderBy(x => x.CombinedDateTime)
            .Select(x => x.OriginalItem)
            .ToList();
            allFlights.Clear();
            foreach (var sortedItem in sortedFlights)
            {
                allFlights.Add(sortedItem);
            }
            if (engines != null)
            {
                engines.Clear();
            }
            engines = await GetEnginesFromDB();

            foreach (EngineModel engine in engines)
            {
                engine.Hours = engine.InitialHours;
            }

            foreach (FlightActivityModel flight in allFlights)
            {
                TimeSpan startupTime = TimeSpanToStringConverter.ConvertStringToTimeSpan(flight.Startup);
                TimeSpan shutdownTime = TimeSpanToStringConverter.ConvertStringToTimeSpan(flight.Shutdown);
                TimeSpan flightDuration;
                if (shutdownTime > startupTime)
                {
                    flightDuration = shutdownTime - startupTime;
                }
                else
                {
                    flightDuration = TimeSpan.FromDays(1) + shutdownTime - startupTime;
                }

                var engine = engines.FirstOrDefault(e => e.Engine == flight.EngineSN);
                if (engine != null)
                {
                    // Parse engine hours to TimeSpan for calculations
                    TimeSpan currentEngineHours = TimeSpanToStringConverter.ConvertStringToTimeSpan(engine.Hours);

                    // Update the flight record with the current engine hours
                    flight.EngineHours = TimeSpanToStringConverter.ConvertTimeSpanToString(currentEngineHours);

                    // Calculate new engine hours
                    TimeSpan newEngineHours = currentEngineHours + flightDuration;
                    flight.EngineHoursAfterFlight = TimeSpanToStringConverter.ConvertTimeSpanToString(newEngineHours);

                    // Format back to string before updating the DB

                    engine.Hours = TimeSpanToStringConverter.ConvertTimeSpanToString(newEngineHours);

                    // Update the engine in the database
                    //UpdateEngineInDB(engine);
                }
            }
            foreach (FlightActivityModel flight in allFlights)
            {
                await DatabaseService.UpdateServiceAsync<FlightActivityModel>(flight);
            }

            string s = "check";


        }



    }
}
