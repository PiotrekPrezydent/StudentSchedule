using System.Windows;
using StudentScheduleBackend.Entities;
using StudentScheduleClient.AdminPages;

namespace StudentScheduleClient.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new AdminStartPage());
        }

        void StartBTN_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AdminStartPage());
        }

        void AccountBTN_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AdminEntityPage<Account>());
        }

        void ClassBTN_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new AdminClassPage());
        }

        void ClassroomBTN_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new AdminClassroomPage());
        }

        void ProgramBTN_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new AdminProgramPage());
        }

        void StudentBTN_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new AdminStudentPage());
        }

        void StudentProgramBTN_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new AdminStudentProgramPage());
        }

        void SubjectBTN_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new AdminSubjectPage());
        }
    }
}
