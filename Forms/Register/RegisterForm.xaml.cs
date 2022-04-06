using Quality_Control.Dto;
using Quality_Control.Forms.Login;
using Quality_Control.Repository;
using System;
using System.Windows;
using System.Windows.Input;

namespace Quality_Control.Forms.Register
{
    /// <summary>
    /// Logika interakcji dla klasy RegisterForm.xaml
    /// </summary>
    public partial class RegisterForm : Window
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSubbmit_Click(null, null);
            }
        }

        private void BtnSubbmit_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateTextBox()) return;

            UserRepository repository = new UserRepository();
            RegisterStatus status = repository.ExistUserByLogin(TxtLogin.Text);

            if (status == RegisterStatus.Ok)
            {
                string name = TxtName.Text;
                string surName = TxtSurname.Text;
                string email = TxtEmail.Text;
                string login = TxtLogin.Text;
                string identifier = name.ToUpper().Substring(0, 1) + surName.ToUpper().Substring(0, 1);
                string password = TxtPassword.Password;

                UserDto user = new UserDto(name, surName, email, login, "user", identifier, false, DateTime.Now, password);
                _ = repository.Save(user);

                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }

        private bool ValidateTextBox()
        {
            bool name = string.IsNullOrEmpty(TxtName.Text);
            bool surName = string.IsNullOrEmpty(TxtSurname.Text);
            bool email = string.IsNullOrEmpty(TxtEmail.Text);
            bool login = string.IsNullOrEmpty(TxtLogin.Text);
            bool password = string.IsNullOrEmpty(TxtPassword.Password);
            bool passwordRepeat = string.IsNullOrEmpty(TxtPasswordRepeat.Password);

            if (name || surName || email || email || login || password || passwordRepeat)
            {
                _ = MessageBox.Show("Należy wypełnić wszystkie pola.", "Puste pola", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (TxtPassword.Password.Length < 3)
            {
                _ = MessageBox.Show("Hasło jest za krókie. Wprowadź inne hasło.", "Błąd hasła", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (TxtPassword.Password != TxtPasswordRepeat.Password)
            {
                _ = MessageBox.Show("Powtórzone hasło się nie zgadza!", "Błąd hasła", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

    }
}
