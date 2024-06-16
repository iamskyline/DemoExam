using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RequestsManagement.Models;

namespace RequestsManagement.AppWindows.ManagerWindows
{
    public partial class ManagerRequestsWindow : Window
    {
        public ManagerRequestsWindow()
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
    }
}
