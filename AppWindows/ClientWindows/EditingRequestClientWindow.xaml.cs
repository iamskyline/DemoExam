using System;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows;
using RequestsManagement.Models;

namespace RequestsManagement.AppWindows.ClientWindows
{
    public partial class EditingRequestClientWindow : Window
    {
        private Requests _currentRequest;
        public EditingRequestClientWindow(Requests request)
        {
            InitializeComponent();
            _currentRequest = request;
            DataContext = _currentRequest;
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
                        
                        request.CarType = carType.Text;
                        request.CarModel = carModel.Text;
                        request.ProblemDescription = problemDescription.Text;
                        
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

            if (carType.Text.Length == 0) { errors.AppendLine("Введите тип авто!"); }
            if (carModel.Text.Length == 0) { errors.AppendLine("Введите модель авто!"); }

            return errors;
        }
    }
}
