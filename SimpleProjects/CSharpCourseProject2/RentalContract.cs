using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCourseProject2
{
    class RentalContract
    {
        private RentalContract()
        {

        }
        void UpdateValue(string fieldName, object value)
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"UPDATE RentalContract SET {fieldName} = @v WHERE Id = @id;", con))
            {
                com.Parameters.AddWithValue("@id", Id);
                com.Parameters.AddWithValue("@v", value);
                com.ExecuteNonQuery();
            }
        }
        private long _carId, _customerId, _employeeId;
        private DateTime _startTime;
        private DateTime _endTime;
        private double _hourlyRate;

        public long Id { get; private set; }
        public Car Car => Car.GetCar(_carId);
        public Customer Customer => Customer.GetCustomer(_customerId);
        public Employee Employee => Employee.GetEmployee(_employeeId);
        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                UpdateValue(nameof(StartTime), value.Ticks);
                _startTime = value;
            }
        }
        public DateTime EndTime
        {
            get => _endTime;
            set
            {
                UpdateValue(nameof(EndTime), value.Ticks);
                _endTime = value;
            }
        }
        public double HourlyRate
        {
            get => _hourlyRate;
            set
            {
                UpdateValue(nameof(HourlyRate), value);
                _hourlyRate = value;

            }
        }
        public double Revenue => HourlyRate * (EndTime - StartTime).TotalHours;
        public void Delete()
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"DELETE FROM RentalContract WHERE Id = @id;", con))
            {
                com.Parameters.AddWithValue("@id", Id);
                com.ExecuteNonQuery();
            }
        }
        private static RentalContract ReadContract(SQLiteDataReader rdr)
        {
            return new RentalContract
            {
                Id = (long)rdr[nameof(Id)],
                _carId = (long)rdr["CarId"],
                _customerId = (long)rdr["CustomerId"],
                _employeeId = (long)rdr["EmployeeId"],
                _hourlyRate = (double)rdr[nameof(HourlyRate)],
                _startTime = new DateTime((long)rdr[nameof(StartTime)]),
                _endTime = new DateTime((long)rdr[nameof(EndTime)]),
            };
        }
        public static RentalContract GetContract(long id)
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"SELECT * FROM RentalContract WHERE id = @id", con))
            {
                com.Parameters.AddWithValue("@id", id);
                using (var rdr = com.ExecuteReader())
                {
                    rdr.Read();
                    return ReadContract(rdr);
                }
            }
        }
        public static RentalContract[] GetAllContracts()
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"SELECT * FROM RentalContract", con))
            using (var rdr = com.ExecuteReader())
            {
                var res = new List<RentalContract>();
                while (rdr.Read())
                {
                    res.Add(ReadContract(rdr));
                }
                return res.ToArray();
            }
        }
        public static RentalContract Create(Employee e, Customer cu, Car ca)
        {
            using (var con = DbManager.GetConnection())
            {
                using (var com = new SQLiteCommand($"INSERT INTO RentalContract(EmployeeId, CustomerId, CarId) VALUES(@eId, @cuId, @caId);", con))
                {
                    com.Parameters.AddWithValue("@eId", e.Id);
                    com.Parameters.AddWithValue("@cuId", cu.Id);
                    com.Parameters.AddWithValue("@caId", ca.Id);
                    com.ExecuteNonQuery();
                }
                using (var com = new SQLiteCommand($"SELECT * FROM RentalContract WHERE Id = @id;", con))
                {
                    com.Parameters.AddWithValue("@id", con.LastInsertRowId);
                    using (var rdr = com.ExecuteReader())
                    {
                        rdr.Read();
                        return ReadContract(rdr);
                    }
                }
            }
        }
    }
}
