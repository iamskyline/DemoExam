using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using iTextSharp.text.pdf;
using iTextSharp.text;
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

        private void PdfFormingButton_OnClick(Object sender, RoutedEventArgs e)
        {
            Document document = new Document();
            String path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Отчет_ПДФ.pdf");
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));

            document.Open();

            BaseFont baseFont = BaseFont.CreateFont("C:/Windows/Fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new Font(baseFont, 14, Font.NORMAL);

            List<Requests> requests = RequestsManagementEntities.GetContext().Requests.ToList();

            PdfPTable table = new PdfPTable(6);
            table.DefaultCell.BorderWidth = 1;
            table.AddCell(new Phrase("Номер заявки", font));
            table.AddCell(new Phrase("Тип авто", font));
            table.AddCell(new Phrase("Модель авто", font));
            table.AddCell(new Phrase("Описание проблемы", font));
            table.AddCell(new Phrase("Статус", font));
            table.AddCell(new Phrase("Использованные запчасти", font));
            
            foreach (Requests item in requests)
            {
                table.AddCell(new Phrase(item.Id.ToString(), font));
                table.AddCell(new Phrase(item.CarType, font));
                table.AddCell(new Phrase(item.CarModel, font));
                table.AddCell(new Phrase(item.ProblemDescription, font));
                table.AddCell(new Phrase(item.RequestStatus, font));
                table.AddCell(new Phrase(item.RepairParts, font));
            }

            SetCellPadding(table, 10);

            document.Add(table);
            document.Close();
            MessageBox.Show("PDF файл успешно сохранен на рабочий стол!");
        }

        private void SetCellPadding(PdfPTable table, int padding)
        {
            for (int row = 0; row < table.Rows.Count; row++)
            {
                PdfPRow tableRow = table.GetRow(row);

                for (int col = 0; col < tableRow.GetCells().Length; col++)
                {
                    PdfPCell cell = tableRow.GetCells()[col];
                    cell.Padding = padding;
                }
            }
        }
    }
}
