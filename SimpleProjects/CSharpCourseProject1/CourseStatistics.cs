using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProject
{
    class CourseStatistics
    {
        public CourseStatistics(int numberOfEnrolledStudents, int numberOfPassedStudents, int numberOfFailedStudents, double lowestGrade, double highestGrade)
        {
            NumberOfEnrolledStudents = numberOfEnrolledStudents;
            NumberOfPassedStudents = numberOfPassedStudents;
            NumberOfFailedStudents = numberOfFailedStudents;
            LowestGrade = lowestGrade;
            HighestGrade = highestGrade;
        }

        public int NumberOfEnrolledStudents { get; }
        public int NumberOfPassedStudents { get; }
        public int NumberOfFailedStudents { get; }
        public double HighestGrade { get; }
        public double LowestGrade { get; }
    }
}
