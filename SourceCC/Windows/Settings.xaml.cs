using System.Windows;

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

            var settings = Properties.Settings.Default;
            string tf2Folder = settings.TF2Folder;
            string l4d2Folder = settings.L4D2Folder;

            this.tf2Folder.Text = tf2Folder;
            this.l4d2Folder.Text = l4d2Folder;
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
