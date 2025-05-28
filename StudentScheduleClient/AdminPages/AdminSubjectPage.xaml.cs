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
    }
}
