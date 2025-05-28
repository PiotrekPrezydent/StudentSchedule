using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StudentScheduleBackend.Entities;


namespace StudentScheduleClient.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy EditPopup.xaml
    /// </summary>
    public partial class EditPopup : Window
    {
        Func<object,bool> _onSave;
        List<KeyValuePair<Type, TextBox>> _constructorParametersWithArguments;
        Type _targetedType;
        public EditPopup(Type entityType,object ob, Func<object,bool> onSave)
        {
            InitializeComponent();
            _onSave = onSave;
            _constructorParametersWithArguments = new();
            _targetedType = entityType;

            var simpleProperties = entityType.GetProperties().Where(p => p.PropertyType == typeof(string) || p.PropertyType.IsValueType);
            foreach (var property in simpleProperties)
                AddEditablePropertyWithUsedValue(property, ob);

        }

        void AddEditablePropertyWithUsedValue(PropertyInfo property,object ob)
        {
            string name = property.Name;
            object? value = property.GetValue(ob)!;

            StackPanel container = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            var grid = new Grid
            {
                Margin = new Thickness(0, 5, 0, 5), // optional spacing between rows
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // 10%
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }); // 20%
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // 10%
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) }); // 50%
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // 10%

            var label = new Label
            {
                Content = $"{name}",
                VerticalAlignment = VerticalAlignment.Center
            };

            Grid.SetColumn(label, 1);
            grid.Children.Add(label);

            var textBox = new TextBox
            {
                Text = value?.ToString() ?? "",
                VerticalAlignment = VerticalAlignment.Center,
                IsReadOnly = name == "Id" ? true : false,
                Background = name == "Id" ? Brushes.LightGray : Brushes.White,
            };

            KeyValuePair<Type, TextBox> parameters = new(property.PropertyType, textBox);
            _constructorParametersWithArguments.Add(parameters);

            Grid.SetColumn(textBox, 3);
            grid.Children.Add(textBox);

            container.Children.Add(grid);
            PropertiesContainer.Children.Add(container);
        }

        void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var o = CreateObjectFromTextBoxes(_targetedType, _constructorParametersWithArguments);
                _onSave(o);
                DialogResult = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Exception when trying to update object:\n{ex}");
                DialogResult = false;
            }
        }

        void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Close without saving
        }
        object CreateObjectFromTextBoxes(Type targetType, List<KeyValuePair<Type, TextBox>> parameters)
        {
            var paramTypes = new Type[parameters.Count];
            var paramValues = new object[parameters.Count];

            for (int i = 0; i < parameters.Count; i++)
            {
                var type = parameters[i].Key;
                var textBox = parameters[i].Value;
                string text = textBox.Text;

                paramTypes[i] = type;

                paramValues[i] = ConvertFromString(text, type);
            }

            ConstructorInfo ctor = targetType.GetConstructor(paramTypes);
            if (ctor == null)
                throw new Exception("No matching constructor found");

            return ctor.Invoke(paramValues);
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
