using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProject
{
    public class Student
    {
        public int Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime Birthday { get; set; }
        public Dictionary<int, CourseGrade> Grades { get; } = new Dictionary<int, CourseGrade>();
        public Student(int id)
        {
            Id = id;
        }
        public string GetInfoText()
        {
            return
$@"Id: {Id}
First Name: {FirstName}
Last Name: {LastName}
Email: {Email}
Telephone: {Telephone}
Birthday: {Birthday.ToShortDateString()}
Grades: {{ {string.Join(", ", Grades.Values)} }}";
        }
    }
}
