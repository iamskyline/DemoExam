using RequestsManagement.Models;
using System;
using System.Linq;
using System.Text;
using System.Windows;

namespace RequestsManagement.AppWindows.AdminWindows
{
    public partial class EditingRequestAdminWindow : Window
    {
        private Requests _currentRequest;
        public EditingRequestAdminWindow(Requests currentRequest)
        {
            InitializeComponent();
            _currentRequest = currentRequest;
            DataContext = _currentRequest;
            LoadDataToComboBoxes();
        }

        private void LoadDataToComboBoxes()
        {
            Users client = RequestsManagementEntities.GetContext().Users.FirstOrDefault(u => u.Id == _currentRequest.ClientId);
            clientComboBox.ItemsSource = RequestsManagementEntities.GetContext()
                .Users
                .Where(u => u.UserRoleId == (Int32)UserRole.Client)
                .ToList();
            clientComboBox.SelectedItem = client;

            Users master = RequestsManagementEntities.GetContext().Users.FirstOrDefault(u => u.Id == _currentRequest.MasterId);
            masterComboBox.ItemsSource = RequestsManagementEntities.GetContext()
                .Users
                .Where(u => u.UserRoleId == (Int32)UserRole.Mechanic)
                .ToList();
            masterComboBox.SelectedItem = master;
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
                        Requests request = context.Requests.Find(_currentRequest.Id);

                        Users master = masterComboBox.SelectedItem as Users;
                        Users client = clientComboBox.SelectedItem as Users;

                        request.StartDate = startDate.SelectedDate ?? DateTime.Now;
                        request.CarType = carType.Text;
                        request.CarModel = carModel.Text;
                        request.ProblemDescription = problemDescription.Text;
                        request.RequestStatus = requestStatus.Text;
                        request.CompletionDate = endDate.SelectedDate;
                        request.RepairParts = repairParts.Text;
                        request.MasterId = master.Id;
                        request.ClientId = client.Id;

                        context.SaveChanges();
                        MessageBox.Show("Заявка успешно отредактирована!",
                            "Заявка успешно сохранена", MessageBoxButton.OK,
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
                                 $"произошли следующие проблемы:\n\n{errors}", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
