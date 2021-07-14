using Npgsql;
using System;
using System.Data;
using System.Windows;
namespace DbProject
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                e.SetObserved();
                HandleException(e.Exception);
            };
            Application.Current.DispatcherUnhandledException += (s, e) =>
            {
                e.Handled = true;
                HandleException(e.Exception);
            };
            InitializeComponent();
            cbxTables.ItemsSource = DatabaseManager.TablesNames;
            cbxTables.SelectedIndex = 0;
        }
        void HandleException(Exception? ex)
        {
            MessageBox.Show(
$@"Error: {ex?.Message ?? "Unknown error occured"}.
As a result the application will SHUTDOWN.",
"Unexpected Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
        }


        readonly NpgsqlConnection _connection = DatabaseManager.GetConnection();
        DataSet? _tableSet;
        DataTable? _table;
        NpgsqlDataAdapter? _tableAdapter;
        NpgsqlCommandBuilder? _commandBuilder;
        private void RefreshDataGrid()
        {
            string query = $"SELECT * FROM {cbxTables.SelectedItem}";
            if (string.IsNullOrWhiteSpace(txtCondition.Text) == false)
            {
                query += $" WHERE {txtCondition.Text.Trim()}";
            }
            query += ";";
            _tableSet?.Dispose();
            _tableSet = new DataSet();
            _tableAdapter?.Dispose();
            _tableAdapter = new NpgsqlDataAdapter(query, _connection);
            _commandBuilder?.Dispose();
            _commandBuilder = new NpgsqlCommandBuilder(_tableAdapter);
            _tableAdapter.UpdateCommand = _commandBuilder.GetUpdateCommand(true);
            _tableAdapter.InsertCommand = _commandBuilder.GetInsertCommand(true);
            _tableAdapter.DeleteCommand = _commandBuilder.GetDeleteCommand(true);
            _tableAdapter.Fill(_tableSet);
            _table = _tableSet.Tables[0];
            _table.DefaultView.AllowNew = true;
            _table.DefaultView.AllowEdit = true;
            dg.DataContext = _table;
            dg.ItemsSource = _table.DefaultView;
        }

        private void btnViewTable_Click(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_tableAdapter == null) { return; }
            try
            {
                dg.CommitEdit();
                _tableAdapter?.Update(_tableSet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshDataGrid();
            }
        }

        private async void btnPopulateDb_Click(object sender, RoutedEventArgs e)
        {
            await DatabaseManager.PopulateDatabase();
            RefreshDataGrid();
        }

        private void btnAddRow_Click(object sender, RoutedEventArgs e)
        {
            _table?.DefaultView.AddNew();
        }

        private void btnDeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if (_table == null) { return; }
            while (dg.SelectedIndex >= 0 && dg.SelectedIndex < _table!.DefaultView.Count)
            {
                _table!.DefaultView.Delete(dg.SelectedIndex);
            }
        }
    }
}
