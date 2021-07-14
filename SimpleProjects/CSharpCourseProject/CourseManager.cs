using System.Collections.Generic;
using System.Data.SQLite;

namespace CSharpProject
{
    class CourseManager
    {
        public static Course[] GetAll()
        {
            List<Course> result = new List<Course>();
            using (var con = new SQLiteConnection(DbInfo.ConnectionString))
            using (var cmd = new SQLiteCommand(@"SELECT * FROM Course", con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cr = new Course(int.Parse(reader["Id"].ToString()))
                        {
                            Name = (string)reader["Name"]
                        };
                        result.Add(cr);
                    }
                }
            }
            return result.ToArray();
        }
        public static Course Get(int courseId)
        {
            using (var con = new SQLiteConnection(DbInfo.ConnectionString))
            using (var cmd = new SQLiteCommand(@"SELECT * FROM Course WHERE id = @courseId", con))
            {
                con.Open();
                cmd.Parameters.AddWithValue("@courseId", courseId);
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    return new Course(int.Parse(reader["Id"].ToString()))
                    {
                        Name = (string)reader["Name"]
                    };
                }
            }
        }
        public static void Delete(int courseId)
        {
            using (var con = new SQLiteConnection(DbInfo.ConnectionString))
            using (var cmd = new SQLiteCommand(@"DELETE FROM CourseGrade WHERE courseId = @courseId;DELETE FROM Course WHERE id = @courseId", con))
            {
                con.Open();
                cmd.Parameters.AddWithValue("@courseId", courseId);
                cmd.CommandTimeout = 10;
                cmd.ExecuteNonQuery();
            }
        }
        public static Course Create()
        {
            using (var con = new SQLiteConnection(DbInfo.ConnectionString))
            using (var cmd = new SQLiteCommand(@"INSERT INTO Course(name) VALUES('')", con))
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return new Course((int)con.LastInsertRowId) { Name = "" };
            }
        }
        public static void Update(Course course)
        {
            using (var con = new SQLiteConnection(DbInfo.ConnectionString))
            using (var cmd = new SQLiteCommand("UPDATE Course SET Name = @name WHERE id = @courseId", con))
            {
                con.Open();
                cmd.Parameters.AddWithValue("@courseId", course.Id);
                cmd.Parameters.AddWithValue("@name", course.Name);
                cmd.ExecuteNonQuery();
            }
        }
        public static CourseStatistics GetStatistics(int courseId)
        {
            using (var con = new SQLiteConnection(DbInfo.ConnectionString))
            {
                con.Open();
                int nStudents, nPassedStudents;
                double lowestGrade = 0.0, highestGrade = 0.0;
                using (var cmd = new SQLiteCommand(
@"SELECT COUNT(courseId) FROM CourseGrade WHERE courseId = @courseId;
SELECT COUNT(courseId) FROM CourseGrade WHERE courseId = @courseId AND (First + Second + Third + Final) >= 50;
SELECT MIN(First + Second + Third + Final) FROM CourseGrade WHERE courseId = @courseId;
SELECT MAX(First + Second + Third + Final) FROM CourseGrade WHERE courseId = @courseId;", con))
                {
                    cmd.Parameters.AddWithValue("@courseId", courseId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        nStudents = reader.GetInt32(0);
                        reader.NextResult();
                        reader.Read();
                        nPassedStudents = reader.GetInt32(0);
                        reader.NextResult();
                        reader.Read();
                        if (reader.IsDBNull(0) == false) { lowestGrade = reader.GetDouble(0); }
                        reader.NextResult();
                        reader.Read();
                        if (reader.IsDBNull(0) == false) { highestGrade = reader.GetDouble(0); }
                    }
                }
                return new CourseStatistics(nStudents, nPassedStudents, nStudents - nPassedStudents, lowestGrade, highestGrade);
            }
        }
    }
}
