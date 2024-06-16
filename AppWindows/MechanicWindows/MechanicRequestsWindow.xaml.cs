using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RequestsManagement.GlobalStorage;
using RequestsManagement.Models;

namespace RequestsManagement.AppWindows.MechanicWindows
{
    public partial class MechanicRequestsWindow : Window
    {
        public MechanicRequestsWindow()
        {
            InitializeComponent();
            LoadAllApplications();
        }

        private void LoadAllApplications()
        {
            try
            {
                itemsControl.ItemsSource = RequestsManagementEntities.GetContext()
                    .Requests.Where(r => r.MasterId == UserStorage.AuthUser.Id).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить список с " +
                                $"заявками! {ex.Message}");
            }
        }

        private void AddCommentButton_OnClick(Object sender, RoutedEventArgs e)
        {
            CommentWindow window = new CommentWindow((sender as Button)
                .DataContext as Requests);
            window.ShowDialog();
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

        private void AddRepairParts_OnClick(Object sender, RoutedEventArgs e)
        {
            NewRepairPartsWindow window = new NewRepairPartsWindow((sender as Button)
                .DataContext as Requests);
            window.ShowDialog();
        }
    }
}
