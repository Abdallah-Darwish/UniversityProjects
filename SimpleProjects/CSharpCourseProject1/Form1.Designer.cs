namespace CSharpProject
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstStudents = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lstCourses = new System.Windows.Forms.ListView();
            this.CHID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCreateStudent = new System.Windows.Forms.Button();
            this.btnCreateCourse = new System.Windows.Forms.Button();
            this.btnDeleteStudent = new System.Windows.Forms.Button();
            this.btnDeleteCourse = new System.Windows.Forms.Button();
            this.btnExportStudents = new System.Windows.Forms.Button();
            this.btnexportCourses = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstStudents
            // 
            this.lstStudents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lstStudents.FullRowSelect = true;
            this.lstStudents.HideSelection = false;
            this.lstStudents.Location = new System.Drawing.Point(12, 25);
            this.lstStudents.MultiSelect = false;
            this.lstStudents.Name = "lstStudents";
            this.lstStudents.Size = new System.Drawing.Size(568, 207);
            this.lstStudents.TabIndex = 0;
            this.lstStudents.UseCompatibleStateImageBehavior = false;
            this.lstStudents.View = System.Windows.Forms.View.Details;
            this.lstStudents.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstStudents_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            this.columnHeader1.Width = 67;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "First Name";
            this.columnHeader2.Width = 76;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Last Name";
            this.columnHeader3.Width = 78;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Email";
            this.columnHeader4.Width = 158;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Telephone";
            this.columnHeader5.Width = 89;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Birthday";
            this.columnHeader6.Width = 79;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Students";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 281);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Courses";
            // 
            // lstCourses
            // 
            this.lstCourses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CHID,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader12,
            this.columnHeader11});
            this.lstCourses.FullRowSelect = true;
            this.lstCourses.HideSelection = false;
            this.lstCourses.Location = new System.Drawing.Point(12, 297);
            this.lstCourses.MultiSelect = false;
            this.lstCourses.Name = "lstCourses";
            this.lstCourses.Size = new System.Drawing.Size(568, 97);
            this.lstCourses.TabIndex = 4;
            this.lstCourses.UseCompatibleStateImageBehavior = false;
            this.lstCourses.View = System.Windows.Forms.View.Details;
            this.lstCourses.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstCourses_MouseDoubleClick);
            // 
            // CHID
            // 
            this.CHID.Text = "Id";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Name";
            this.columnHeader7.Width = 100;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Endrolled Students";
            this.columnHeader8.Width = 110;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Passed";
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Failed";
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Lowest Grade";
            this.columnHeader12.Width = 80;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Highest Grade";
            this.columnHeader11.Width = 80;
            // 
            // btnCreateStudent
            // 
            this.btnCreateStudent.Location = new System.Drawing.Point(12, 238);
            this.btnCreateStudent.Name = "btnCreateStudent";
            this.btnCreateStudent.Size = new System.Drawing.Size(75, 23);
            this.btnCreateStudent.TabIndex = 5;
            this.btnCreateStudent.Text = "Create";
            this.btnCreateStudent.UseVisualStyleBackColor = true;
            this.btnCreateStudent.Click += new System.EventHandler(this.btnCreateStudent_Click);
            // 
            // btnCreateCourse
            // 
            this.btnCreateCourse.Location = new System.Drawing.Point(12, 400);
            this.btnCreateCourse.Name = "btnCreateCourse";
            this.btnCreateCourse.Size = new System.Drawing.Size(75, 23);
            this.btnCreateCourse.TabIndex = 6;
            this.btnCreateCourse.Text = "Create";
            this.btnCreateCourse.UseVisualStyleBackColor = true;
            this.btnCreateCourse.Click += new System.EventHandler(this.btnCreateCourse_Click);
            // 
            // btnDeleteStudent
            // 
            this.btnDeleteStudent.Location = new System.Drawing.Point(505, 238);
            this.btnDeleteStudent.Name = "btnDeleteStudent";
            this.btnDeleteStudent.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteStudent.TabIndex = 7;
            this.btnDeleteStudent.Text = "Delete";
            this.btnDeleteStudent.UseVisualStyleBackColor = true;
            this.btnDeleteStudent.Click += new System.EventHandler(this.btnDeleteStudent_Click);
            // 
            // btnDeleteCourse
            // 
            this.btnDeleteCourse.Location = new System.Drawing.Point(505, 400);
            this.btnDeleteCourse.Name = "btnDeleteCourse";
            this.btnDeleteCourse.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteCourse.TabIndex = 8;
            this.btnDeleteCourse.Text = "Delete";
            this.btnDeleteCourse.UseVisualStyleBackColor = true;
            this.btnDeleteCourse.Click += new System.EventHandler(this.btnDeleteCourse_Click);
            // 
            // btnExportStudents
            // 
            this.btnExportStudents.Location = new System.Drawing.Point(248, 238);
            this.btnExportStudents.Name = "btnExportStudents";
            this.btnExportStudents.Size = new System.Drawing.Size(75, 23);
            this.btnExportStudents.TabIndex = 9;
            this.btnExportStudents.Text = "Export";
            this.btnExportStudents.UseVisualStyleBackColor = true;
            this.btnExportStudents.Click += new System.EventHandler(this.btnExportStudents_Click);
            // 
            // btnexportCourses
            // 
            this.btnexportCourses.Location = new System.Drawing.Point(248, 400);
            this.btnexportCourses.Name = "btnexportCourses";
            this.btnexportCourses.Size = new System.Drawing.Size(75, 23);
            this.btnexportCourses.TabIndex = 10;
            this.btnexportCourses.Text = "Export";
            this.btnexportCourses.UseVisualStyleBackColor = true;
            this.btnexportCourses.Click += new System.EventHandler(this.btnexportCourses_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(248, 451);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(75, 23);
            this.btnAbout.TabIndex = 11;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 486);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnexportCourses);
            this.Controls.Add(this.btnExportStudents);
            this.Controls.Add(this.btnDeleteCourse);
            this.Controls.Add(this.btnDeleteStudent);
            this.Controls.Add(this.btnCreateCourse);
            this.Controls.Add(this.btnCreateStudent);
            this.Controls.Add(this.lstCourses);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstStudents);
            this.Name = "Form1";
            this.Text = "RegSystem";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstStudents;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lstCourses;
        private System.Windows.Forms.ColumnHeader CHID;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.Button btnCreateStudent;
        private System.Windows.Forms.Button btnCreateCourse;
        private System.Windows.Forms.Button btnDeleteStudent;
        private System.Windows.Forms.Button btnDeleteCourse;
        private System.Windows.Forms.Button btnExportStudents;
        private System.Windows.Forms.Button btnexportCourses;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
    }
}

