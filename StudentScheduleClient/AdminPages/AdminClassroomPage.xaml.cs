using System.Windows;
using System.Windows.Controls;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Repositories;
using StudentScheduleClient.Windows;

namespace StudentScheduleClient.AdminPages
{
    /// <summary>
    /// Logika interakcji dla klasy AdminClassroomPage.xaml
    /// </summary>
    public partial class AdminClassroomPage : Page
    {
        ClassroomRepository _repository;
        public AdminClassroomPage()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new(App.DBContext);
            Entities.ItemsSource = _repository.GetAll();
        }

        void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is Classroom entity))
                return;

            var editWindow = new EditPopup(typeof(Classroom), entity, e => _repository.Update((Classroom)e))
            {
                Owner = Window.GetWindow(this)
            };
            bool? result = editWindow.ShowDialog();
            if (result == true)
            {
                MessageBox.Show($"Saved changes for classroom: {entity.Id}");
            }
            Entities.ItemsSource = _repository.GetAll();
        }

        void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is Classroom entity))
                return;

            try
            {
                _repository.Delete(entity.Id);
                MessageBox.Show($"Removed classroom with id: {entity.Id}");
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
            Classroom entity = new(_repository.GetAll().Last().Id + 1, null, null);
            var editWindow = new EditPopup(typeof(Classroom), entity, e => _repository.Add((Classroom)e), true)
            {
                Owner = Window.GetWindow(this)
            };
            bool? result = editWindow.ShowDialog();
            if (result == true)
            {
                MessageBox.Show($"added new entity with id: {_repository.GetAll().Last().Id}");
            }
            Entities.ItemsSource = _repository.GetAll();
        }
    }
}
