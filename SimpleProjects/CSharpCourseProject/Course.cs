namespace CSharpProject
{
    public class Course
    {
        public int Id { get; }
        public string Name { get; set; }
        public Course(int id)
        {
            Id = id;
        }
        public override string ToString() => Name;
        public string GetInfoText()
        {
            var stat = CourseManager.GetStatistics(Id);
            return
$@"Id: {Id}
Name: {Name}
Number of enrolled students: {stat.NumberOfEnrolledStudents}
Number of passed students: {stat.NumberOfPassedStudents}
Number of failed students: {stat.NumberOfFailedStudents}
Lowest Grade: {stat.LowestGrade}
Highest Grade: {stat.HighestGrade}";
        }
    }
}
