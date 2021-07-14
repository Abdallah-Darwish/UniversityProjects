using System.Windows;

namespace DbProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await DatabaseManager.ValidateDatabase();
        }
    }
}
