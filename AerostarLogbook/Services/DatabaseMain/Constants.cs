using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Services.DatabaseMain
{
    public static class Constants
    {
        public const string DatabaseFilename = "LogbookDatabaseFinal.db";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                // Determine if the platform is Android or Windows
                if (OperatingSystem.IsAndroid())
                { 


                    return Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
                }
                else if (OperatingSystem.IsWindows())
                {
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DatabaseFilename);
                    //string projectPath = System.AppDomain.CurrentDomain.BaseDirectory;
                    //string localFolderPath = Directory.GetParent(projectPath).Parent.Parent.Parent.Parent.Parent.FullName;
                    //string myResourcesPath = Path.Combine(localFolderPath, "Resources");
                    //string myResource_RawFolder = Path.Combine(myResourcesPath, "Raw");

                    //return Path.Combine(myResource_RawFolder, DatabaseFilename);

                }

                throw new PlatformNotSupportedException("Unsupported platform");
            }
        }

        //public static string DatabasePath =>
        //    Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}
