using System.IO;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Documents;

using i18n = SourceCC.Classes.I18n;
using System.Threading.Tasks;

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
            this.settingsButtonText.Text = i18n.__("settings");
            this.submitButton.Content = i18n.__("begin_process");
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem ComboItem = (ComboBoxItem)directorySelector.SelectedItem;
            string game = ComboItem.Name;
            string dir;
            string exe; //  Currently not used other than to be re-assigned later. May be used as a sort of way of "validating" chosen folders by looking for the game executable.

            directorySelector.IsEnabled = false;
            submitButton.IsEnabled = false;

            resultsWindow.Document.Blocks.Clear();
            resultsWindow.Document.Blocks.Add(new Paragraph(new Run(i18n.__("process_started"))));

            switch (game)
            {
                case "tf2":
                default:
                    dir = Properties.Settings.Default.TF2Folder;
                    exe = "hl2.exe";
                    break;
                case "l4d2":
                    dir = Properties.Settings.Default.L4D2Folder;
                    exe = "left4dead2.exe";
                    break;
            }

            if (dir != null)
            {
                if (!Directory.Exists(dir))
                {
                    MessageBoxResult res = MessageBox.Show(i18n.__("folder_not_found_text", dir, game.ToUpper()), i18n.__("folder_not_found_caption"), MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);

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
                    int foundFiles = 0;

                    foreach (string file in Directory.EnumerateFiles(dir, "*.cache", SearchOption.AllDirectories))
                    {
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                            string line = i18n.__("process_deleted_file", file);
                            resultsWindow.Document.Blocks.Add(new Paragraph(new Run(line)));
                            foundFiles++;
                        }
                    }

                    watch.Stop();
                    long elapsedMs = watch.ElapsedMilliseconds;
                    if (foundFiles == 0)
                    {
                        resultsWindow.Document.Blocks.Add(new Paragraph(new Run(i18n.__("process_already_clean"))));
                    }
                    resultsWindow.Document.Blocks.Add(new Paragraph(new Run(i18n.__("process_completed", elapsedMs.ToString()))));
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

        private void authorLink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://steamcommunity.com/id/minorin");
        }
    }
}
