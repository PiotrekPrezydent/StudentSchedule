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
using StudentScheduleBackend.Interfaces;
using StudentScheduleBackend.Repositories;
using StudentScheduleClient.Windows;

namespace StudentScheduleClient.AdminPages
{
    /// <summary>
    /// Logika interakcji dla klasy AdminStudentProgramPage.xaml
    /// </summary>
    public partial class AdminStudentProgramPage : Page
    {
        StudentProgramRepository _repository;
        public AdminStudentProgramPage()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new(App.DBContext);
            Entities.ItemsSource = _repository.GetAll();
        }

        void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is StudentProgram entity))
                return;

            var editWindow = new EditPopup(typeof(StudentProgram), entity, e => _repository.Update((StudentProgram)e))
            {
                Owner = Window.GetWindow(this)
            };
            bool? result = editWindow.ShowDialog();
            if (result == true)
            {
                MessageBox.Show($"Saved changes for student program connection: StudentID: {entity.StudentId} ProgramID {entity.ProgramId}");
            }
            Entities.ItemsSource = _repository.GetAll();
        }

        void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is StudentProgram entity))
                return;

            try
            {
                _repository.Delete(entity.StudentId, entity.ProgramId);
                MessageBox.Show($"Removed connection for student program connection: StudentID: {entity.StudentId} ProgramID {entity.ProgramId}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when trying to delete:\n{ex.Message}");
            }
            finally
            {
                Entities.ItemsSource = _repository.GetAll();
            }
        }

        void AddButton_Click(object sender, RoutedEventArgs e)
        {
            StudentProgram entity = new(1, 1);
            var editWindow = new EditPopup(typeof(StudentProgram), entity, e => _repository.Add((StudentProgram)e), true)
            {
                Owner = Window.GetWindow(this)
            };
            bool? result = editWindow.ShowDialog();
            if (result == true)
            {
                MessageBox.Show($"added new connection for student and program");
            }
            Entities.ItemsSource = _repository.GetAll();
        }
    }
}
