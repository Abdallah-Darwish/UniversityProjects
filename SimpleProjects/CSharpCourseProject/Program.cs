using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpProject
{
    static class Program
    {
        private static void ValidateDb()
        {
            if (File.Exists(DbInfo.DbFileName)) { return; }
            SQLiteConnection.CreateFile(DbInfo.DbFileName);
            using (var con = new SQLiteConnection(DbInfo.ConnectionString))
            using (var cmd = new SQLiteCommand(DbInfo.CreationScript, con))
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ValidateDb();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
