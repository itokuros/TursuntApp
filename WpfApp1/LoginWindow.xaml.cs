using System;
using System.Linq;
using System.Windows;
using Tursunt;

namespace WpfApp1
{
    public partial class LoginWindow : Window
    {
        public static users CurrentUser { get; set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text.Trim();
            string password = pbPassword.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                tbError.Text = "Введите логин и пароль!";
                return;
            }

            try
            {
                using (var db = new НастяEntities())
                {
                    var user = db.users.FirstOrDefault(u => u.login == login && u.password_hash == password);

                    if (user == null)
                    {
                        tbError.Text = "Неверный логин или пароль!";
                        return;
                    }

                    CurrentUser = user;

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }   
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // TODO: окно регистрации
            MessageBox.Show("Функция регистрации в разработке");
        }
    }
}