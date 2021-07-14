using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpCourseProject2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        void FillCustomerItem(ListViewItem it)
        {
            var c = it.Tag as Customer;
            it.SubItems.Clear();
            it.Text = c.Id.ToString();
            it.SubItems.AddRange(new string[] { c.FirstName, c.LastName, c.Email, c.LicenseNumber, c.Age.ToString() });
            cbxCustomer.Invalidate();
        }
        void FillCustomerInfo(Customer c)
        {
            txtFName.Text = c.FirstName;
            txtLName.Text = c.LastName;
            txtEmail.Text = c.Email;
            txtLN.Text = c.LicenseNumber;
            nudAge.Value = c.Age;
        }
        private void btnCreateCustomer_Click(object sender, EventArgs e)
        {
            var c = Customer.Create();
            var it = new ListViewItem { Tag = c };
            FillCustomerItem(it);
            lvCustomers.Items.Add(it);
            cbxCustomer.Items.Add(c);
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (lvCustomers.SelectedItems.Count == 0) { return; }
            var it = lvCustomers.SelectedItems[0];
            var c = it.Tag as Customer;
            if (c.HasRentalContracts)
            {
                if (MessageBox.Show($"Customer {c.FullName} has rental contracts, you have to delete them before deleteing him.\nDo you want to delete them ?", "Rental Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (var i in cbxCustomer.Items)
                    {
                        if ((i as Customer).Id == c.Id)
                        {
                            cbxCustomer.Items.Remove(i);
                            break;
                        }
                    }
                    //0 a
                    //1 b
                    //2 d
                    for (int i = 0; i < lvContracts.Items.Count; i++)
                    {
                        var contract = lvContracts.Items[i];
                        if ((contract.Tag as RentalContract).Customer.Id == c.Id)
                        {
                            lvContracts.Items.RemoveAt(i);
                            i--;
                        }
                    }
                    c.DeleteRentalContracts();
                    c.Delete();
                    lvCustomers.Items.Remove(it);
                }
            }
            else
            {
                foreach (var i in cbxCustomer.Items)
                {
                    if ((i as Customer).Id == c.Id)
                    {
                        cbxCustomer.Items.Remove(i);
                        break;
                    }
                }
                c.Delete();
                lvCustomers.Items.Remove(it);
            }
        }

        private void btnSaveCustomer_Click(object sender, EventArgs ex)
        {
            if (lvCustomers.SelectedItems.Count == 0) { return; }
            var it = lvCustomers.SelectedItems[0];
            var c = it.Tag as Customer;

            var errs = new List<string>();
            if (txtFName.TextLength == 0) { errs.Add("First Name can't be empty."); }
            if (txtLName.TextLength == 0) { errs.Add("Last Name can't be empty."); }
            var e = txtEmail.Text;
            if (e.Length == 0 || e.IndexOf('@') <= 0 || e.IndexOf('@') != e.LastIndexOf('@') || e[e.Length - 1] == '@')
            { errs.Add("Email must follow format *@*"); }
            if (txtLN.TextLength == 0) { errs.Add("License Number can't be empty."); }
            else if (txtLN.Text != c.LicenseNumber)
            {
                using (var con = DbManager.GetConnection())
                using (var com = new SQLiteCommand("SELECT FirstName || LastName FROM Customer WHERE LicenseNumber = @ln", con))
                {
                    com.Parameters.AddWithValue("@ln", txtLN.Text);
                    var licenseOwnerName = com.ExecuteScalar();
                    if (licenseOwnerName != null) { errs.Add($"License Number {txtLN.Text} is already used by \"{licenseOwnerName}\"."); }
                }
            }

            if (errs.Count == 0)
            {
                c.Age = (int)nudAge.Value;
                c.Email = txtEmail.Text;
                c.FirstName = txtFName.Text;
                c.LastName = txtLName.Text;
                c.LicenseNumber = txtLN.Text;
                FillCustomerItem(it);
            }
            else
            {
                MessageBox.Show(string.Join("\n", errs), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReloadCustomer_Click(object sender, EventArgs e)
        {
            if (lvCustomers.SelectedItems.Count == 0) { return; }
            var it = lvCustomers.SelectedItems[0];
            var c = it.Tag as Customer;
            FillCustomerInfo(c);
        }

        private void lvCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvCustomers.SelectedItems.Count == 0)
            {
                txtFName.Text = "";
                txtLName.Text = "";
                txtLN.Text = "";
                txtEmail.Text = "";
                nudAge.Value = nudAge.Minimum;
            }
            else
            {
                var it = lvCustomers.SelectedItems[0];
                var c = it.Tag as Customer;
                FillCustomerInfo(c);
            }
        }
        void FillCarItem(ListViewItem it)
        {
            var c = it.Tag as Car;
            it.SubItems.Clear();
            it.Text = c.Id.ToString();
            it.SubItems.AddRange(new string[] { c.Manufacturer, c.Model, c.PlateNumber, c.EngineCCs.ToString(), c.GearsCount.ToString() });
            btnDeleteContract.Invalidate();
        }
        void FillCarInfo(Car c)
        {
            nudEngineCCs.Value = c.EngineCCs;
            nudGearsCount.Value = c.GearsCount;
            txtManufacturer.Text = c.Manufacturer;
            txtModel.Text = c.Model;
            txtPN.Text = c.PlateNumber;
        }
        private void btnCreateCar_Click(object sender, EventArgs e)
        {
            var c = Car.Create();
            var it = new ListViewItem { Tag = c };
            FillCarItem(it);
            lvCars.Items.Add(it);
            cbxCar.Items.Add(c);
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            if (lvCars.SelectedItems.Count == 0) { return; }

            var it = lvCars.SelectedItems[0];
            var c = it.Tag as Car;
            if (c.HasRentalContracts)
            {
                if (MessageBox.Show($"Car {c.Name} has rental contracts, you have to delete them before deleteing it.\nDo you want to delete them ?", "Rental Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    c.DeleteRentalContracts();
                    foreach (var i in cbxCar.Items)
                    {
                        if ((i as Car).Id == c.Id)
                        {
                            cbxCar.Items.Remove(i);
                            break;
                        }
                    }
                    for (int i = 0; i < lvContracts.Items.Count; i++)
                    {
                        var contract = lvContracts.Items[i];
                        if ((contract.Tag as RentalContract).Car.Id == c.Id)
                        {
                            lvContracts.Items.RemoveAt(i);
                            i--;
                        }
                    }
                    c.Delete();
                    lvCars.Items.Remove(it);
                }
            }
            else
            {
                foreach (var i in cbxCar.Items)
                {
                    if ((i as Car).Id == c.Id)
                    {
                        cbxCar.Items.Remove(i);
                        break;
                    }
                }
                c.Delete();
                lvCars.Items.Remove(it);
            }
        }

        private void btnSaveCar_Click(object sender, EventArgs e)
        {
            if (lvCars.SelectedItems.Count == 0) { return; }
            var it = lvCars.SelectedItems[0];
            var c = it.Tag as Car;

            var errs = new List<string>();
            if (txtManufacturer.TextLength == 0) { errs.Add("Manufacturer can't be empty."); }
            if (txtModel.TextLength == 0) { errs.Add("Model can't be empty."); }

            if (txtPN.TextLength == 0) { errs.Add("Plate Number can't be empty."); }
            else if (txtLN.Text != c.PlateNumber)
            {
                using (var con = DbManager.GetConnection())
                using (var com = new SQLiteCommand("SELECT Manufacturer || ':' || Model FROM Car WHERE PlateNumber = @pn", con))
                {
                    com.Parameters.AddWithValue("@pn", txtPN.Text);
                    var plateOwnerName = com.ExecuteScalar();
                    if (plateOwnerName != null) { errs.Add($"Plate Number {txtPN.Text} is already used by \"{plateOwnerName}\"."); }
                }
            }

            if (errs.Count == 0)
            {
                c.EngineCCs = (int)nudEngineCCs.Value;
                c.GearsCount = (int)nudGearsCount.Value;
                c.Manufacturer = txtManufacturer.Text;
                c.Model = txtModel.Text;
                c.PlateNumber = txtPN.Text;
                FillCarItem(it);
            }
            else
            {
                MessageBox.Show(string.Join("\n", errs), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReloadCar_Click(object sender, EventArgs e)
        {
            if (lvCars.SelectedItems.Count == 0) { return; }
            var it = lvCars.SelectedItems[0];
            var c = it.Tag as Car;
            FillCarInfo(c);
        }

        private void lvCars_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvCars.SelectedItems.Count == 0)
            {

                txtPN.Text = "";
                txtManufacturer.Text = "";
                txtModel.Text = "";
                nudEngineCCs.Value = nudEngineCCs.Minimum;
                nudGearsCount.Value = nudGearsCount.Minimum;
            }
            else
            {
                var it = lvCars.SelectedItems[0];
                var c = it.Tag as Car;
                FillCarInfo(c);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtUsername.Text = Employee.CurrentUser.Username;
            foreach (var c in Customer.GetAllCustomers())
            {
                var it = new ListViewItem { Tag = c };
                FillCustomerItem(it);
                lvCustomers.Items.Add(it);
                cbxCustomer.Items.Add(c);
            }
            foreach (var c in Car.GetAllCars())
            {
                var it = new ListViewItem { Tag = c };
                FillCarItem(it);
                lvCars.Items.Add(it);
                cbxCar.Items.Add(c);
            }
            foreach (var c in RentalContract.GetAllContracts())
            {
                var it = new ListViewItem { Tag = c };
                FillContractItem(it);
                lvContracts.Items.Add(it);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Employee.Logout();
            var f = new LoginForm();
            f.Show();
        }
        void FillContractItem(ListViewItem it)
        {
            var c = it.Tag as RentalContract;
            it.SubItems.Clear();
            it.Text = c.Id.ToString();
            it.SubItems.AddRange(new string[] { c.Car.Name, c.Customer.FullName, c.Employee.Username, c.HourlyRate.ToString(), c.StartTime.ToString(), c.EndTime.ToString() });
        }
        void FillContractInfo(RentalContract c)
        {
            dtpStartTime.Value = c.StartTime;
            dtpEndTime.Value = c.EndTime;
            nudRate.Value = (decimal)c.HourlyRate;
        }
        private void btnCreateContract_Click(object sender, EventArgs e)
        {
            if (cbxCar.SelectedIndex == -1 || cbxCustomer.SelectedIndex == -1) { return; }
            var customer = cbxCustomer.SelectedItem as Customer;
            var car = cbxCar.SelectedItem as Car;
            var c = RentalContract.Create(Employee.CurrentUser, customer, car);
            var it = new ListViewItem { Tag = c };
            FillContractItem(it);
            lvContracts.Items.Add(it);
        }

        private void btnDeleteContract_Click(object sender, EventArgs e)
        {
            if (lvContracts.SelectedItems.Count == 0) { return; }

            var it = lvContracts.SelectedItems[0];
            var c = it.Tag as RentalContract;
            lvContracts.Items.Remove(it);
            c.Delete();
        }

        private void btnSaveContract_Click(object sender, EventArgs e)
        {
            if (lvContracts.SelectedItems.Count == 0) { return; }

            var it = lvContracts.SelectedItems[0];
            var c = it.Tag as RentalContract;
            if (dtpStartTime.Value >= dtpEndTime.Value)
            {
                MessageBox.Show("Contract start time can't be >= end time.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            c.StartTime = dtpStartTime.Value;
            c.EndTime = dtpEndTime.Value;
            c.HourlyRate = (double)nudRate.Value;
            FillContractItem(it);
        }

        private void btnReloadContract_Click(object sender, EventArgs e)
        {
            if (lvContracts.SelectedItems.Count == 0) { return; }

            var it = lvContracts.SelectedItems[0];
            var c = it.Tag as RentalContract;
            FillContractInfo(c);
        }

        private void btnShowRevenue_Click(object sender, EventArgs e)
        {
            double r = 0;
            foreach (var c in RentalContract.GetAllContracts())
            {
                r += c.Revenue;
            }
            MessageBox.Show($"The total revenue is {r:0.00}.", "Revenue", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lvContracts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvContracts.SelectedItems.Count == 0)
            {
                dtpStartTime.Value = DateTime.Now;
                dtpEndTime.Value = DateTime.Now;
                nudRate.Value = nudRate.Minimum;
            }
            else
            {
                var it = lvContracts.SelectedItems[0];
                var c = it.Tag as RentalContract;
                FillContractInfo(c);
            }
        }

        private void btnSaveEmployee_Click(object sender, EventArgs e)
        {
            if ((txtUsername.Text.Length == 0 || txtUsername.Text == Employee.CurrentUser.Username) &&
                (txtNPass.Text.Length == 0 || txtNPass.Text == Employee.CurrentUser.Password)) { return; }
            var errs = new List<string>();
            if (txtCPass.Text != Employee.CurrentUser.Password) { errs.Add("Current password doesn't match logged in employee password"); }
            if (txtUsername.Text.Length != 0 && txtUsername.Text != Employee.CurrentUser.Username)
            {
                using (var con = DbManager.GetConnection())
                using (var cmd = new SQLiteCommand("SELECT id FROM Employee WHERE Username = @un", con))
                {
                    cmd.Parameters.AddWithValue("@un", txtUsername.Text);
                    var userId = cmd.ExecuteScalar();
                    if (userId != null)
                    {
                        errs.Add($"Username {txtUsername.Text} is already used by employee with id {userId}.");
                    }
                }
            }
            if (errs.Count == 0)
            {
                if (txtUsername.Text.Length != 0 && txtUsername.Text != Employee.CurrentUser.Username)
                {
                    Employee.CurrentUser.Username = txtUsername.Text;
                    for (int i = 0; i < lvContracts.Items.Count; i++)
                    {
                        var it = lvContracts.Items[i];
                        var c = it.Tag as RentalContract;
                        if (c.Employee.Id == Employee.CurrentUser.Id)
                        {
                            FillContractItem(it);
                        }
                    }
                }
                if (txtNPass.Text.Length != 0)
                {
                    Employee.CurrentUser.Password = txtNPass.Text;
                }
            }
            else
            {
                MessageBox.Show(string.Join("\n", errs), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReloadEmployee_Click(object sender, EventArgs e) => txtUsername.Text = Employee.CurrentUser.Username;

        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to delete employee {Employee.CurrentUser.Username} ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            Employee.CurrentUser.Delete();
            Employee.Logout();
            Close();
        }

        private void btnLogout_Click(object sender, EventArgs e) => Close();
    }
}
