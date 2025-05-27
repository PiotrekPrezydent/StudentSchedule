using System.Configuration;
using System.Data;
using System.Windows;
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

        public static void StartStudentSession(LoginWindow instance)
        {
            StudentWindow window = new();
            window.Show();
            instance.Close();
        }

    }

}
