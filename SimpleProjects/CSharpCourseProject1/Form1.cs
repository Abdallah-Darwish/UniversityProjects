using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
namespace CSharpProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCreateStudent_Click(object sender, EventArgs e)
        {
            var frm = new StudentForm(StudentManager.Create().Id);
            frm.ShowDialog();
            ReloadStudents();
            ReloadCourses();
        }
        private void ReloadStudents()
        {
            lstStudents.Items.Clear();
            foreach (var student in StudentManager.GetAll())
            {
                var studentItem = new ListViewItem { Text = student.Id.ToString(), Tag = student };
                studentItem.SubItems.Add(student.FirstName);
                studentItem.SubItems.Add(student.LastName);
                studentItem.SubItems.Add(student.Email);
                studentItem.SubItems.Add(student.Telephone);
                studentItem.SubItems.Add(student.Birthday.ToShortDateString());
                lstStudents.Items.Add(studentItem);
            }
        }
        private void ReloadCourses()
        {
            lstCourses.Items.Clear();

            foreach (var course in CourseManager.GetAll())
            {
                var stat = CourseManager.GetStatistics(course.Id);

                var courseItem = new ListViewItem { Text = course.Id.ToString(), Tag = course };
                courseItem.SubItems.Add(course.Name);
                courseItem.SubItems.Add(stat.NumberOfEnrolledStudents.ToString());
                courseItem.SubItems.Add(stat.NumberOfPassedStudents.ToString());
                courseItem.SubItems.Add(stat.NumberOfFailedStudents.ToString());
                courseItem.SubItems.Add(stat.LowestGrade.ToString());
                courseItem.SubItems.Add(stat.HighestGrade.ToString());
                lstCourses.Items.Add(courseItem);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ReloadCourses();
            ReloadStudents();
        }

        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            if (lstStudents.SelectedItems.Count == 0) { return; }
            var selectedItem = lstStudents.SelectedItems[0];
            StudentManager.Delete((selectedItem.Tag as Student).Id);
            lstStudents.Items.Remove(selectedItem);
            ReloadCourses();
        }

        private void btnDeleteCourse_Click(object sender, EventArgs e)
        {
            if (lstCourses.SelectedItems.Count == 0) { return; }
            var selectedItem = lstCourses.SelectedItems[0];
            CourseManager.Delete((selectedItem.Tag as Course).Id);
            lstCourses.Items.Remove(selectedItem);
        }

        private void UpdateCourse(ListViewItem item)
        {
            var course = item.Tag as Course;
            var newName = Microsoft.VisualBasic.Interaction.InputBox("New course name", "Course Info", course.Name);
            course.Name = newName;
            CourseManager.Update(course);
            item.SubItems[1].Text = course.Name;
        }

        private void btnCreateCourse_Click(object sender, EventArgs e)
        {
            var newCourse = CourseManager.Create();
            var newItem = new ListViewItem { Text = newCourse.Id.ToString(), Tag = newCourse };
            newItem.SubItems.AddRange(new string[] { newCourse.Name, "0", "0", "0", "0", "0" });
            lstCourses.Items.Add(newItem);
            UpdateCourse(newItem);
        }

        private void btnExportStudents_Click(object sender, EventArgs e)
        {
            string fileName = "Students.txt";
            string seperator = "===========================================================================================";
            var students = StudentManager.GetAll();
            using (var writer = new StreamWriter(fileName, false))
            {
                for (int i = 0; i < students.Length; i++)
                {
                    if (i > 1) { writer.WriteLine(seperator); }
                    writer.WriteLine(students[i].GetInfoText());
                }
            }
            MessageBox.Show($"Exported all students info to file {fileName}", "Exported Students Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnexportCourses_Click(object sender, EventArgs e)
        {
            string fileName = "Courses.txt";
            string seperator = "===========================================================================================";
            var courses = CourseManager.GetAll();
            using (var writer = new StreamWriter(fileName, false))
            {
                for (int i = 0; i < courses.Length; i++)
                {
                    if (i > 1) { writer.WriteLine(seperator); }
                    writer.WriteLine(courses[i].GetInfoText());
                }
            }
            MessageBox.Show($"Exported all courses info to file {fileName}", "Exported Courses Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lstStudents_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstStudents.SelectedItems.Count == 0) { return; }
            var frm = new StudentForm((lstStudents.SelectedItems[0].Tag as Student).Id);
            frm.ShowDialog();
            ReloadStudents();
            ReloadCourses();
        }

        private void lstCourses_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstCourses.SelectedItems.Count == 0) { return; }
            UpdateCourse(lstCourses.SelectedItems[0]);
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mini Regestration system.\nDone By:\n1) Abdullah Darweish", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
