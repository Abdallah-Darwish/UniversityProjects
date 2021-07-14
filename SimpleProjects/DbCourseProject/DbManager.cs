﻿using Dapper;
using Npgsql;
using System.IO;
using System.Threading.Tasks;
namespace DbProject
{
    public static class DatabaseManager
    {
        private const string PostgresUserId = "postgres", PostgressPassword = "password";
        public static readonly string[] TablesNames = { "Airport", "Flight", "FlightLeg", "FlightLegInstance", "FlightFare", "AirplaneType", "Airplane", "AirplaneTypeLanding", "SeatReservation" };
        public const string DbName = "airlines";
        private const string DataPopulationScript = @"
INSERT INTO Airport (airportcode, name, city, state)
VALUES
	('ALIA', 'Quuen Alia International Airport', 'Amman', 'Jordan'),
	('FAHD', 'King Abdulaziz International Airport', 'Dammam', 'KSA'),
	('CAIRO', 'Cairo International Airport', 'Cairo', 'Egypt'),
	('ABU_DHABI', 'Abu Dhabi International Airport', 'Abu Dhabi', 'UAE'),
	('HELSINKI', 'Helsinki-Vantaa Airport', 'Helsinki', 'Finland');

INSERT INTO AirplaneType (TypeName, Company, MaxSeats)
VALUES
	('Airbus_A220', 'Airbus', 220),
	('Airbus_A350', 'Airbus', 310),
	('Airbus_A380', 'Airbus', 350),
	('Boeing_747', 'Boeing', 550),
	('Boeing_787', 'Boeing', 321);

INSERT INTO AirplaneTypeLanding (typename, airportcode)
VALUES
	('Airbus_A220', 'ALIA'),
	('Airbus_A350', 'ALIA'),
	('Airbus_A380', 'FAHD'),
	('Boeing_747', 'CAIRO'),
	('Boeing_787', 'ABU_DHABI'),
	('Airbus_A380', 'HELSINKI'),
	('Boeing_747', 'HELSINKI');
  
INSERT INTO Airplane (AirplaneId, TotalSeats, TypeName)
VALUES
	('RJ_PLANE', 200, 'Airbus_A220'),
	('Pegasus_PLANE', 305, 'Airbus_A350'),
	('TUR_PLANE', 342, 'Airbus_A380'),
	('KU_PLANE', 533, 'Boeing_747'),
	('QAT_PLANE', 219, 'Boeing_787');

INSERT INTO Flight (FlightNumber, Airline, Weekdays)
VALUES
	('AC01', 'Royal Jordanian', 3),
	('DC99', 'Commercial Saudi', 7),
	('AH78', 'Fly NAS', 4),
	('DA73', 'Turkish Airlines', 9),
	('AA47', 'Royal Jordanian', 15);

INSERT INTO FlightFare (FareCode, Amount, FlightNumber, Restrictions)
VALUES
	('business', 500.5, 'AA47', 'Above 15 years only !'),
	('economy', 350, 'AA47', 'Only 15kg lagguage'),
	('super_saver', 200, 'AA47', NULL),
	('economy', 410, 'DC99', NULL),
	('super_saver', 150.75, 'DC99',  'Transit passengers'),
	('business', 500.5, 'AC01', NULL),
	('economy', 500.5, 'AH78', 'Citizens only'),
	('super_saver', 111.112, 'DA73', 'NO LAGGUAGE');

INSERT INTO FlightLeg (FlightLegNumber, DepartureAirportCode, ArrivalAirportCode, ScheduledDepartureTime, ScheduledArrivalTime, FlightNumber)
VALUES
	(1, 'ALIA', 'ABU_DHABI', '2019-12-01 05:03:01', '2019-12-02 07:00:05', 'AC01'),
	(2, 'ABU_DHABI', 'CAIRO', '2019-12-02 05:03:01', '2019-12-03 07:00:05', 'AC01'),
	(1, 'FAHD', 'HELSINKI', '2019-12-10 05:03:01', '2019-12-11 08:12:020', 'DC99'),
	(2, 'HELSINKI', 'CAIRO', '2019-12-11 05:03:01', '2019-12-13 07:00:05', 'DC99'),
	(1, 'HELSINKI', 'ALIA', '2019-12-17 05:03:01', '2019-12-18 07:00:05', 'AH78'),
	(1, 'FAHD', 'ABU_DHABI', '2019-12-23 05:03:01', '2019-12-24 07:00:05', 'DA73'),
	(1, 'ALIA', 'FAHD', '2019-12-11 05:03:01', '2019-12-12 07:00:05', 'AA47');
	
INSERT INTO FlightLegInstance (FlightLegInstanceDate, AvailableSeats, AirplaneId, DepartureAirportCode, ArrivalAirportCode, FlightNumber, FlightLegNumber, DepartureTime, ArrivalTime)
VALUES
	('2019-12-25', 55, 'RJ_PLANE', 'HELSINKI', 'ABU_DHABI', 'AC01', 1, '2019-12-01 05:03:01', '2019-12-02 07:00:05'),
	('2019-12-26', 66, 'Pegasus_PLANE', 'ALIA', 'CAIRO', 'AC01', 2, '2019-12-11 05:03:01', '2019-12-13 07:00:05'),
	('2019-12-27', 77, 'TUR_PLANE', 'HELSINKI', 'CAIRO', 'DC99', 1, '2019-12-01 05:03:01', '2019-12-02 07:00:05'),
	('2019-12-28', 88, 'QAT_PLANE', 'ALIA', 'HELSINKI', 'DC99', 2, '2019-12-11 05:03:01', '2019-12-13 07:00:05'),
	('2019-12-29', 99, 'RJ_PLANE', 'ALIA', 'FAHD', 'AH78', 1, '2019-12-01 05:03:01', '2019-12-02 07:00:05'),
	('2019-12-01', 20, 'Pegasus_PLANE', 'ABU_DHABI', 'FAHD', 'DA73', 1, '2019-12-11 05:03:01', '2019-12-12 07:00:05'),
	('2019-12-02', 11, 'TUR_PLANE', 'CAIRO', 'ABU_DHABI', 'AA47', 1, '2019-12-10 05:03:01', '2019-12-11 08:12:020');

/*Should customer number be unique*/
INSERT INTO SeatReservation(FlightLegInstanceDate ,FlightLegNumber, FlightNumber, SeatNumber, CustomerName, CustomerNumber)
VALUES
	('2019-12-25', 1, 'AC01', 100, 'Abdullah Mansour', 1),
	('2019-12-26', 2, 'AC01', 101, 'Abdullah Mansour', 1),
	('2019-12-27', 1, 'DC99', 102, 'Abdullah Omari', 3),
	('2019-12-28', 2, 'DC99', 103, 'Abdullah Omari', 4),
	('2019-12-29', 1, 'AH78', 104, 'Maher Sh7', 5);";
        private const string TablesCreationScript = @"
DROP TABLE IF EXISTS 
Airport,
Flight,
FlightLeg,
FlightLegInstance,
FlightFare,
AirplaneType,
Airplane,
AirplaneTypeLanding,
SeatReservation
CASCADE;

DROP DOMAIN IF EXISTS 
NTEXT,
PINT,
NINT
CASCADE;

/*Not empty text*/
CREATE DOMAIN NTEXT AS TEXT NOT NULL CHECK (LENGTH(VALUE) > 0);

/*Not null integer*/
CREATE DOMAIN NINT AS INTEGER NOT NULL CHECK(VALUE  > 0);

/*Not null greater than 0 integer*/
CREATE DOMAIN PINT AS INTEGER NOT NULL CHECK(VALUE  > 0);

CREATE TABLE Airport (
	AirportCode NTEXT PRIMARY KEY,
	Name NTEXT,
    City NTEXT,
    State NTEXT
);
CREATE TABLE AirplaneType (
    TypeName NTEXT PRIMARY KEY,
    Company NTEXT,
    MaxSeats PINT
);
CREATE TABLE AirplaneTypeLanding (
    TypeName NTEXT REFERENCES AirplaneType(TypeName),
    AirportCode NTEXT REFERENCES Airport(AirportCode),
    PRIMARY KEY(TypeName, AirportCode)
);
CREATE TABLE Airplane (
    AirplaneId NTEXT PRIMARY KEY,
    TotalSeats PINT,
    TypeName NTEXT REFERENCES AirplaneType(TypeName)
);
CREATE TABLE Flight (
	FlightNumber NTEXT PRIMARY KEY,
	Airline NTEXT,
	/*Each bit represents a day*/
    Weekdays NINT CHECK(Weekdays <= 127)
);
CREATE TABLE FlightLeg (
	FlightLegNumber NINT,
	DepartureAirportCode NTEXT REFERENCES Airport(AirportCode),
	ArrivalAirportCode NTEXT REFERENCES Airport(AirportCode),
    ScheduledArrivalTime TIMESTAMP NOT NULL,
    ScheduledDepartureTime TIMESTAMP NOT NULL,
    FlightNumber NTEXT REFERENCES Flight(FlightNumber),
	PRIMARY KEY(FlightLegNumber, FlightNumber),
	CHECK(ScheduledArrivalTime > ScheduledDepartureTime)
);
CREATE TABLE FlightFare (
    FareCode NTEXT,
    Amount NUMERIC(12, 5) NOT NULL,
    FlightNumber NTEXT REFERENCES Flight(FlightNumber),
	Restrictions TEXT,
    PRIMARY KEY(FareCode, FlightNumber)
);
CREATE TABLE FlightLegInstance (
    FlightLegInstanceDate DATE NOT NULL,
    AvailableSeats NINT CHECK(AvailableSeats >= 0),
    AirplaneId NTEXT REFERENCES Airplane(AirplaneId),
    DepartureAirportCode NTEXT REFERENCES Airport(AirportCode),
	ArrivalAirportCode NTEXT REFERENCES Airport(AirportCode),
    FlightNumber NTEXT,
    FlightLegNumber NINT,
	ArrivalTime TIMESTAMP NOT NULL,
    DepartureTime TIMESTAMP NOT NULL,
    PRIMARY KEY(FlightLegInstanceDate, FlightNumber, FlightLegNumber),
	FOREIGN KEY(FlightLegNumber, FlightNumber) REFERENCES FlightLeg(FlightLegNumber, FlightNumber),
	CHECK(DepartureAirportCode != ArrivalAirportCode),
	CHECK(ArrivalTime > DepartureTime)
);

CREATE TABLE SeatReservation (
    FlightLegNumber NINT,
    FlightNumber NTEXT,
    SeatNumber PINT,
    FlightLegInstanceDate DATE NOT NULL,
    CustomerName NTEXT,
    CustomerNumber NTEXT,
    PRIMARY KEY(FlightLegNumber, FlightNumber, SeatNumber, FlightLegInstanceDate),
	FOREIGN KEY (FlightNumber, FlightLegNumber, FlightLegInstanceDate) REFERENCES FlightLegInstance(FlightNumber, FlightLegNumber, FlightLegInstanceDate)
);";
        public static NpgsqlConnection GetConnection() => new NpgsqlConnection($@"Server=127.0.0.1;Port=5432;Database={DbName};User Id={PostgresUserId};Password={PostgressPassword};");
        private static NpgsqlConnection GetNoDbConnection() => new NpgsqlConnection($@"Server=127.0.0.1;Port=5432;User Id={PostgresUserId};Password={PostgressPassword};");
        private static async ValueTask CreateDatabase()
        {
            using (var xcon = GetNoDbConnection())
            {
                await xcon.ExecuteAsync($@"
DROP DATABASE IF EXISTS {DbName};
CREATE DATABASE {DbName};
").ConfigureAwait(false);
            }
            using var con = GetConnection();
            await con.ExecuteAsync(TablesCreationScript).ConfigureAwait(false);
        }
        public static async ValueTask PopulateDatabase()
        {
            using var con = GetConnection();
            foreach (var tbl in TablesNames)
            {
                await con.ExecuteAsync($"ALTER TABLE {tbl} DISABLE TRIGGER ALL;").ConfigureAwait(false);
            }
            foreach (var tbl in TablesNames)
            {
                await con.ExecuteAsync($"TRUNCATE TABLE {tbl} RESTART IDENTITY CASCADE;").ConfigureAwait(false);
            }

            await con.ExecuteAsync(DataPopulationScript).ConfigureAwait(false);

            foreach (var tbl in TablesNames)
            {
                await con.ExecuteAsync($"ALTER TABLE {tbl} ENABLE TRIGGER ALL;").ConfigureAwait(false);
            }
        }
        public static async ValueTask ValidateDatabase()
        {
            try
            {
                using var con = GetConnection();
                foreach (var tbl in TablesNames)
                {
                    await con.ExecuteAsync($"SELECT * FROM {tbl}").ConfigureAwait(false);
                }
            }
            catch (PostgresException)
            {
                await CreateDatabase().ConfigureAwait(false);
            }
        }
    }
}
