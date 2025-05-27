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
using StudentScheduleBackend.Repositories;

namespace StudentScheduleClient.StudentPages
{
    /// <summary>
    /// Logika interakcji dla klasy StudentProgramsPage.xaml
    /// </summary>
    public partial class StudentProgramsPage : Page
    {
        public StudentProgramsPage()
        {
            InitializeComponent();
            ProgramsListBox.Items.Clear();
            LoadData();
        }

        async void LoadData()
        {
            await Task.Run(() =>
            {
                int studentId = App.CurrentStudent.Id;

                StudentProgramRepository rep = new(App.DBContext);
                var programs = rep.GetAll().Where(e => e.StudentId == studentId).Select(e=>e.Program);

                foreach(var program in programs)
                {
                    ProgramsListBox.Dispatcher.Invoke(() =>
                    {
                        ProgramsListBox.Items.Add(program);
                    });
                }
            });
        }
    }
}
