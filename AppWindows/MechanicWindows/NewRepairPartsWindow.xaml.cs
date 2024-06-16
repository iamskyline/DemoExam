using RequestsManagement.Models;
using System;
using System.Security.AccessControl;
using System.Text;
using System.Windows;

namespace RequestsManagement.AppWindows.MechanicWindows
{
    public partial class NewRepairPartsWindow : Window
    {
        private Requests _currentRequest;
        public NewRepairPartsWindow(Requests request)
        {
            InitializeComponent();
            _currentRequest = request;
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

                        request.RepairParts = repairPartsTextBox.Text;

                        context.SaveChanges();
                        MessageBox.Show("Запчасти к заявке успешно сохранены!",
                            "Успех", MessageBoxButton.OK,
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

            if (repairPartsTextBox.Text.Length == 0) { errors.AppendLine("Вы не указали запчасти для заявки!"); }

            return errors;
        }
    }
}
