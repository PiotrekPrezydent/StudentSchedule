using System.Windows;
using System.Windows.Controls;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Repositories;
using StudentScheduleClient.Windows;

namespace StudentScheduleClient.AdminPages
{
    /// <summary>
    /// Logika interakcji dla klasy AdminSubjectPage.xaml
    /// </summary>
    public partial class AdminSubjectPage : Page
    {
        SubjectRepository _repository;
        public AdminSubjectPage()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new(App.DBContext);
            Entities.ItemsSource = _repository.GetAll();
        }

        void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is Subject entity))
                return;

            var editWindow = new EditPopup(typeof(Subject), entity, e => _repository.Update((Subject)e))
            {
                Owner = Window.GetWindow(this)
            };
            bool? result = editWindow.ShowDialog();
            if (result == true)
            {
                MessageBox.Show($"Saved changes for subject: {entity.Id}");
            }
            Entities.ItemsSource = _repository.GetAll();
        }

        void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is Subject entity))
                return;

            try
            {
                _repository.Delete(entity.Id);
                MessageBox.Show($"Removed subject with id: {entity.Id}");
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
            Subject entity = new(_repository.GetAll().Last().Id + 1, null);
            var editWindow = new EditPopup(typeof(Subject), entity, e => _repository.Add((Subject)e),true)
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
