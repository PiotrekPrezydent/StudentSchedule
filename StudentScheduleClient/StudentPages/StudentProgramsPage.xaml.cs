using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using StudentScheduleBackend.Entities;
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
            int studentId = App.CurrentStudent.Id;

            Repository<StudentProgram> rep = new(App.DBContext);
            ProgramsListView.ItemsSource = rep.GetAll().Where(e => e.StudentId == studentId).Select(e => e.Program).ToList();
        }

    }
}
