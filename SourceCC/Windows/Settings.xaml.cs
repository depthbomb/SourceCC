using System.Windows;
using System.Windows.Controls;

using i18n = SourceCC.Classes.I18n;

namespace SourceCC.Windows
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();

            this.Title = i18n.__("settings");

            var settings = Properties.Settings.Default;
            string tf2Folder = settings.TF2Folder;
            string l4d2Folder = settings.L4D2Folder;

            if (!settings.SeenOsArchMessage)
            {
                MessageBox.Show($"I have detected that you are using a {(Classes.Constants.Is64BitOs ? "64bit" : "32bit")} operating system and the default directories have been set accordingly. If this is incorrect then you should change them.\n\nThis message will only be displayed once.", "Default folders set", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                settings.SeenOsArchMessage = true;
                settings.Save();
            }

            string changeButtonText = i18n.__("change_folder");

            this.tf2Folder.Text = tf2Folder;
            this.l4d2Folder.Text = l4d2Folder;

            this.changeTf2Folder.Content = changeButtonText;
            this.changeL4d2Folder.Content = changeButtonText;

            //  Does this really need to be this complicated?
            foreach (ComboBoxItem item in this.languageSelector.Items)
            {
                if (item.Name == Classes.Constants.Language) {
                    this.languageSelector.SelectedItem = item;
                    break;
                }
            }
        }

        private void LanguageSelector_DropDownClosed(object sender, System.EventArgs e)
        {
            ComboBoxItem ComboItem = (ComboBoxItem)languageSelector.SelectedItem;
            string lang = ComboItem.Name;
            Properties.Settings.Default.Language = lang;
            Properties.Settings.Default.Save();

            if (!Properties.Settings.Default.SeenLanguageRestartMessage)
            {
                MessageBoxResult res = MessageBox.Show("The program will now be restarted so the language change can take effect.", "Restart required", MessageBoxButton.OK, MessageBoxImage.Warning);

                Properties.Settings.Default.SeenLanguageRestartMessage = true;
                Properties.Settings.Default.Save();
            }

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void changeTf2_Click(object sender, RoutedEventArgs e)
        {
            string newFolder = ChooseFolder();
            if (newFolder != null)
            {
                Properties.Settings.Default.TF2Folder = newFolder;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();

                this.tf2Folder.Text = newFolder;
            }
        }

        private void changeL4d2_Click(object sender, RoutedEventArgs e)
        {
            string newFolder = ChooseFolder();
            if (newFolder != null)
            {
                Properties.Settings.Default.L4D2Folder = newFolder;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();

                this.l4d2Folder.Text = newFolder;
            }
        }

        private string ChooseFolder()
        {
            var folderSelector = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = folderSelector.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                return folderSelector.SelectedPath;
            }
            else
            {
                return null;
            }
        }
    }
}
