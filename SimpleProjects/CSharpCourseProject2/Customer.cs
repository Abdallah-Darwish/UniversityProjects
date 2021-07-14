using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCourseProject2
{
    public class Customer
    {
        private Customer()
        {

        }
        void UpdateValue(string fieldName, object value)
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"UPDATE Customer SET {fieldName} = @v WHERE Id = @id;", con))
            {
                com.Parameters.AddWithValue("@id", Id);
                com.Parameters.AddWithValue("@v", value);
                com.ExecuteNonQuery();
            }
        }
        private string _firstName;
        private string _lastName;
        private int _age;
        private string _licenseNumber;
        private string _email;
        public long Id { get; private set; }
        public string FirstName
        {
            get => _firstName;
            set
            {
                UpdateValue(nameof(FirstName), value);
                _firstName = value;
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                UpdateValue(nameof(LastName), value);
                _lastName = value;
            }
        }
        public string FullName => $"{FirstName} {LastName}";
        public int Age
        {
            get => _age;
            set
            {
                UpdateValue(nameof(Age), value);
                _age = value;
            }
        }
        public string LicenseNumber
        {
            get => _licenseNumber;
            set
            {
                UpdateValue(nameof(LicenseNumber), value);
                _licenseNumber = value;
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                UpdateValue(nameof(Email), value);
                _email = value;
            }
        }
        public bool HasRentalContracts
        {
            get
            {
                using (var con = DbManager.GetConnection())
                using (var com = new SQLiteCommand($"SELECT id FROM RentalContract WHERE CustomerId = @id;", con))
                {
                    com.Parameters.AddWithValue("@id", Id);
                    return com.ExecuteScalar() != null;
                }
            }
        }
        public void DeleteRentalContracts()
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"DELETE FROM RentalContract WHERE CustomerId = @id;", con))
            {
                com.Parameters.AddWithValue("@id", Id);
                com.ExecuteNonQuery();
            }
        }
        public void Delete()
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"DELETE FROM Customer WHERE Id = @id;", con))
            {
                com.Parameters.AddWithValue("@id", Id);
                com.ExecuteNonQuery();
            }
        }
        public override string ToString() => FullName;
        private static Customer ReadCustomer(SQLiteDataReader rdr)
        {
            return new Customer
            {
                Id = (long)rdr[nameof(Id)],
                _age = (int)(long)rdr[nameof(Age)],
                _firstName = rdr[nameof(FirstName)].ToString(),
                _lastName = rdr[nameof(LastName)].ToString(),
                _email = rdr[nameof(Email)].ToString(),
                _licenseNumber = rdr[nameof(LicenseNumber)].ToString(),
            };
        }
        public static Customer GetCustomer(long id)
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"SELECT * FROM Customer WHERE id = @id", con))
            {
                com.Parameters.AddWithValue("@id", id);
                using (var rdr = com.ExecuteReader())
                {
                    rdr.Read();
                    return ReadCustomer(rdr);
                }
            }
        }
        public static Customer[] GetAllCustomers()
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"SELECT * FROM Customer", con))
            using (var rdr = com.ExecuteReader())
            {
                var res = new List<Customer>();
                while (rdr.Read())
                {
                    res.Add(ReadCustomer(rdr));
                }
                return res.ToArray();
            }
        }
        public static Customer Create()
        {
            using (var con = DbManager.GetConnection())
            {
                using (var com = new SQLiteCommand($"INSERT INTO Customer DEFAULT VALUES;", con))
                {
                    com.ExecuteNonQuery();
                }
                using (var com = new SQLiteCommand($"SELECT * FROM Customer WHERE Id = @id;", con))
                {
                    com.Parameters.AddWithValue("@id", con.LastInsertRowId);
                    using (var rdr = com.ExecuteReader())
                    {
                        rdr.Read();
                        return ReadCustomer(rdr);
                    }
                }
            }
        }
    }
}
