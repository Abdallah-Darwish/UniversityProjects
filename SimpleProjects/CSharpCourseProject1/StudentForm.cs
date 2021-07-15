using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
namespace CSharpProject
{
    public partial class StudentForm : Form
    {
        private readonly Student _student;
        public StudentForm(int studentId)
        {
            InitializeComponent();
            _student = StudentManager.Get(studentId);
        }
        private void UpdateInfo()
        {
            lblAverage.Text = (_student.Grades.Values.Select(c => c.Total).DefaultIfEmpty().Sum() / (Math.Max(_student.Grades.Count, 1) * 100f)).ToString();
            lblMinGrade.Text = (_student.Grades.Values.Select(c => c.Total).DefaultIfEmpty().Min()).ToString();
            lblMaxGrade.Text = (_student.Grades.Values.Select(c => c.Total).DefaultIfEmpty().Max()).ToString();
            lblFailedCourses.Text = (_student.Grades.Values.Where(c => c.Total < 50f).Count()).ToString();
            lblPassedCourses.Text = (_student.Grades.Values.Where(c => c.Total >= 50f).Count()).ToString();
        }
        private void StudentForm_Load(object sender, EventArgs e)
        {
            Text = $"Student {_student.Id}";
            dteBirthday.MaxDate = DateTime.Now;
            dteBirthday.MinDate = DateTime.MinValue;
            cbxCourses.Items.AddRange(CourseManager.GetAll());

            lblId.Text = _student.Id.ToString();
            txtFirstName.Text = _student.FirstName;
            txtLastName.Text = _student.LastName;
            txtTelephone.Text = _student.Telephone;
            txtEmail.Text = _student.Email;
            dteBirthday.Value = _student.Birthday;

            foreach (var grade in _student.Grades.Values)
            {
                var gradeItem = new ListViewItem() { Tag = grade };
                gradeItem.SubItems.AddRange(new string[6] { "", "", "", "", "", "" });
                UpdateGradeItem(gradeItem);
                lstCourses.Items.Add(gradeItem);
            }

            UpdateInfo();
        }
        private void UpdateGradeItem(ListViewItem item)
        {
            var grade = item.Tag as CourseGrade;
            item.Text = grade.CourseId.ToString();
            item.SubItems[1].Text = CourseManager.Get(grade.CourseId).Name;
            item.SubItems[2].Text = grade.First.ToString();
            item.SubItems[3].Text = grade.Second.ToString();
            item.SubItems[4].Text = grade.Third.ToString();
            item.SubItems[5].Text = grade.Final.ToString();
            item.SubItems[6].Text = grade.Total.ToString();
            UpdateInfo();
        }
        private void nudFirst_ValueChanged(object sender, EventArgs e)
        {
            if (lstCourses.SelectedItems.Count == 0) { return; }
            var selectedItem = lstCourses.SelectedItems[0];
            var selectedGrade = selectedItem.Tag as CourseGrade;
            selectedGrade.First = (float)nudFirst.Value;
            UpdateGradeItem(selectedItem);
        }

        private void nudSecond_ValueChanged(object sender, EventArgs e)
        {
            if (lstCourses.SelectedItems.Count == 0) { return; }
            var selectedItem = lstCourses.SelectedItems[0];
            var selectedGrade = selectedItem.Tag as CourseGrade;
            selectedGrade.Second = (float)nudSecond.Value;
            UpdateGradeItem(selectedItem);
        }

        private void nudThird_ValueChanged(object sender, EventArgs e)
        {
            if (lstCourses.SelectedItems.Count == 0) { return; }
            var selectedItem = lstCourses.SelectedItems[0];
            var selectedGrade = selectedItem.Tag as CourseGrade;
            selectedGrade.Third = (float)nudThird.Value;
            UpdateGradeItem(selectedItem);
        }

        private void nudFinal_ValueChanged(object sender, EventArgs e)
        {
            if (lstCourses.SelectedItems.Count == 0) { return; }
            var selectedItem = lstCourses.SelectedItems[0];
            var selectedGrade = selectedItem.Tag as CourseGrade;
            selectedGrade.Final = (float)nudFinal.Value;
            UpdateGradeItem(selectedItem);
        }

        private void btnEnrollInCourse_Click(object sender, EventArgs e)
        {
            var selectedCourse = cbxCourses.SelectedItem as Course;
            if (selectedCourse == null || _student.Grades.ContainsKey(selectedCourse.Id)) { return; }
            var grade = new CourseGrade { CourseId = selectedCourse.Id, StudentId = _student.Id, First = 0.0f, Second = 0.0f, Third = 0.0f, Final = 0.0f };
            var gradeItem = new ListViewItem { Tag = grade };
            gradeItem.SubItems.AddRange(new string[6] { "", "", "", "", "", "" });
            UpdateGradeItem(gradeItem);
            lstCourses.Items.Add(gradeItem);
            lstCourses.Items[lstCourses.Items.Count - 1].Selected = true;
            _student.Grades.Add(grade.CourseId, grade);
            UpdateInfo();
        }

        private void btnDropCourse_Click(object sender, EventArgs e)
        {
            if (lstCourses.SelectedItems.Count == 0) { return; }
            _student.Grades.Remove(int.Parse(lstCourses.SelectedItems[0].Text));
            lstCourses.Items.Remove(lstCourses.SelectedItems[0]);
            UpdateInfo();
        }

        private void lstCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCourses.SelectedItems.Count == 0) { return; }
            var selectedGrade = lstCourses.SelectedItems[0].Tag as CourseGrade;
            nudFirst.Value = (decimal)selectedGrade.First;
            nudSecond.Value = (decimal)selectedGrade.Second;
            nudThird.Value = (decimal)selectedGrade.Third;
            nudFinal.Value = (decimal)selectedGrade.Final;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _student.FirstName = txtFirstName.Text;
            _student.LastName = txtLastName.Text;
            _student.Email = txtEmail.Text;
            _student.Telephone = txtTelephone.Text;
            _student.Birthday = dteBirthday.Value;
            StudentManager.Update(_student);
        }

        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            StudentManager.Delete(_student.Id);
            Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var fileName = $"Student{_student.Id}.txt";
            using (var writer = new StreamWriter(fileName, false))
            {
                writer.WriteLine(_student.GetInfoText());
            }
            MessageBox.Show($"Exported student data to file {fileName}", "Exported data", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
