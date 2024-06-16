using RequestsManagement.Models;
using System;
using System.Text;
using System.Windows;

namespace RequestsManagement.AppWindows.AdminWindows
{
    public partial class CreatingNewClientWindow : Window
    {
        public CreatingNewClientWindow()
        {
            InitializeComponent();
        }

        private void CreateClient_OnClick(Object sender, RoutedEventArgs e)
        {
            StringBuilder errors = CheckValidData();
            if (errors.Length == 0)
            {
                try
                {
                    using (RequestsManagementEntities context = new RequestsManagementEntities())
                    {
                        Users newUser = new Users
                        {
                            FIO = clientFio.Text,
                            Tel = clientTel.Text,
                            UserLogin = clientLogin.Text,
                            UserPassword = clientPassword.Password,
                            UserRoleId = (Int32)UserRole.Client
                        };
                        context.Users.Add(newUser);
                        context.SaveChanges();
                        MessageBox.Show("Новаый клиент успешно создан!",
                            "Клиент успешно создан", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else MessageBox.Show($"При заполнении формы " +
                                 $"произошли следующие проблемы:\n\n{errors}", "Внимание!",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private StringBuilder CheckValidData()
        {
            StringBuilder errors = new StringBuilder();

            if (clientFio.Text.Length == 0) { errors.AppendLine("Вы не ввели ФИО клиента!"); }
            if (clientTel.Text.Length == 0) { errors.AppendLine("Вы не ввели телефон клиента!"); }
            if (clientLogin.Text.Length == 0) { errors.AppendLine("Вы не ввели логин клиента!"); }
            if (clientPassword.Password.Length == 0) { errors.AppendLine("Вы не ввели пароль клиента!"); }

            return errors;
        }
    }
}
