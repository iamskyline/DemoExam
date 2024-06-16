using System;
using System.Security.AccessControl;
using System.Text;
using System.Windows;
using RequestsManagement.GlobalStorage;
using RequestsManagement.Models;

namespace RequestsManagement.AppWindows.MechanicWindows
{
    public partial class CommentWindow : Window
    {
        private Requests _currentRequest;
        public CommentWindow(Requests request)
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
                        Comments newComment = new Comments
                        {
                            MessageText = commentTextBox.Text,
                            RequestId = _currentRequest.Id,
                            MasterId = UserStorage.AuthUser.Id
                        };

                        context.Comments.Add(newComment);
                        context.SaveChanges();
                        MessageBox.Show("Комментарий успешно добавлен!",
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
            else MessageBox.Show($"При заполнении формы" +
                                 $"произошли следующие проблемы:\n\n{errors}", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private StringBuilder CheckValidData()
        {
            StringBuilder errors = new StringBuilder();

            if (commentTextBox.Text.Length == 0) { errors.AppendLine("Вы не указали комментарий к заявке!"); }

            return errors;
        }
    }
}
