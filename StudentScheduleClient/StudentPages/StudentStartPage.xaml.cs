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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentScheduleClient.StudentPages
{
    /// <summary>
    /// Logika interakcji dla klasy StudentStartPage.xaml
    /// </summary>
    public partial class StudentStartPage : Page
    {
        public StudentStartPage()
        {
            InitializeComponent();
            WelcomeBlock.Text =
                $"Witaj!\n" +
                $"{App.CurrentStudent.FirstName} {App.CurrentStudent.LastName}\n" +
                $"{App.CurrentStudent.IndexNumber}";
           
        }
    }
}
