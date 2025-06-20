﻿using System.IO;
using System.Reflection;
using System.Windows;
using StudentScheduleBackend;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Repositories;

namespace StudentScheduleClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            PasswordBox.Password = "Haslo123!";
        }

        void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginBox.Text;

            var password = PasswordBox.Password;

            string exeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
            string configPath = Path.Combine(exeFolder, "AppSettings.json");
            string connectionString = "";
            try
            {
                connectionString = Context.BuildConnectionString(configPath,login,password, out int id);
                if (App.DBContext == null)
                    App.DBContext = Context.Initialize(connectionString);

                Repository<Account> ar = new(App.DBContext);
                Repository<Student> sr = new(App.DBContext);
                Account acc = ar.GetById(id);
                Student? s = sr.GetAll().FirstOrDefault(e => e.AccountId == acc.Id);
                if (acc.IsAdmin)
                {
                    App.StartAdminSession(this);
                }
                else
                {
                    if(s == null)
                    {
                        ErrorMessage.Text = $"Konto {acc.Id} nie ma powiązanego studenta, i niejest administratorem";
                        return;
                    }
                    App.StartStudentSession(this, s);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = $"Zły login lub hasło";
                MessageBox.Show(ex.Message);
            }
        }
    }
}