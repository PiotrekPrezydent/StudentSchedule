using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Interfaces;
using StudentScheduleClient.Windows;

namespace StudentScheduleClient.AdminPages
{
    //internal abstract class AbstractAdminPage<T> : Page
    //{
    //    IRepository<T> _repository;

    //    public AbstractAdminPage(IRepository<T> repository)
    //    {
    //        DataContext = this;
    //        _repository = repository;
    //        Entities.ItemsSource = _repository.GetAll();
    //    }

    //    void EditButton_Click(object sender, RoutedEventArgs e)
    //    {
    //        if (!(sender is Button btn) || !(btn.Tag is T entity))
    //            return;

    //        var editWindow = new EditPopup(typeof(T), entity, e => _repository.Update((T)e))
    //        {
    //            Owner = Window.GetWindow(this)
    //        };
    //        bool? result = editWindow.ShowDialog();
    //        if (result == true)
    //        {
    //            MessageBox.Show($"Saved changes for: {entity}");
    //        }
    //        Entities.ItemsSource = _repository.GetAll();
    //    }

    //    void DeleteButton_Click(object sender, RoutedEventArgs e)
    //    {
    //        if (!(sender is Button btn) || !(btn.Tag is T entity))
    //            return;

    //        try
    //        {
    //            _repository.Delete(T);
    //            MessageBox.Show($"Removed account with id: {entity.Id}");
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show($"Error when trying to delete:\n{ex.Message}");
    //        }
    //        finally
    //        {
    //            Entities.ItemsSource = _repository.GetAll();
    //        }
    //    }

    //}
}
