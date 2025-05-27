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
        List<ListBox> _classesContainer;
        public StudentSchedulePage()
        {
            InitializeComponent();
            _currentDay = DateTime.Now.DayOfWeek;
            WeekDayNameTextBlock.Text = _currentDay.ToString();
            _classesContainer = new();

            int studentId = App.CurrentStudent.Id;

            StudentProgramRepository sprep = new(App.DBContext);
            _studentPrograms = sprep.GetAll().Where(e => e.StudentId == studentId).Select(e => e.Program);

            ClassRepository crep = new(App.DBContext);
            _studentClasses = crep.GetAll().Where(e => _studentPrograms.Any(p => p.Id == e.ProgramId));

            foreach (var program in _studentPrograms)
            {
                //becouse of this we cant go async :CCC
                var label = new Label
                {
                    Content = program.Name,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Margin = new Thickness(0, 10, 0, 5)
                };

                var listBox = new ListBox
                {
                    Margin = new Thickness(10, 0, 0, 20),
                };

                _classesContainer.Add(listBox);
                ProgramsContainer.Children.Add(label);
                ProgramsContainer.Children.Add(listBox);
            }
            SetClassesForCurrentDay();
        }

        //async void LoadData()
        //{
        //    await Task.Run(() =>
        //    {

        //    });
        //}

        void SetClassesForCurrentDay()
        {
            for(int i = 0; i < _classesContainer.Count; i++)
            {
                var programId = _studentPrograms.ElementAt(i).Id;
                var container = _classesContainer[i];
                container.Items.Clear();
                var classesForCurrentDay = _studentClasses.Where(e => e.ProgramId == programId && e.Weekday == _currentDay.ToString());
                foreach(var c in classesForCurrentDay)
                {
                    var str = $"{c.Subject.Name}\t\t{c.Classroom.Building}-{c.Classroom.RoomNumber}\tod: {c.StartTime} do: {c.EndTime}";
                    container.Items.Add(str);
                }
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
