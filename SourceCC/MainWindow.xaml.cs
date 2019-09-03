using System.IO;
using System.Linq;
using System.Windows;
using System.Diagnostics;
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

        private void submitButton_Click(object sender, RoutedEventArgs e)
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
                    resultsWindow.Document.Blocks.Clear();
                    var watch = Stopwatch.StartNew();

                    DirectoryInfo di = new DirectoryInfo(dir);
                    var cacheFiles = from f in di.EnumerateFiles()
                                     where f.Name.Contains(".cache")
                                     select f;
                    int filesLength = cacheFiles.Count();

                    if (filesLength > 0)
                    {
                        foreach (var f in cacheFiles)
                        {
                            string file = f.FullName;
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

            resultsWindow.ScrollToEnd();
            directorySelector.IsEnabled = true;
            submitButton.IsEnabled = true;
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.Settings settingsWindow = new Windows.Settings();
            settingsWindow.ShowDialog();
        }
    }
}
