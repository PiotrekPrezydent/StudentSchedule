using System.Windows;
using System.Windows.Controls;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Repositories;
using StudentScheduleClient.Windows;

namespace StudentScheduleClient.AdminPages
{
    /// <summary>
    /// Logika interakcji dla klasy AdminProgramPage.xaml
    /// </summary>
    public partial class AdminProgramPage : Page
    {
        ProgramRepository _repository;
        public AdminProgramPage()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new(App.DBContext);
            Entities.ItemsSource = _repository.GetAll();
        }

        void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is Program entity))
                return;

            var editWindow = new EditPopup(typeof(Program), entity, e => _repository.Update((Program)e))
            {
                Owner = Window.GetWindow(this)
            };
            bool? result = editWindow.ShowDialog();
            if (result == true)
            {
                MessageBox.Show($"Saved changes for program: {entity.Id}");
            }
            Entities.ItemsSource = _repository.GetAll();
        }

        void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is Program entity))
                return;

            try
            {
                _repository.Delete(entity.Id);
                MessageBox.Show($"Removed program with id: {entity.Id}");
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
            Program entity = new(_repository.GetAll().Last().Id + 1, null);
            var editWindow = new EditPopup(typeof(Program), entity, e => _repository.Add((Program)e), true)
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
