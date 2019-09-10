using System;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

using CaprineNet.INI;

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
            if (!File.Exists(Path.Combine(Classes.Constants.DataPath, "SourceCC.ini")))
            {
                Directory.CreateDirectory(Classes.Constants.DataPath);

                INI cfg = new INI(Classes.Constants.ConfigPath);
                cfg.Write("Tf2", Classes.Constants.TF2DefaultPath, "Folders");
                cfg.Write("L4d2", Classes.Constants.L4D2DefaultPath, "Folders");
            }
        }

        private async void submitButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem ComboItem = (ComboBoxItem)directorySelector.SelectedItem;
            string game = ComboItem.Name;
            string dir;

            directorySelector.IsEnabled = false;
            submitButton.IsEnabled = false;

            resultsWindow.Document.Blocks.Clear();
            resultsWindow.Document.Blocks.Add(new Paragraph(new Run("Processing...")));

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
                if (!Directory.Exists(dir))
                {
                    MessageBoxResult res = MessageBox.Show($"{dir} could not be located. If your {game.ToUpper()} installation is in a different location then please change it in settings.\n\nWould you like to open settings now?", "Folder not found", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);

                    switch (res)
                    {
                        case MessageBoxResult.None:
                        case MessageBoxResult.No:
                            break;
                        case MessageBoxResult.Yes:
                        default:
                            Windows.Settings settingsWindow = new Windows.Settings();
                            settingsWindow.ShowDialog();
                            break;
                    }

                    resultsWindow.Document.Blocks.Clear();
                }
                else
                {
                    var watch = Stopwatch.StartNew();
                    int foundFiles = 0;

                    var deletedFile = new Progress<string>(file =>
                    {
                        string line = $"Deleted {file}";
                        resultsWindow.Document.Blocks.Add(new Paragraph(new Run(line)));
                    });

                    await Task.Run(() =>
                    {
                        DeleteFiles(dir, deletedFile, out foundFiles);
                    });

                    watch.Stop();
                    long elapsedMs = watch.ElapsedMilliseconds;
                    if (foundFiles < 1)
                    {
                        resultsWindow.Document.Blocks.Add(new Paragraph(new Run("Wow! This game folder is already clean!")));
                    }
                    else
                    {
                        resultsWindow.Document.Blocks.Add(new Paragraph(new Run($"Deleted {foundFiles} files(s)")));
                    }

                    resultsWindow.Document.Blocks.Add(new Paragraph(new Run($"Finished process in {elapsedMs}ms.")));
                }
            }

            resultsWindow.ScrollToEnd();
            directorySelector.IsEnabled = true;
            submitButton.IsEnabled = true;
        }

        private void DeleteFiles(string dir, IProgress<string> deletedFile, out int foundFiles)
        {
            int ff = 0;
            foreach (string file in Directory.EnumerateFiles(dir, "*.cache", SearchOption.AllDirectories))
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                    ff++;
                    deletedFile.Report(file);
                }
            }

            foundFiles = ff;
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.Settings settingsWindow = new Windows.Settings();
            settingsWindow.ShowDialog();
        }

        private void authorLink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://steamcommunity.com/id/minorin");
        }
    }
}
