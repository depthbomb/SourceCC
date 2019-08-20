﻿using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SourceCC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void authorLink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://steamcommunity.com/id/minorin");
        }

        private async void submitButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem ComboItem = (ComboBoxItem)directorySelector.SelectedItem;
            string game = ComboItem.Name;
            string dir;

            directorySelector.IsEnabled = false;
            submitButton.IsEnabled = false;

            resultsWindow.Document.Blocks.Clear();
            resultsWindow.Document.Blocks.Add(new Paragraph(new Run($"Processing...")));

            switch (game)
            {
                case "tf2":
                default:
                    dir = Properties.Settings.Default.TF2Folder;
                    break;
                case "l4d2":
                    dir = Properties.Settings.Default.L4D2Folder;
                    break;
            }

            if (dir != null)
            {
                resultsWindow.Document.Blocks.Clear();

                if (!Directory.Exists(dir))
                {
                    MessageBox.Show($"{dir} could not be located.\n\nIf your {game.ToUpper()} installation is in a different location then please change it in settings.", "Folder not found", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
                else
                {
                    await DeleteFiles(dir);
                }
            }

            directorySelector.IsEnabled = true;
            submitButton.IsEnabled = true;
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.Settings settingsWindow = new Windows.Settings();
            settingsWindow.ShowDialog();
        }

        private async Task DeleteFiles(string dir)
        {
            string[] files = await Task.Run(() => Directory.GetFiles(dir, "*.cache", SearchOption.AllDirectories));
            int filesLength = files.Length;
            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (filesLength > 0)
            {
                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                        string line = $"Deleted {Path.GetFileName(file)}";
                        resultsWindow.Document.Blocks.Add(new Paragraph(new Run(line)));
                    }
                }
            }
            else
            {
                resultsWindow.Document.Blocks.Add(new Paragraph(new Run("Wow! This game folder is already clean!")));
            }

            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;
            resultsWindow.Document.Blocks.Add(new Paragraph(new Run($"Finished operation in {elapsedMs}ms.")));
        }
    }
}