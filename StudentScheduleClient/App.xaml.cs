using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend;
using StudentScheduleBackend.Entities;
using StudentScheduleClient.Windows;

namespace StudentScheduleClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Context DBContext;
        public static Student CurrentStudent;

        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            base.OnStartup(e);

            // Create DbContext with your debug connection string
            var options = new DbContextOptionsBuilder<Context>()
                .UseSqlServer($"Server=localhost\\SQLEXPRESS;Database=StudentSchedule;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            using (var context = new Context(options))
            {
               context.Database.EnsureDeleted();
               context.Database.Migrate();

                string seedFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "seed");

                Seeder.SeedAllFromJson(context, seedFolder);
            }
#endif
        }


        public static void StartStudentSession(LoginWindow instance,Student currentStudent)
        {
            CurrentStudent = currentStudent;
            StudentWindow window = new();
            window.Show();
            instance.Close();
        }

        public static void StartAdminSession(LoginWindow instance)
        {
            AdminWindow window = new AdminWindow();
            window.Show();
            instance.Close();
        }
    }
}
