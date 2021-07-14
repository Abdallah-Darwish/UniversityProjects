using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
namespace CSharpCourseProject2
{
    class Employee
    {
        private string _username;
        private string _password;

        private Employee() { }
        void UpdateValue(string fieldName, object value)
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand($"UPDATE Employee SET {fieldName} = @v WHERE Id = @id;", con))
            {
                com.Parameters.AddWithValue("@id", Id);
                com.Parameters.AddWithValue("@v", value);
                com.ExecuteNonQuery();
            }
        }
        public long Id { get; private set; }
        public string Username
        {
            get => _username;
            set
            {
                UpdateValue(nameof(Username), value);
                _username = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                UpdateValue(nameof(Password), value);
                _password = value;
            }
        }
        public void Delete()
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand(@"DELETE FROM RentalContract WHERE EmployeeId = @id;DELETE FROM Employee WHERE Id = @id;", con))
            {
                com.Parameters.AddWithValue("@id", Id);
                com.ExecuteNonQuery();
            }
        }
        public static Employee CurrentUser { get; private set; }
        public static void Logout() => CurrentUser = null;
        public static bool Login(string username, string password)
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand("SELECT Id FROM Employee WHERE Username = @username AND Password = @password", con))
            {
                com.Parameters.AddWithValue("@username", username);
                com.Parameters.AddWithValue("@password", password);
                var id = com.ExecuteScalar();
                if (id == null) { return false; }
                CurrentUser = new Employee { Id = (long)id, _username = username, _password = password };
                return true;
            }
        }
        public static bool SignUp(string username, string password)
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand("INSERT INTO Employee(Username, Password) VALUES(@username, @password)", con))
            {
                com.Parameters.AddWithValue("@username", username);
                com.Parameters.AddWithValue("@password", password);
                try
                {
                    com.ExecuteNonQuery();
                }
                catch (SQLiteException) { return false; }
                return true;
            }
        }
        public static Employee GetEmployee(long id)
        {
            using (var con = DbManager.GetConnection())
            using (var com = new SQLiteCommand("SELECT * FROM Employee WHERE Id = @id", con))
            {
                com.Parameters.AddWithValue("@id", id);
                using (var rdr = com.ExecuteReader())
                {
                    rdr.Read();
                    return new Employee
                    {
                        Id = id,
                        _username = (string)rdr[nameof(Username)],
                        _password = (string)rdr[nameof(Password)],
                    };
                }
            }
        }
    }
}
