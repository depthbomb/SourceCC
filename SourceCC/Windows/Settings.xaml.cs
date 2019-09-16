using System.IO;
using System.Windows;

using CaprineNet.INI;

namespace SourceCC.Windows
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private INI Config = new INI(Classes.Constants.ConfigPath);

        public Settings()
        {
            InitializeComponent();

            var settings = Properties.Settings.Default;

            string tf2Folder = Config.Read("Tf2", "Folders");
            string l4d2Folder = Config.Read("L4d2", "Folders");

            if (!settings.SeenOsArchMessage)
            {
                MessageBox.Show($"I have detected that you are using a {(Classes.Constants.Is64BitOs ? "64bit" : "32bit")} operating system and the default directories have been set accordingly. If this is incorrect then you should change them.\n\nThis message will only be displayed once.", "Default folders set", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                settings.SeenOsArchMessage = true;
                settings.Save();
            }

            this.tf2Folder.Text = tf2Folder;
            this.l4d2Folder.Text = l4d2Folder;
            this.extraFilesInput.Text = Config.Read("ExtraFiles", "Files");
        }

        private void changeTf2_Click(object sender, RoutedEventArgs e)
        {
            string newFolder = ChooseFolder();
            if (newFolder != null)
            {
                Config.Write("tf2", newFolder, "Folders");
                this.tf2Folder.Text = newFolder;
            }
        }

        private void changeL4d2_Click(object sender, RoutedEventArgs e)
        {
            string newFolder = ChooseFolder();
            if (newFolder != null)
            {
                Config.Write("l4d2", newFolder, "Folders");
                this.l4d2Folder.Text = newFolder;
            }
        }

        private void SaveExtraFiles_Click(object sender, RoutedEventArgs e)
        {
            string extraFiles = this.extraFilesInput.Text;
            //  Clean up the input
            extraFiles = extraFiles
                .Replace(" ", "")
                .Replace("*", "")
                .Trim(',', ' ');
            Config.Write("ExtraFiles", extraFiles, "Files");

            this.extraFilesInput.Text = extraFiles;
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
