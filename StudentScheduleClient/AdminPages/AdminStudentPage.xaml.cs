using System.Windows;
using System.Windows.Controls;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Repositories;
using StudentScheduleClient.Windows;

namespace StudentScheduleClient.AdminPages
{
    /// <summary>
    /// Logika interakcji dla klasy AdminStudentPage.xaml
    /// </summary>
    public partial class AdminStudentPage : Page
    {
        StudentRepository _repository;
        public AdminStudentPage()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new(App.DBContext);
            Entities.ItemsSource = _repository.GetAll();
        }

        void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is Student entity))
                return;

            var editWindow = new EditPopup(typeof(Student), entity, e => _repository.Update((Student)e))
            {
                Owner = Window.GetWindow(this)
            };
            bool? result = editWindow.ShowDialog();
            if (result == true)
            {
                MessageBox.Show($"Saved changes for student: {entity.Id}");
            }
            Entities.ItemsSource = _repository.GetAll();
        }

        void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is Student entity))
                return;

            try
            {
                _repository.Delete(entity.Id);
                MessageBox.Show($"Removed student with id: {entity.Id}");
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
    }
}
