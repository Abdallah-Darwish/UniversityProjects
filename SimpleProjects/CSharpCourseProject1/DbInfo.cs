using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProject
{
    public static class DbInfo
    {
        public static string DbFileName { get; } = "StudentsDb.sqlite";
        public static string ConnectionString { get; } = $"Data Source={DbFileName};Version=3;";
        public static string CreationScript { get; } =
@"
CREATE TABLE Student(
Id INTEGER PRIMARY KEY AUTOINCREMENT,
FirstName TEXT NOT NULL,
LastName TEXT NOT NULL,
Birthday TEXT NOT NULL,
Email TEXT NOT NULL,
Telephone TEXT NOT NULL);
CREATE TABLE Course(
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Name TEXT NOT NULL);
CREATE TABLE CourseGrade(
CourseId INT NOT NULL,
StudentId INT NOT NULL,
First REAL NOT NULL,
Second REAL NOT NULL,
Third REAL NOT NULL,
Final REAL NOT NULL,
CONSTRAINT FK_CourseGrade_CourseId FOREIGN KEY (CourseId)
REFERENCES Course(Id),
CONSTRAINT FK_CourseGrade_StudentId FOREIGN KEY (StudentId)
REFERENCES Student(Id));";
    }
}
