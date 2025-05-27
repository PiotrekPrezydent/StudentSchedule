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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Repositories;

namespace StudentScheduleClient.StudentPages
{
    /// <summary>
    /// Logika interakcji dla klasy StudentSchedulePage.xaml
    /// </summary>
    public partial class StudentSchedulePage : Page
    {
        DayOfWeek _currentDay;
        IEnumerable<Program> _studentPrograms;
        IEnumerable<Class> _studentClasses;

        public ObservableCollection<KeyValuePair<Program, IEnumerable<Class>>> GroupedProgramsWithClasses { get; set; }

        public StudentSchedulePage()
        {
            InitializeComponent();
            DataContext = this;
            _currentDay = DateTime.Now.DayOfWeek;
            WeekDayNameTextBlock.Text = _currentDay.ToString();

            int studentId = App.CurrentStudent.Id;
            GroupedProgramsWithClasses = new();

            StudentProgramRepository sprep = new(App.DBContext);
            _studentPrograms = sprep.GetAll().Where(e => e.StudentId == studentId).Select(e => e.Program);

            ClassRepository crep = new(App.DBContext);
            _studentClasses = crep.GetAll().Where(e => _studentPrograms.Any(p => p.Id == e.ProgramId));

            SetClassesForCurrentDay();
        }


        void SetClassesForCurrentDay()
        {
            GroupedProgramsWithClasses.Clear();
            foreach (var program in _studentPrograms)
            {
                var classesForProgram = _studentClasses.Where(e => e.ProgramId == program.Id && e.Weekday == _currentDay.ToString());
                GroupedProgramsWithClasses.Add(new(program,classesForProgram));
            }
        }

        void PreviousDayBTN_Click(object sender, RoutedEventArgs e)
        {
            _currentDay -= 1;
            if ((int)_currentDay < 0)
                _currentDay = (DayOfWeek)6;

            WeekDayNameTextBlock.Text = _currentDay.ToString();
            SetClassesForCurrentDay();
        }

        void NextDayBTN_Click(object sender, RoutedEventArgs e)
        {
            _currentDay += 1;
            if ((int)_currentDay > 6)
                _currentDay = (DayOfWeek)0;

            WeekDayNameTextBlock.Text = _currentDay.ToString();
            SetClassesForCurrentDay();
        }
    }
}
