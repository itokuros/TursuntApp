using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Manager.MainFrame = MainFrame;
            MainFrame.Navigate(new HIPage());

            if (LoginWindow.CurrentUser != null)
            {
                tbUserInfo.Text = $"Пользователь: {LoginWindow.CurrentUser.full_name} ({LoginWindow.CurrentUser.role})";
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow.CurrentUser = null;
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
