using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RequestsManagement.AppWindows.AdminWindows;
using RequestsManagement.AppWindows.ClientWindows;
using RequestsManagement.AppWindows.ManagerWindows;
using RequestsManagement.AppWindows.MechanicWindows;
using RequestsManagement.GlobalStorage;
using RequestsManagement.Models;

namespace RequestsManagement.AppWindows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_OnClick(Object sender, RoutedEventArgs e)
        {
            String login = loginTextBox.Text;
            String password = passwordPasswordBox.Password;

            Users user = RequestsManagementEntities.GetContext()
                .Users.FirstOrDefault(u => u.UserLogin == login 
                                           && u.UserPassword == password);
            if (user == null)
            {
                MessageBox.Show("Логин или пароль введены неверно!", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UserStorage.AuthUser = user;

            switch ((UserRole)UserStorage.AuthUser.UserRoleId)
            {
                case UserRole.Manager:
                    ManagerRequestsWindow managerWindow = new ManagerRequestsWindow();
                    managerWindow.Show();
                    this.Close();
                    break;
                case UserRole.Mechanic:
                    MechanicRequestsWindow window = new MechanicRequestsWindow();
                    window.Show();
                    this.Close();
                    break;
                case UserRole.Admin:
                    AdminRequestsWindow adminWindow = new AdminRequestsWindow();
                    adminWindow.Show();
                    this.Close();
                    break;
                case UserRole.Client:
                    ClientRequestsWindow clientWindow = new ClientRequestsWindow();
                    this.Close();
                    clientWindow.Show();
                    break;
                default:
                    MessageBox.Show("Неизвестная роль пользователя", 
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private void MainWindow_OnKeyDown(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                loginButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}
