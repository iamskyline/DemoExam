using RequestsManagement.Models;
using System;
using System.Linq;
using System.Windows;
using RequestsManagement.GlobalStorage;
using System.Windows.Controls;

namespace RequestsManagement.AppWindows.ClientWindows
{
    public partial class ClientRequestsWindow : Window
    {
        public ClientRequestsWindow()
        {
            InitializeComponent();
            LoadAllApplications();
        }

        private void LoadAllApplications()
        {
            try
            {
               itemsControl.ItemsSource = RequestsManagementEntities.GetContext()
                   .Requests.Where(r => r.ClientId == UserStorage.AuthUser.Id).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить список с " +
                                $"заявками! {ex.Message}");
            }
        }

        private void EditApplicationBtn_OnClick(Object sender, RoutedEventArgs e)
        {
            EditingRequestClientWindow window = new EditingRequestClientWindow((sender as Button)
                .DataContext as Requests);
            window.ShowDialog();
        }

        private void CreateButton_OnClick(Object sender, RoutedEventArgs e)
        {
            NewRequestClientWindow window = new NewRequestClientWindow();
            window.ShowDialog();
        }
    }
}
