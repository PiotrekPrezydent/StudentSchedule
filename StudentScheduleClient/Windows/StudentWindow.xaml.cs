using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using StudentScheduleClient.StudentPages;

namespace StudentScheduleClient.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        public StudentWindow()
        {
            InitializeComponent();
            StudentIndex.Text += $"{App.CurrentStudent.IndexNumber}";
            MainFrame.Navigate(new StudentStartPage());

        }

        void StartBTN_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StudentStartPage());
        }

        private void ScheduleBTN_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StudentSchedulePage());
        }

        private void ProgramsBTN_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StudentProgramsPage());
        }
    }
}
