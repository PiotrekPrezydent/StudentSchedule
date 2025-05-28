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
        }

        void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var cancelToken = new CancellationTokenSource();
            var login = LoginBox.Text;
            var password = PasswordBox.Text;

#if DEBUG
            if (login == "ADMIN" && password == "ADMIN")
            {
                cancelToken.Cancel();
                App.StartAdminSession(this);
                if (App.DBContext == null)
                    App.DBContext = Context.Initialize(null, false);
                return;
            }
#endif
            if (App.DBContext == null)
                App.DBContext = Context.Initialize(null, false);
            ShowLoadingText();


            AccountRepository ac = new(App.DBContext);

            bool logged = false;
            Account acc;
            try
            {
                acc = ac.GetAll().FirstOrDefault(e => e.Login == login && e.Password == password);
                logged = !EqualityComparer<Account>.Default.Equals(acc, default(Account));
            }
            catch(Exception ex)
            {
                cancelToken.Cancel();
                ErrorMessage.Text = $"{ex}";
                return;
            }

            if (logged)
            {
                cancelToken.Cancel();
                App.CurrentStudent = acc.Student;
                App.StartStudentSession(this);
            }
            else
            {
                cancelToken.Cancel();
                ErrorMessage.Text = $"Zły login lub hasło";
            }

            async Task ShowLoadingText()
            {
                while (true)
                {
                    cancelToken.Token.ThrowIfCancellationRequested();
                    ErrorMessage.Text = "TRWA ŁĄCZENIE";
                    for (int i = 0; i < 3; i++)
                    {
                        cancelToken.Token.ThrowIfCancellationRequested();
                        ErrorMessage.Text += ".";
                        await Task.Delay(1000, cancelToken.Token);
                    }
                }
            }

        }


    }
}