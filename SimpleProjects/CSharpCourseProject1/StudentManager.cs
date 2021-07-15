using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
namespace CSharpProject
{
    class StudentManager
    {
        public static Student[] GetAll()
        {
            var result = new Dictionary<int, Student>();
            using (var con = new SQLiteConnection(DbInfo.ConnectionString))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(@"SELECT * FROM Student", con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var st = new Student(int.Parse(reader["Id"].ToString()))
                        {
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            Email = (string)reader["Email"],
                            Telephone = (string)reader["Telephone"],
                            Birthday = DateTime.ParseExact((string)reader["Birthday"], "o", System.Globalization.CultureInfo.InvariantCulture)
                        };
                        result.Add(st.Id, st);
                    }
                }
                using (var cmd = new SQLiteCommand(@"SELECT * FROM CourseGrade", con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cg = new CourseGrade
                        {
                            CourseId = (int)reader["CourseId"],
                            StudentId = (int)reader["StudentId"],
                            First = (double)reader["First"],
                            Second = (double)reader["Second"],
                            Third = (double)reader["Third"],
                            Final = (double)reader["Final"]
                        };
                        result[cg.StudentId].Grades.Add(cg.CourseId, cg);
                    }
                }
            }
            return result.Values.ToArray();
        }
        public static Student Get(int studentId)
        {
            Student st;
            using (var con = new SQLiteConnection(DbInfo.ConnectionString))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(@"SELECT * FROM Student WHERE id = @studentId", con))
                {
                    cmd.Parameters.AddWithValue("@studentId", studentId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        st = new Student(int.Parse(reader["Id"].ToString()))
                        {
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            Email = (string)reader["Email"],
                            Telephone = (string)reader["Telephone"],
                            Birthday = DateTime.ParseExact((string)reader["Birthday"], "o", System.Globalization.CultureInfo.InvariantCulture)
                        };

                    }
                }
                using (var cmd = new SQLiteCommand(@"SELECT * FROM CourseGrade WHERE studentId = @studentId", con))
                {
                    cmd.Parameters.AddWithValue("@studentId", studentId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cg = new CourseGrade
                            {
                                CourseId = (int)reader["CourseId"],
                                StudentId = (int)reader["StudentId"],
                                First = (double)reader["First"],
                                Second = (double)reader["Second"],
                                Third = (double)reader["Third"],
                                Final = (double)reader["Final"]
                            };
                            st.Grades.Add(cg.CourseId, cg);
                        }
                    }
                }
            }
            return st;
        }
        public static void Delete(int studentId)
        {
            using (var con = new SQLiteConnection(DbInfo.ConnectionString))
            using (var cmd = new SQLiteCommand(@"DELETE FROM CourseGrade WHERE studentId = @studentId;DELETE FROM Student WHERE id = @studentId", con))
            {
                con.Open();
                cmd.Parameters.AddWithValue("@studentId", studentId);
                cmd.ExecuteNonQuery();
            }
        }
        public static Student Create()
        {
            using (var con = new SQLiteConnection(DbInfo.ConnectionString))
            using (var cmd = new SQLiteCommand(@"INSERT INTO Student(firstName, lastName, Email, Telephone, birthday) VALUES('', '', '', '', @birthday)", con))
            {
                con.Open();
                DateTime birtday = DateTime.Now;
                cmd.Parameters.AddWithValue("@birthday", birtday.ToString("o"));
                cmd.ExecuteNonQuery();
                return new Student((int)con.LastInsertRowId)
                {
                    FirstName = "",
                    LastName = "",
                    Email = "",
                    Telephone = "",
                    Birthday = birtday
                };
            }
        }
        public static void Update(Student student)
        {
            using (var con = new SQLiteConnection(DbInfo.ConnectionString))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(@"UPDATE Student SET firstName = @firstName, lastName = @lastName, email = @email, telephone = @telephone, birthday = @birthday WHERE id = @studentId;DELETE FROM CourseGrade WHERE studentId = @studentId;", con))
                {
                    cmd.Parameters.AddWithValue("@firstName", student.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", student.LastName);
                    cmd.Parameters.AddWithValue("@email", student.Email);
                    cmd.Parameters.AddWithValue("@telephone", student.Telephone);
                    cmd.Parameters.AddWithValue("@birthday", student.Birthday.ToString("o"));
                    cmd.Parameters.AddWithValue("@studentId", student.Id);
                    cmd.ExecuteNonQuery();
                }
                foreach (var grade in student.Grades)
                {
                    using (var cmd = new SQLiteCommand(@"INSERT INTO CourseGrade(courseId, studentId, first, second, third, final) VALUES(@courseId, @studentId, @first, @second, @third, @final)", con))
                    {
                        cmd.Parameters.AddWithValue("@courseId", grade.Value.CourseId);
                        cmd.Parameters.AddWithValue("@studentId", grade.Value.StudentId);
                        cmd.Parameters.AddWithValue("@first", grade.Value.First);
                        cmd.Parameters.AddWithValue("@second", grade.Value.Second);
                        cmd.Parameters.AddWithValue("@third", grade.Value.Third);
                        cmd.Parameters.AddWithValue("@final", grade.Value.Final);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
