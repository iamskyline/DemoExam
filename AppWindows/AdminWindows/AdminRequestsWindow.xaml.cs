using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RequestsManagement.Models;

namespace RequestsManagement.AppWindows.AdminWindows
{
    public partial class AdminRequestsWindow : Window
    {
        public AdminRequestsWindow()
        {
            InitializeComponent();
            LoadAllApplications();
        }

        private void LoadAllApplications()
        {
            try
            {
                itemsControl.ItemsSource = RequestsManagementEntities.GetContext().Requests.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить список с " +
                                $"заявками! {ex.Message}");
            }
        }

        private void SearchTextBox_OnTextChanged(Object sender, TextChangedEventArgs e)
        {
            List<Requests> applications = RequestsManagementEntities
                .GetContext().Requests.ToList();

            if (searchTextBox.Text.Length > 0)
            {
                applications = applications
                    .Where(a => a.Id.ToString()
                        .ToLower()
                        .Contains(searchTextBox.Text.ToLower()) || a.CarModel.ToLower()
                        .Contains(searchTextBox.Text.ToLower())
                    ).ToList();
            }
            itemsControl.ItemsSource = applications;
        }

        private void EditApplicationBtn_OnClick(Object sender, RoutedEventArgs e)
        {
            EditingRequestAdminWindow window = new EditingRequestAdminWindow((sender as Button)
                .DataContext as Requests);
            window.ShowDialog();
        }

        private void CreateRequest_OnClick(Object sender, RoutedEventArgs e)
        {
            NewRequestAdminWindow window = new NewRequestAdminWindow();
            window.ShowDialog();
        }
    }
}
