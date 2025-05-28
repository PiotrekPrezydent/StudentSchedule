using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace StudentScheduleClient.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy FilterPopup.xaml
    /// </summary>
    public partial class FilterPopup : Window
    {
        public List<KeyValuePair<string, object>> ResultFilters { get; private set; }
        List<(CheckBox checkBox, TextBox textBox, PropertyInfo propertyInfo)> _filterControls = new();
        public FilterPopup(Type entityType, List<KeyValuePair<string, object>> currentFilters)
        {
            InitializeComponent();
            ResultFilters = currentFilters;
            var simpleProperties = entityType.GetProperties().Where(p => p.PropertyType == typeof(string) || p.PropertyType.IsValueType);
            _filterControls = new();
            foreach (var property in simpleProperties)
                AddFiltrablePropertiesWithUsedFilters(property);
        }

        void AddFiltrablePropertiesWithUsedFilters(PropertyInfo property)
        {
            string name = property.Name;

            StackPanel container = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            var grid = new Grid
            {
                Margin = new Thickness(0, 5, 0, 5),
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            // Define columns
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // 0
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }); // 1
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // 2
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) }); // 3
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // 4
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // 5
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // 6

            // Label in column 1
            var label = new Label
            {
                Content = $"{name}",
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(label, 1);
            grid.Children.Add(label);

            // TextBox in column 3
            var textBox = new TextBox
            {
                Text = ResultFilters.Any(e => e.Key == name) ? ResultFilters.First(e => e.Key == name).Value.ToString() : "",
                VerticalAlignment = VerticalAlignment.Center,
            };
            Grid.SetColumn(textBox, 3);
            grid.Children.Add(textBox);

            // CheckBox in column 5
            var checkBox = new CheckBox
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                IsChecked = ResultFilters.Any(e => e.Key == name)
            };
            Grid.SetColumn(checkBox, 5);
            grid.Children.Add(checkBox);

            container.Children.Add(grid);
            _filterControls.Add((checkBox, textBox, property));
            PropertiesContainer.Children.Add(container);
        }


        void Button_Click(object sender, RoutedEventArgs e)
        {
            ResultFilters = new();
            foreach (var filterControl in _filterControls)
            {
                if (filterControl.checkBox.IsChecked == false)
                    continue;

                KeyValuePair<string, object> kvp = new(filterControl.propertyInfo.Name, ConvertFromString(filterControl.textBox.Text, filterControl.propertyInfo.PropertyType));
                ResultFilters.Add(kvp);
            }
            DialogResult = true;
        }

        object ConvertFromString(string text, Type type)
        {
            if (type == typeof(string))
                return text;
            else if (type.IsEnum)
                return Enum.Parse(type, text);
            else if (type == typeof(Guid))
                return Guid.Parse(text);
            else if (type == typeof(bool))
                return bool.Parse(text);
            else if (Nullable.GetUnderlyingType(type) != null)
            {
                // Nullable type, get underlying type and convert
                Type underlyingType = Nullable.GetUnderlyingType(type);
                if (string.IsNullOrEmpty(text))
                    return null;
                return Convert.ChangeType(text, underlyingType);
            }
            else
            {
                // For int, double, DateTime, etc.
                return Convert.ChangeType(text, type);
            }
        }
    }
}
