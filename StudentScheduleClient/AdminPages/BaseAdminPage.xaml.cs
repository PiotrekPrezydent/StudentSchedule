using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Extensions;
using StudentScheduleClient.Windows;

namespace StudentScheduleClient.AdminPages
{
    /// <summary>
    /// Logika interakcji dla klasy BaseAdminPage.xaml
    /// </summary>
    public abstract partial class BaseAdminPage : Page
    {
        protected List<KeyValuePair<string, object>> _filters;
        protected List<Entity> _entities;

        Type _pageType;

        protected BaseAdminPage(Type t)
        {
            if (!typeof(Entity).IsAssignableFrom(t))
                throw new ArgumentException("Type must derive from Entity", nameof(t));

            InitializeComponent();
            DataContext = this;
            _pageType = t;
            _filters = new();
            _entities = new();
            int i = 1;
            foreach(var prop in _pageType.GetProperties())
            {
                if (typeof(Entity).IsAssignableFrom(prop.PropertyType))
                    continue;

                var newColumn = new GridViewColumn
                {
                    Header = prop.Name,
                    Width = 100,
                    DisplayMemberBinding = new Binding($"{prop.Name}")
                };
                ColumnContainer.Columns.Insert(i, newColumn);
                i++;
            }
        }

        protected abstract bool OnEdit(object o);

        protected abstract bool OnAdd(object o);

        protected abstract bool OnDelete(object o);

        void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(_pageType.IsInstanceOfType(btn.Tag)))
                return;
            var entity = btn.Tag as Entity;

            if (entity == null)
                return;

            var editWindow = new EditPopup(_pageType, entity, OnEdit);
            editWindow.Owner = Window.GetWindow(this);

            bool? result = editWindow.ShowDialog();

            if (result == true)
                MessageBox.Show($"Saved changes for {_pageType.Name}: {entity.Id}");

            Entities.ItemsSource = _entities.PanDa5ZaTenSuperFilter(_filters);
        }

        void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(_pageType.IsInstanceOfType(btn.Tag)))
                return;
            var entity = btn.Tag as Entity;

            if (entity == null)
                return;

            try
            {
                OnDelete(entity);
                MessageBox.Show($"Removed {_pageType.Name} with id: {entity.Id}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when trying to delete:\n{ex.Message}");
            }
            finally
            {
                Entities.ItemsSource = _entities.PanDa5ZaTenSuperFilter(_filters);
            }
        }
        //WIP
        void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //if (!(sender is Button btn) || !(_pageType.IsInstanceOfType(btn.Tag)))
            //    return;

            //var entity = btn.Tag as Entity;

            //if (entity == null)
            //    return;

            //var editWindow = new EditPopup(_pageType, entity, OnAdd,true);
            //editWindow.Owner = Window.GetWindow(this);
            //bool? result = editWindow.ShowDialog();
            //if (result == true)
            //{
            //    MessageBox.Show($"added new entity with id: {_repository.GetAll().Last().Id}");
            //}
            //Entities.ItemsSource = _entities.PanDa5ZaTenSuperFilter(_filters);
        }

        void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            //WIP
        }
    }
}