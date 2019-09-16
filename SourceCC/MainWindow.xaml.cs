using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.Generic;

using CaprineNet.INI;

namespace SourceCC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        INI Config;
        List<string> targetFiles = new List<string>() { ".cache" };

        public MainWindow()
        {
            InitializeComponent();
            LoadConfig();
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
            //  Before we start deleting files, let's read the config and update the list of files to look for from the config.
            if (Config.KeyExists("ExtraFiles", "Files"))
            {
                string[] extraFiles = Config.Read("ExtraFiles", "Files").Split(',');
                foreach (string file in extraFiles)
                    targetFiles.Add(file);
            }

            //  Time to start looking through files!
            int ff = 0;
            foreach (string file in Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories)
                .ToArray().Where(s => targetFiles.Any(ext => ext == Path.GetExtension(s))))
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

        private void LoadConfig()
        {
            string configPath = Classes.Constants.ConfigPath;
            if (!File.Exists(configPath))
            {
                Directory.CreateDirectory(Classes.Constants.DataPath);

                INI cfg = new INI(configPath);
                cfg.Write("Tf2", Classes.Constants.TF2DefaultPath, "Folders");
                cfg.Write("L4d2", Classes.Constants.L4D2DefaultPath, "Folders");
                cfg.Write("ExtraFiles", ".ztmp", "Files");
                Config = cfg;
            }
            else
            {
                Config = new INI(configPath);

                //  Handle old settings key
                if (Config.KeyExists("DeleteZtmp", "Files"))
                {
                    if (bool.Parse(Config.Read("DeleteZtmp", "Files")))
                        Config.Write("ExtraFiles", ".ztmp", "Files");

                    Config.DeleteKey("DeleteZtmp", "Files");
                    Config.Write("ExtraFiles", ".ztmp", "Files");
                }
            }
        }
    }
}
