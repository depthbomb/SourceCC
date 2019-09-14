using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.ServiceProcess;
using System.Windows.Controls;

using CaprineNet.INI;
using CaprineNet.UnixTimestamp;

namespace SourceCC.Service.Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SourceCC");
        private static string serviceExecutable = Path.Combine(@".\", "SourceCC.Service.exe");
        private static bool serviceExists = ServiceController.GetServices().Any(s => s.ServiceName == "SourceCCService");

        private static string configFile = Path.Combine(dataPath, "ServiceConfig.ini");
        private static string logFolder = Path.Combine(dataPath, "Service Logs");

        private dynamic Config;

        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists(Path.Combine(dataPath, "SourceCC.ini")))
            {
                MessageBox.Show("Your SourceCC paths config file is missing. This usually happens when you delete the file or don't run the main program first.\n\nPlease run SourceCC at least once before using the service manager.", "Config not generated", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                Application.Current.Shutdown();
            }
            if (!File.Exists(serviceExecutable))
            {
                MessageBoxResult res = MessageBox.Show("The SourceCC service executable could not be located. Reinstalling SourceCC may fix this problem.\n\nWould you like to visit the downloads page?", "Missing Executable", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes);
                switch (res)
                {
                    case MessageBoxResult.Yes:
                        Process.Start("https://github.com/depthbomb/SourceCC/releases/latest");
                        break;
                    case MessageBoxResult.No:
                    default:
                        Application.Current.Shutdown();
                        break;
                }
            }
            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }
            LoadConfig();
            InitializeUI();
        }

        
        /// <summary>
        /// Processes the selected combo box item when the dropdown is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkInterval_DropDownClosed(object sender, System.EventArgs e)
        {
            ComboBoxItem ComboItem = (ComboBoxItem)checkIntervalComboBox.SelectedItem;
            string interval = ComboItem.Name;

            if (Config.Read("CheckInterval", "Checking") != interval)
            {
                Config.Write("CheckInterval", interval, "Checking");
                Config.Write("NextRun", new UnixTimestamp().Timestamp.ToString(), "Operation");
            }
        }

        /// <summary>
        /// Sets up the big install/uninstall button
        /// </summary>
        private void InitializeUI()
        {
            this.installationButton.Content = serviceExists ? "Uninstall Service" : "Install Service";
            this.installationButton.Click += (object sender, RoutedEventArgs e) =>
            {
                this.installationButton.IsEnabled = false;

                string cmdArgs;
                Process cmd = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Verb = "runas";
                if (serviceExists)
                {
                    cmdArgs = "/C SC STOP \"SourceCCService\" & SC DELETE \"SourceCCService\"";
                }
                else
                {
                    cmdArgs = $"/C SC CREATE \"SourceCCService\" binpath= \"{serviceExecutable}\" start= auto DisplayName= \"SourceCC Service\" & SC START SourceCCService";
                }
                startInfo.Arguments = cmdArgs;
                cmd.StartInfo = startInfo;
                cmd.Start();

                serviceExists = !serviceExists;

                this.installationButton.IsEnabled = true;
                this.installationButton.Content = serviceExists ? "Uninstall Service" : "Install Service";
                this.checkIntervalComboBox.IsEnabled = serviceExists;
            };

            this.checkIntervalComboBox.IsEnabled = serviceExists;
            foreach (ComboBoxItem item in this.checkIntervalComboBox.Items)
            {
                if (item.Name == Config.Read("CheckInterval", "Checking"))
                {
                    this.checkIntervalComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Load the configuration file or create it (and then load) if it doesn't exist
        /// </summary>
        private void LoadConfig()
        {
            if (File.Exists(configFile))
            {
                Config = new INI(configFile);
            }
            else
            {
                int now = new UnixTimestamp().Timestamp;

                var ini = new INI(configFile);
                ini.Write("CheckInterval", "weekly", "Checking");
                ini.Write("NextRun", now.ToString(), "Operation");
                Config = ini;
            }
        }
    }
}
