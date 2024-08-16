using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AerostarLogbook.Models;
using Microsoft.Maui.Controls.PlatformConfiguration;
using SQLite;
using System.Reflection;

namespace AerostarLogbook.Services.DatabaseMain
{
    public static class DatabaseConnection
    {
        public static SQLiteAsyncConnection Database { get; set; }

        public static async Task Init()
        {

            if (Database is not null)
                return;

            if (!File.Exists(Constants.DatabasePath))
            {
                if (OperatingSystem.IsAndroid())
                {
                    // For Android, copy from app package
                    using (var stream = await FileSystem.OpenAppPackageFileAsync(Constants.DatabaseFilename))
                    using (var fileStream = new FileStream(Constants.DatabasePath, FileMode.Create, FileAccess.Write))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
                else if (OperatingSystem.IsWindows())
                {
                    var databaseFilename = Constants.DatabaseFilename;
                    var databasePath = Path.Combine(FileSystem.AppDataDirectory, Constants.DatabaseFilename);
                    using (var assetStream = await FileSystem.OpenAppPackageFileAsync(databaseFilename))
                    {
                        // Create a new file stream for copying the database file
                        using (var localFileStream = new FileStream(Constants.DatabasePath, FileMode.CreateNew, FileAccess.Write))
                        {
                            await assetStream.CopyToAsync(localFileStream);
                        }
                    }
                }
                else
                {
                    throw new PlatformNotSupportedException("Unsupported platform");
                }
            }

            // Now you can initialize your database connection
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }






        //static DatabaseConnection()
        //{

        //    if (DeviceInfo.Platform == DevicePlatform.Android)
        //    {
        //        databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AerostarLogbookDatabase.sql");
        //        if (!File.Exists(databasePath))
        //        {
        //            CopyDatabaseFromResources(databasePath);
        //        }
        //    }
        //    else if (DeviceInfo.Platform == DevicePlatform.WinUI)
        //    {
        //        string projectPath = AppDomain.CurrentDomain.BaseDirectory;
        //        string databaseFileName = "mydatabase.db";
        //        string localFolderPath = Path.Combine(Directory.GetParent(projectPath).FullName, "Resources", "Raw");
        //        databasePath = Path.Combine(localFolderPath, databaseFileName);
        //    }
        //    else
        //    {
        //        throw new NotImplementedException("Platform not supported");
        //    }

        //    database = new SQLiteConnection(databasePath);
        //    InitializeDatabase();
        //}



        //public static void InitializeDatabase()
        //{
        //    // This will automatically create the table if it doesn't exist
        //    database.CreateTable<UAV>();
        //}

        //public class UAV
        //{
        //    [PrimaryKey, AutoIncrement]
        //    public int Id { get; set; }

        //    [NotNull]
        //    public string Name { get; set; }
        //    // Add other properties as needed
        //}

        //public static void InsertUAV(UAV uav)
        //{
        //    database.Insert(uav);
        //}

        //public static void UpdateUAV(UAV uav)
        //{
        //    database.Update(uav);
        //}

        //public static void DeleteUAV(UAV uav)
        //{
        //    database.Delete(uav);
        //}

        //public static UAV GetUAV(int id)
        //{
        //    return database.Get<UAV>(id);
        //}

        //public static IEnumerable<UAV> GetAllUAVs()
        //{
        //    return database.Table<UAV>().ToList();
        //}
    }
}
