using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Net.Mime;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace PhotoInserter {
    public partial class MainWindow : Window {
        private readonly string defaultLocalizationLabelText;
        private readonly string defaultImageNamesLabelText;
        private string[]? imageFilePaths;

        private string saveDirectoryPath = Environment.CurrentDirectory;

        public MainWindow() {
            InitializeComponent();
            defaultLocalizationLabelText = (string)LocalizationLabel.Content;
            defaultImageNamesLabelText = (string)ImageNamesLabel.Content;

            LocalizationLabel.Content += saveDirectoryPath;
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e) {
            CommonOpenFileDialog openFileDialog = new() {
                IsFolderPicker = true,
            };

            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok) {
                saveDirectoryPath = openFileDialog.FileName;
                LocalizationLabel.Content = defaultLocalizationLabelText + openFileDialog.FileName;
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new() {
                Multiselect = true,
                Filter = "Zdjęcia (*.png; *.jpg)| *.png; *.jpg",
                FilterIndex = 1,
            };

            if (openFileDialog.ShowDialog() == true) {
                imageFilePaths = openFileDialog.FileNames;
                string imageFileText = defaultImageNamesLabelText;
                foreach (var imageName in imageFilePaths) {
                    imageFileText += imageName + "; ";
                }

                ImageNamesLabel.Content = imageFileText;
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e) {
            if(imageFilePaths == null) {
                InformationLabel.Content = "Nie podano zdjęć do utworzenia.";
                return;
            }

            bool parsedWidth = int.TryParse(WidthTextBox.Text, out int imageWidth);
            bool parsedHeight = int.TryParse(HeightTextBox.Text, out int imageHeight);
            if(!parsedWidth || !parsedHeight) {
                InformationLabel.Content = "Podano nieprawidłowe dane.";
                return;
            }

            string savePath = saveDirectoryPath + $"\\{SaveNameTextBox.Text}.png";

            DisplayImage.Source = null;

            if (File.Exists(savePath)) {
                InformationLabel.Content = "Plik o podanej nazwie już istnieje. Zmień nazwę pliku aby kontynuować";
                return;
            }

            ImageHelper.CreateImage(imageWidth, imageHeight, imageFilePaths, savePath);

            InformationLabel.Content = "Podgląd utworzonego zdjęcia";

            imageFilePaths = null;
            ImageNamesLabel.Content = defaultImageNamesLabelText;
            DisplayImage.Source = new BitmapImage(new Uri(savePath));
        }

        private void DefaultValueButton_Click(object sender, RoutedEventArgs e) {
            WidthTextBox.Text = 2376.ToString();
            HeightTextBox.Text = 1680.ToString();
        }
    }
}
