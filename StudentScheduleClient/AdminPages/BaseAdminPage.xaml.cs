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
        Type _pageType;
        protected Entity? _btnContext; 

        protected BaseAdminPage(Type t)
        {
            if (!typeof(Entity).IsAssignableFrom(t))
                throw new ArgumentException("Type must derive from Entity", nameof(t));

            InitializeComponent();
            DataContext = this;
            _pageType = t;
            int i = 1;
            foreach(var prop in _pageType.GetProperties())
            {
                if (typeof(Entity).IsAssignableFrom(prop.PropertyType))
                    continue;

                if (prop.Name == "Id")
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

        protected abstract void OnEdit();

        protected abstract void OnDelete();

        protected abstract void OnAdd();

        protected abstract void OnFilter();

        void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(_pageType.IsInstanceOfType(btn.Tag)))
                return;
            _btnContext = btn.Tag as Entity;

            OnEdit();
        }

        void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(_pageType.IsInstanceOfType(btn.Tag)))
                return;
            _btnContext = btn.Tag as Entity;

            OnDelete();
        }

        void AddButton_Click(object sender, RoutedEventArgs e) => OnAdd();

        void FilterButton_Click(object sender, RoutedEventArgs e) => OnFilter();
    }
}