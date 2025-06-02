using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.IdentityModel.Tokens;
using StudentScheduleBackend.Attributes;
using StudentScheduleBackend.Entities;

namespace StudentScheduleClient.Popups
{
    /// <summary>
    /// Logika interakcji dla klasy ShowColumnsPopup.xaml
    /// </summary>
    public partial class ShowColumnsPopup : Window
    {
        public List<KeyValuePair<string, string>> ReadedValues;

        public ShowColumnsPopup(Type entityType, PopupType type,List<KeyValuePair<string, string>> values)
        {
            InitializeComponent();
            GenerateColumns(entityType, type);
            ReadedValues = new();

            //We cant set here our Id column
            if(type == PopupType.Add || type == PopupType.Edit)
            {
                StackPanel s = (StackPanel)PropertiesContainer.Children[0];
                Grid g = (Grid)s.Children[0];
                TextBox t = (TextBox)g.Children[1];
                t.Background = Brushes.LightGray;
                t.IsReadOnly = true;
                if(type == PopupType.Add)
                    t.Text = values[0].Value;
            }

            if (type == PopupType.Edit || type == PopupType.Filter)
            {
                //set column values
                for (int i = 0; i < PropertiesContainer.Children.Count; i++)
                {
                    //this should never happen i think lol
                    if (i > values.Count - 1)
                        return;

                    StackPanel si = (StackPanel)PropertiesContainer.Children[i];
                    Grid gi = (Grid)si.Children[0];
                    var control = gi.Children.OfType<FrameworkElement>().FirstOrDefault(c => Grid.GetColumn(c) == 3);
                    string propName = (string)control.Tag;
                    var propValue = values.First(e => e.Key == propName).Value;
                    var controlType = control.GetType();
                    if(controlType == typeof(ComboBox))
                    {
                        var cb = (ComboBox)control;
                        cb.SelectedIndex = propValue.IsNullOrEmpty() ? -1 :cb.Items.IndexOf(propValue);

                    }
                    else if(controlType == typeof(CheckBox))
                    {
                        (control as CheckBox).IsChecked = (propValue == "True");
                    }
                    else
                    {
                        (control as TextBox).Text = propValue;
                    }
                }
            }
        }

        void GenerateColumns(Type entityType, PopupType pType)
        {
            foreach (var prop in entityType.GetProperties())
            {
                if (typeof(Entity).IsAssignableFrom(prop.PropertyType))
                    continue;
                if (prop.PropertyType.IsGenericType && typeof(ICollection<>).IsAssignableFrom(prop.PropertyType.GetGenericTypeDefinition()))
                    continue;

                string name = prop.Name;

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

                //front-end validation:
                // CONTROL GENERATION
                FrameworkElement control;
                var type = prop.PropertyType;
                var attr = prop.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(ForeignKeyOf));
                if (attr != null)
                {
                    var foreignEntityType = (Type)attr.ConstructorArguments.First().Value!;
                    var source = App.DBContext.GetEntitiesByType(foreignEntityType).Select(e => e.Id.ToString()).ToList();
                    if (pType == PopupType.Filter)
                        source.Insert(0, "");
                    control = new ComboBox
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        ItemsSource = source,
                        
                    };
                }
                else
                {
                    if (type == typeof(bool))
                    {
                        control = new CheckBox { VerticalAlignment = VerticalAlignment.Center };
                    }
                    else if (type == typeof(TimeSpan))
                    {
                        control = new TextBox
                        {
                            VerticalAlignment = VerticalAlignment.Center,
                            ToolTip = "Format: hh:mm"
                        };
                    }
                    else if (type == typeof(int) || type == typeof(double) || type == typeof(decimal))
                    {
                        control = new TextBox
                        {
                            VerticalAlignment = VerticalAlignment.Center,
                        };
                        control.PreviewTextInput += (s, e) => e.Handled = !char.IsDigit(e.Text, 0);
                    }
                    else
                    {
                        control = new TextBox { VerticalAlignment = VerticalAlignment.Center };
                    }
                }
   

                // Tag property name to identify later
                control.Tag = name;

                Grid.SetColumn(control, 3);
                grid.Children.Add(control);
                container.Children.Add(grid);
                //id column should be at top
                if (name == "Id")
                    PropertiesContainer.Children.Insert(0, container);
                else
                    PropertiesContainer.Children.Add(container);

            }
        }

        void SetReadedValues()
        {
            ReadedValues.Clear();
            for (int i = 0; i < PropertiesContainer.Children.Count; i++)
            {
                StackPanel si = (StackPanel)PropertiesContainer.Children[i];
                Grid gi = (Grid)si.Children[0];
                var control = gi.Children.OfType<FrameworkElement>().FirstOrDefault(c => Grid.GetColumn(c) == 3);
                var controlType = control.GetType();
                KeyValuePair<string, string> kvp;
                if (controlType == typeof(ComboBox))
                {
                    var cb = (ComboBox)control;
                    if(cb.SelectedIndex != -1)
                        kvp = new((string)control.Tag, cb.SelectedItem.ToString());
                    else
                        kvp = new((string)control.Tag, "");

                }
                else if (controlType == typeof(CheckBox))
                {
                    kvp = new((string)control.Tag, (control as CheckBox).IsChecked.ToString());
                }
                else
                {
                    kvp = new((string)control.Tag, (control as TextBox).Text);
                }
                ReadedValues.Add(kvp);
            }
        }


        void Accept_Click(object sender, RoutedEventArgs e)
        {
           SetReadedValues();
           DialogResult = true;
        }

        void Cancel_Click(object sender, RoutedEventArgs e)
        {
           DialogResult = false;
        }
    }
}
