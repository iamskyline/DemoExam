using RequestsManagement.Models;
using System;
using System.Linq;
using System.Text;
using System.Windows;

namespace RequestsManagement.AppWindows.AdminWindows
{
    public partial class NewRequestAdminWindow : Window
    {
        public NewRequestAdminWindow()
        {
            InitializeComponent();
            LoadDataToComboBoxes();
        }

        private void LoadDataToComboBoxes()
        {
            clientComboBox.ItemsSource = RequestsManagementEntities.GetContext()
                .Users
                .Where(u => u.UserRoleId == (Int32)UserRole.Client)
                .ToList();
            clientComboBox.SelectedIndex = 0;

            masterComboBox.ItemsSource = RequestsManagementEntities.GetContext()
                .Users
                .Where(u => u.UserRoleId == (Int32)UserRole.Mechanic)
                .ToList();
            masterComboBox.SelectedIndex = 0;
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
                        Users master = masterComboBox.SelectedItem as Users;
                        Users client = clientComboBox.SelectedItem as Users;

                        Requests newRequest = new Requests
                        {
                            StartDate = startDate.SelectedDate ?? DateTime.Now,
                            CarType = carType.Text,
                            CarModel = carModel.Text,
                            ProblemDescription = problemDescription.Text,
                            RequestStatus = requestStatus.Text,
                            CompletionDate = endDate.SelectedDate,
                            RepairParts = repairParts.Text,
                            MasterId = master.Id,
                            ClientId = client.Id
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
            else MessageBox.Show($"При заполнении формы " +
                                 $"произошли следующие проблемы:\n\n{errors}", "Внимание!",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private StringBuilder CheckValidData()
        {
            StringBuilder errors = new StringBuilder();

            if (clientComboBox.SelectedItem == null) { errors.AppendLine("Выберите клиента!"); }

            if (carType.Text.Length == 0) { errors.AppendLine("Введите тип авто!"); }
            if (carModel.Text.Length == 0) { errors.AppendLine("Введите модель авто!"); }
            if (requestStatus.Text.Length == 0) { errors.AppendLine("Введите статус заявки!"); }

            return errors;
        }

        private void CreateClient_OnClick(Object sender, RoutedEventArgs e)
        {
            CreatingNewClientWindow window = new CreatingNewClientWindow();
            window.ShowDialog();
            LoadDataToComboBoxes();
        }
    }
}
