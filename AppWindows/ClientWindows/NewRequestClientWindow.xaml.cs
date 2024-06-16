using RequestsManagement.Models;
using System;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows;
using RequestsManagement.GlobalStorage;

namespace RequestsManagement.AppWindows.ClientWindows
{
    public partial class NewRequestClientWindow : Window
    {
        public NewRequestClientWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_OnClick(Object sender, RoutedEventArgs e)
        {
            StringBuilder errors = CheckValidData();
            if (errors.Length == 0)
            {
                try
                {
                    using (RequestsManagementEntities context = new RequestsManagementEntities())
                    {
                        Requests newRequest = new Requests
                        {
                            StartDate = DateTime.Now,
                            CarType = carType.Text,
                            CarModel = carModel.Text,
                            ProblemDescription = problemDescription.Text,
                            RequestStatus = "Новая заявка",
                            ClientId = UserStorage.AuthUser.Id
                        };

                        context.Requests.Add(newRequest);
                        context.SaveChanges();
                        MessageBox.Show("Новая заявка успешно создана!",
                            "Заявка успешно создана", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else MessageBox.Show($"При заполнении формы" +
                                 $"произошли следующие проблемы:\n\n{errors}", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private StringBuilder CheckValidData()
        {
            StringBuilder errors = new StringBuilder();

            if (carType.Text.Length == 0) { errors.AppendLine("Введите тип авто!"); }
            if (carModel.Text.Length == 0) { errors.AppendLine("Введите модель авто!"); }

            return errors;
        }
    }
}
