using Quality_Control.Forms.Quality;
using Quality_Control.Forms.Register;
using Quality_Control.Repository;
using Quality_Control.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Quality_Control.Forms.Login
{
    /// <summary>
    /// Logika interakcji dla klasy LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        private readonly string _loginPath = @"\Data\login.txt";
        private readonly List<string> _logins;

        public LoginForm()
        {
            InitializeComponent();

            _logins = GetLogins();
            CmbUserName.ItemsSource = _logins;
            if (_logins.Count > 0)
            {
                CmbUserName.Text = _logins[0];
            }
        }

        private void BtnSubbmit_Click(object sender, RoutedEventArgs e)
        {
            UserRepository repository = new UserRepository();
            SaveLogins();
            User user = repository.GetUserByLoginAndPassword(CmbUserName.Text, TxtPassword.Password);

            if (user == null)
            {
                _ = MessageBox.Show("Nieprawidłowy login lub hasło. Spróbuj ponownie",
                    "Błąd logowania", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (user.IsActive)
            {
                _ = UserSingleton.CreateInstance(user.Id, user.Name, user.Surname, user.Email, user.Login, user.Permission, user.Identifier, user.IsActive);
                QualityForm quality = new QualityForm();
                quality.Show();
                Close();
            }
            else
            {
                _ = MessageBox.Show("Użytkownik: '" + user.Login + "' jest jeszcze nieaktywny. Skontaktuj się z administratorem.",
                    "Brak uprawnień", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSubbmit_Click(null, null);
            }
        }

        private List<string> GetLogins()
        {
            List<string> logins = new List<string>();
            if (File.Exists(Environment.CurrentDirectory + _loginPath))
            {
                StreamReader file = new StreamReader(Environment.CurrentDirectory + _loginPath);
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    logins.Add(line);
                }
                file.Close();
            }
            return logins;
        }

        private void SaveLogins()
        {
            var login = CmbUserName.Text;
            var file = Environment.CurrentDirectory + _loginPath;

            _logins.Sort();
            _logins.Remove(login);
            if (!_logins.Contains(login))
            {
                _logins.Insert(0, login);
            }

            if (!Directory.Exists(Path.GetDirectoryName(file)))
                Directory.CreateDirectory(Path.GetDirectoryName(file));

            File.WriteAllLines(Environment.CurrentDirectory + _loginPath, _logins);
        }
    }
}
