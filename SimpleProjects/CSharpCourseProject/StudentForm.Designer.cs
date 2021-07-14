namespace CSharpProject
{
    partial class StudentForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dteBirthday = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtTelephone = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblMaxGrade = new System.Windows.Forms.Label();
            this.lblMinGrade = new System.Windows.Forms.Label();
            this.lblFailedCourses = new System.Windows.Forms.Label();
            this.lblPassedCourses = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblAverage = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDropCourse = new System.Windows.Forms.Button();
            this.btnEnrollInCourse = new System.Windows.Forms.Button();
            this.cbxCourses = new System.Windows.Forms.ComboBox();
            this.nudFinal = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.nudThird = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.nudSecond = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.nudFirst = new System.Windows.Forms.NumericUpDown();
            this.lstCourses = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblId = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDeleteStudent = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFinal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThird)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSecond)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFirst)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 41);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "First Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 73);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Last Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Id";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(101, 37);
            this.txtFirstName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(271, 22);
            this.txtFirstName.TabIndex = 4;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(101, 69);
            this.txtLastName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(271, 22);
            this.txtLastName.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 172);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Birthday";
            // 
            // dteBirthday
            // 
            this.dteBirthday.Location = new System.Drawing.Point(101, 165);
            this.dteBirthday.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dteBirthday.Name = "dteBirthday";
            this.dteBirthday.Size = new System.Drawing.Size(271, 22);
            this.dteBirthday.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 105);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Email";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 137);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Telephone";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(101, 101);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(271, 22);
            this.txtEmail.TabIndex = 11;
            // 
            // txtTelephone
            // 
            this.txtTelephone.Location = new System.Drawing.Point(101, 133);
            this.txtTelephone.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTelephone.Name = "txtTelephone";
            this.txtTelephone.Size = new System.Drawing.Size(271, 22);
            this.txtTelephone.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblMaxGrade);
            this.groupBox1.Controls.Add(this.lblMinGrade);
            this.groupBox1.Controls.Add(this.lblFailedCourses);
            this.groupBox1.Controls.Add(this.lblPassedCourses);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.lblAverage);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(16, 484);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(597, 123);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            // 
            // lblMaxGrade
            // 
            this.lblMaxGrade.AutoSize = true;
            this.lblMaxGrade.Location = new System.Drawing.Point(131, 49);
            this.lblMaxGrade.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaxGrade.Name = "lblMaxGrade";
            this.lblMaxGrade.Size = new System.Drawing.Size(54, 17);
            this.lblMaxGrade.TabIndex = 11;
            this.lblMaxGrade.Text = "label18";
            // 
            // lblMinGrade
            // 
            this.lblMinGrade.AutoSize = true;
            this.lblMinGrade.Location = new System.Drawing.Point(131, 20);
            this.lblMinGrade.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMinGrade.Name = "lblMinGrade";
            this.lblMinGrade.Size = new System.Drawing.Size(54, 17);
            this.lblMinGrade.TabIndex = 10;
            this.lblMinGrade.Text = "label17";
            // 
            // lblFailedCourses
            // 
            this.lblFailedCourses.AutoSize = true;
            this.lblFailedCourses.Location = new System.Drawing.Point(495, 49);
            this.lblFailedCourses.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFailedCourses.Name = "lblFailedCourses";
            this.lblFailedCourses.Size = new System.Drawing.Size(54, 17);
            this.lblFailedCourses.TabIndex = 9;
            this.lblFailedCourses.Text = "label17";
            // 
            // lblPassedCourses
            // 
            this.lblPassedCourses.AutoSize = true;
            this.lblPassedCourses.Location = new System.Drawing.Point(495, 20);
            this.lblPassedCourses.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPassedCourses.Name = "lblPassedCourses";
            this.lblPassedCourses.Size = new System.Drawing.Size(54, 17);
            this.lblPassedCourses.TabIndex = 8;
            this.lblPassedCourses.Text = "label17";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(376, 49);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(102, 17);
            this.label16.TabIndex = 7;
            this.label16.Text = "Failed Courses";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(376, 20);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(111, 17);
            this.label15.TabIndex = 6;
            this.label15.Text = "Passed Courses";
            // 
            // lblAverage
            // 
            this.lblAverage.AutoSize = true;
            this.lblAverage.Location = new System.Drawing.Point(127, 79);
            this.lblAverage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAverage.Name = "lblAverage";
            this.lblAverage.Size = new System.Drawing.Size(54, 17);
            this.lblAverage.TabIndex = 5;
            this.lblAverage.Text = "label10";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 79);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 17);
            this.label9.TabIndex = 2;
            this.label9.Text = "Average";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 49);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "Maximum Grade";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 20);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "Minimum Grade";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDropCourse);
            this.groupBox2.Controls.Add(this.btnEnrollInCourse);
            this.groupBox2.Controls.Add(this.cbxCourses);
            this.groupBox2.Controls.Add(this.nudFinal);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.nudThird);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.nudSecond);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.nudFirst);
            this.groupBox2.Controls.Add(this.lstCourses);
            this.groupBox2.Location = new System.Drawing.Point(16, 197);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(597, 279);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Courses";
            // 
            // btnDropCourse
            // 
            this.btnDropCourse.Location = new System.Drawing.Point(81, 229);
            this.btnDropCourse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDropCourse.Name = "btnDropCourse";
            this.btnDropCourse.Size = new System.Drawing.Size(100, 28);
            this.btnDropCourse.TabIndex = 17;
            this.btnDropCourse.Text = "Drop";
            this.btnDropCourse.UseVisualStyleBackColor = true;
            this.btnDropCourse.Click += new System.EventHandler(this.btnDropCourse_Click);
            // 
            // btnEnrollInCourse
            // 
            this.btnEnrollInCourse.Location = new System.Drawing.Point(409, 229);
            this.btnEnrollInCourse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEnrollInCourse.Name = "btnEnrollInCourse";
            this.btnEnrollInCourse.Size = new System.Drawing.Size(100, 28);
            this.btnEnrollInCourse.TabIndex = 11;
            this.btnEnrollInCourse.Text = "Enroll";
            this.btnEnrollInCourse.UseVisualStyleBackColor = true;
            this.btnEnrollInCourse.Click += new System.EventHandler(this.btnEnrollInCourse_Click);
            // 
            // cbxCourses
            // 
            this.cbxCourses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCourses.FormattingEnabled = true;
            this.cbxCourses.Location = new System.Drawing.Point(192, 231);
            this.cbxCourses.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxCourses.Name = "cbxCourses";
            this.cbxCourses.Size = new System.Drawing.Size(208, 24);
            this.cbxCourses.TabIndex = 9;
            // 
            // nudFinal
            // 
            this.nudFinal.DecimalPlaces = 2;
            this.nudFinal.Location = new System.Drawing.Point(464, 191);
            this.nudFinal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudFinal.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.nudFinal.Name = "nudFinal";
            this.nudFinal.Size = new System.Drawing.Size(121, 22);
            this.nudFinal.TabIndex = 8;
            this.nudFinal.Tag = "Final";
            this.nudFinal.ValueChanged += new System.EventHandler(this.nudFinal_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(401, 193);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(38, 17);
            this.label13.TabIndex = 7;
            this.label13.Text = "Final";
            // 
            // nudThird
            // 
            this.nudThird.DecimalPlaces = 2;
            this.nudThird.Location = new System.Drawing.Point(60, 191);
            this.nudThird.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudThird.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudThird.Name = "nudThird";
            this.nudThird.Size = new System.Drawing.Size(121, 22);
            this.nudThird.TabIndex = 6;
            this.nudThird.Tag = "Third";
            this.nudThird.ValueChanged += new System.EventHandler(this.nudThird_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 193);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 17);
            this.label12.TabIndex = 5;
            this.label12.Text = "Third";
            // 
            // nudSecond
            // 
            this.nudSecond.DecimalPlaces = 2;
            this.nudSecond.Location = new System.Drawing.Point(464, 150);
            this.nudSecond.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudSecond.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudSecond.Name = "nudSecond";
            this.nudSecond.Size = new System.Drawing.Size(121, 22);
            this.nudSecond.TabIndex = 4;
            this.nudSecond.Tag = "Second";
            this.nudSecond.ValueChanged += new System.EventHandler(this.nudSecond_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(397, 153);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 17);
            this.label11.TabIndex = 3;
            this.label11.Text = "Second";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 153);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 17);
            this.label10.TabIndex = 2;
            this.label10.Text = "First";
            // 
            // nudFirst
            // 
            this.nudFirst.DecimalPlaces = 2;
            this.nudFirst.Location = new System.Drawing.Point(60, 150);
            this.nudFirst.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudFirst.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudFirst.Name = "nudFirst";
            this.nudFirst.Size = new System.Drawing.Size(121, 22);
            this.nudFirst.TabIndex = 1;
            this.nudFirst.Tag = "First";
            this.nudFirst.ValueChanged += new System.EventHandler(this.nudFirst_ValueChanged);
            // 
            // lstCourses
            // 
            this.lstCourses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lstCourses.FullRowSelect = true;
            this.lstCourses.HideSelection = false;
            this.lstCourses.Location = new System.Drawing.Point(8, 23);
            this.lstCourses.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstCourses.MultiSelect = false;
            this.lstCourses.Name = "lstCourses";
            this.lstCourses.Size = new System.Drawing.Size(580, 118);
            this.lstCourses.TabIndex = 0;
            this.lstCourses.UseCompatibleStateImageBehavior = false;
            this.lstCourses.View = System.Windows.Forms.View.Details;
            this.lstCourses.SelectedIndexChanged += new System.EventHandler(this.lstCourses_SelectedIndexChanged);
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Id";
            this.columnHeader7.Width = 39;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 122;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "First";
            this.columnHeader2.Width = 51;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Second";
            this.columnHeader3.Width = 52;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Third";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Final";
            this.columnHeader5.Width = 48;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Total";
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(97, 11);
            this.lblId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(54, 17);
            this.lblId.TabIndex = 15;
            this.lblId.Text = "label14";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(513, 629);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDeleteStudent
            // 
            this.btnDeleteStudent.Location = new System.Drawing.Point(16, 629);
            this.btnDeleteStudent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDeleteStudent.Name = "btnDeleteStudent";
            this.btnDeleteStudent.Size = new System.Drawing.Size(100, 28);
            this.btnDeleteStudent.TabIndex = 17;
            this.btnDeleteStudent.Text = "Delete";
            this.btnDeleteStudent.UseVisualStyleBackColor = true;
            this.btnDeleteStudent.Click += new System.EventHandler(this.btnDeleteStudent_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(253, 629);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 28);
            this.btnExport.TabIndex = 18;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // StudentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 672);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnDeleteStudent);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtTelephone);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dteBirthday);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "StudentForm";
            this.Text = "StudentForm";
            this.Load += new System.EventHandler(this.StudentForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFinal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThird)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSecond)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFirst)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dteBirthday;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtTelephone;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblAverage;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnEnrollInCourse;
        private System.Windows.Forms.ComboBox cbxCourses;
        private System.Windows.Forms.NumericUpDown nudFinal;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudThird;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown nudSecond;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudFirst;
        private System.Windows.Forms.ListView lstCourses;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblFailedCourses;
        private System.Windows.Forms.Label lblPassedCourses;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblMaxGrade;
        private System.Windows.Forms.Label lblMinGrade;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Button btnDropCourse;
        private System.Windows.Forms.Button btnDeleteStudent;
        private System.Windows.Forms.Button btnExport;
    }
}