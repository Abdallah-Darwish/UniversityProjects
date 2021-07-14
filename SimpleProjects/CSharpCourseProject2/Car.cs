using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCourseProject2
{
    class Car
    {
        private Car()
        {

        }
        private string _manufacturer;
        private string _plateNumber;
        private string _model;
        private int _engineCCs;
        private int _gearsCount;

        void UpdateValue(string fieldName, object value)
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"UPDATE Car SET {fieldName} = @v WHERE Id = @id;", con))
            {
                com.Parameters.AddWithValue("@id", Id);
                com.Parameters.AddWithValue("@v", value);
                com.ExecuteNonQuery();
            }
        }
        public long Id { get; private set; }
        public string Name => $"{Manufacturer}:{Model}";
        public string Manufacturer
        {
            get => _manufacturer;
            set
            {
                UpdateValue(nameof(Manufacturer), value);
                _manufacturer = value;
            }
        }
        public string PlateNumber
        {
            get => _plateNumber;
            set
            {
                UpdateValue(nameof(PlateNumber), value);
                _plateNumber = value;
            }
        }
        public string Model
        {
            get => _model;
            set
            {
                UpdateValue(nameof(Model), value);
                _model = value;
            }
        }

        public int EngineCCs
        {
            get => _engineCCs;
            set
            {
                UpdateValue(nameof(EngineCCs), value);
                _engineCCs = value;
            }
        }
        public int GearsCount
        {
            get => _gearsCount;
            set
            {
                UpdateValue(nameof(GearsCount), value);
                _gearsCount = value;
            }
        }
        public bool HasRentalContracts
        {
            get
            {
                using (var con = DbManager.GetConnection())
                using (var com = new SQLiteCommand("SELECT id FROM RentalContract WHERE CarId = @id;", con))
                {
                    com.Parameters.AddWithValue("@id", Id);
                    return com.ExecuteScalar() != null;
                }
            }
        }
        public override string ToString() => Name;
        public void DeleteRentalContracts()
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"DELETE FROM RentalContract WHERE CarId = @id;", con))
            {
                com.Parameters.AddWithValue("@id", Id);
                com.ExecuteNonQuery();
            }
        }
        public void Delete()
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"DELETE FROM Car WHERE Id = @id;", con))
            {
                com.Parameters.AddWithValue("@id", Id);
                com.ExecuteNonQuery();
            }
        }

        private static Car ReadCar(SQLiteDataReader rdr)
        {
            return new Car
            {
                Id = (long)rdr["Id"],
                _manufacturer = rdr[nameof(Manufacturer)].ToString(),
                _model = rdr[nameof(Model)].ToString(),
                _plateNumber = rdr[nameof(PlateNumber)].ToString(),
                _engineCCs = (int)(long)rdr[nameof(EngineCCs)],
                _gearsCount = (int)(long)rdr[nameof(GearsCount)],
            };
        }
        public static Car Create()
        {
            using (var con = DbManager.GetConnection())
            {
                using (var com = new SQLiteCommand($"INSERT INTO Car DEFAULT VALUES;", con))
                {
                    com.ExecuteNonQuery();
                }
                using (var com = new SQLiteCommand($"SELECT * FROM Car WHERE Id = @id;", con))
                {
                    com.Parameters.AddWithValue("@id", con.LastInsertRowId);
                    using (var rdr = com.ExecuteReader())
                    {
                        rdr.Read();
                        return ReadCar(rdr);
                    }
                }
            }
        }
        public static Car[] GetAllCars()
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"SELECT * FROM Car", con))
            using (var rdr = com.ExecuteReader())
            {
                var res = new List<Car>();
                while (rdr.Read())
                {
                    res.Add(ReadCar(rdr));
                }
                return res.ToArray();
            }
        }
        public static Car GetCar(long id)
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"SELECT * FROM Car WHERE id = @id", con))
            {
                com.Parameters.AddWithValue("@id", id);
                using (var rdr = com.ExecuteReader())
                {
                    rdr.Read();
                    return ReadCar(rdr);
                }
            }
        }
    }
}
