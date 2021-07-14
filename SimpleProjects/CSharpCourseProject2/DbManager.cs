using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace CSharpCourseProject2
{
    static class DbManager
    {
        private static readonly string DbCreationScript = @"
CREATE TABLE Car(Id INTEGER NOT NULL PRIMARY KEY,
                 Manufacturer TEXT NOT NULL DEFAULT ('BMW') CHECK(LENGTH(Manufacturer) > 0),
                 PlateNumber TEXT UNIQUE NOT NULL DEFAULT ('PN ' || CURRENT_TIMESTAMP) CHECK(LENGTH(PlateNumber) > 0),
                 Model TEXT NOT NULL DEFAULT ('M3') CHECK(LENGTH(Model) > 0),
                 EngineCCs INTEGER NOT NULL CHECK(EngineCCs > 0) DEFAULT (3500),
                 GearsCount INTEGER NOT NULL CHECK(GearsCount > 0) DEFAULT (6)
                );
CREATE TABLE Employee(Id INTEGER NOT NULL PRIMARY KEY,
                      Username TEXT NOT NULL UNIQUE CHECK(LENGTH(Username) > 0),
                      Password TEXT NOT NULL CHECK(LENGTH(Password) > 0)
                     );
CREATE TABLE Customer(Id INTEGER NOT NULL PRIMARY KEY,
                      Email TEXT NOT NULL CHECK(Email LIKE '%@%') DEFAULT (CURRENT_TIMESTAMP || '@Email.com'),
                      FirstName TEXT NOT NULL CHECK(LENGTH(FirstName) > 0) DEFAULT ('FNAME ' || CURRENT_TIMESTAMP),
                      LastName TEXT NOT NULL CHECK(LENGTH(LastName) > 0) DEFAULT ('LNAME ' || CURRENT_TIMESTAMP),
                      Age INTEGER NOT NULL CHECK(Age >= 18) DEFAULT 18,
                      LicenseNumber TEXT NOT NULL UNIQUE CHECK(LENGTH(LicenseNumber) > 0) DEFAULT ('LN' || CURRENT_TIMESTAMP)
                      );
CREATE TABLE RentalContract(Id INTEGER NOT NULL PRIMARY KEY,
                            CarId INTEGER NOT NULL REFERENCES Car(Id),
                            EmployeeId INTEGER NOT NULL REFERENCES Employee(Id),
                            CustomerId INTEGER NOT NULL REFERENCES Customer(Id),
                            StartTime INTEGER NOT NULL DEFAULT 637322345522006942 CHECK(StartTime < EndTime),
                            EndTime INTEGER NOT NULL DEFAULT 637322345522009000 CHECK(StartTime < EndTime),
                            HourlyRate REAL NOT NULL CHECK(HourlyRate >= 0) DEFAULT 10
                            );

INSERT INTO Employee VALUES (1, 'abd', '123'), (2, 'mans', '123');
INSERT INTO Customer VALUES (1, 'abd@psut.com', 'Abdallah', 'Darwish', 21, '20170112'), (2, 'mans@outlook.com', 'Abdallah', 'Mansour', 21, '987654321');
INSERT INTO Car VALUES (1, 'Hyundai', '123456789', 'STAREX', 505, 6), (2, 'Mitsubishi', '987654321', 'Lancer', 1800, 7);
INSERT INTO RentalContract VALUES (1, 1, 1, 1, 637322345522006942, 637322395522010000, 10), (2, 2, 2, 2, 637322145522006942, 637322545522006942, 100);
";
        private static readonly string DbPath = Path.Combine(Application.StartupPath, "db.sqlite");
        private static readonly string ConnectionString = $"Data Source={DbPath};Version=3;";
        public static SQLiteConnection GetConnection()
        {
            var con = new SQLiteConnection(ConnectionString);
            con.Open();
            using (var com = new SQLiteCommand(@"PRAGMA foreign_keys = ON", con))
            {
                com.ExecuteNonQuery();
            }
            return con;
        }
        static DbManager()
        {
            if (File.Exists(DbPath) == false)
            {
                SQLiteConnection.CreateFile(DbPath);
                using (var con = GetConnection())
                using (var com = new SQLiteCommand(DbCreationScript, con))
                {
                    com.ExecuteNonQuery();
                }
            }
        }
    }
}
