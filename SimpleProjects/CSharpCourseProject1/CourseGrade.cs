namespace CSharpProject
{
    public class CourseGrade
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public double First { get; set; }
        public double Second { get; set; }
        public double Third { get; set; }
        public double Final { get; set; }
        public double Total => First + Second + Third + Final;
        public override string ToString() => $"{CourseManager.Get(CourseId).Name} : {Total: 0.00}";
    }
}
