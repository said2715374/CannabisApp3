using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace CannabisApp
{
    public partial class ImportPage : Page
    {
        public ImportPage()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BrowseExcelFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xls;*.xlsx"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedFilePathTextBlock.Text = openFileDialog.FileName;
            }
        }

        private void ImportData_Click(object sender, RoutedEventArgs e)
        {
            string filePath = SelectedFilePathTextBlock.Text;

            if (filePath == "Aucun fichier sélectionné")
            {
                MessageBox.Show("Veuillez sélectionner un fichier à importer.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Code to import data from the selected Excel file
            MessageBox.Show($"Fichier {filePath} importé avec succès.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Border_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Border_DragLeave(object sender, DragEventArgs e)
        {
            // Optional: Handle visual feedback when the drag leaves the border
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    SelectedFilePathTextBlock.Text = files[0];
                }
            }
        }
    }
}
