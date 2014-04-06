using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Import_Biller.UI
{
    /// <summary>
    /// Interaktionslogik für TabContent.xaml
    /// </summary>
    public partial class TabContent : UserControl
    {
        public TabContent()
        {
            InitializeComponent();
        }

        private async void Office2013Button_Click(object sender, RoutedEventArgs e)
        {
            var path = pathTextbox.Text;
            var import = new Biller.Data.Import.BillerV1.Import(path, (DataContext as EntryPoint).ParentViewModel.Database);
            progressring.IsActive = true;
            await import.ImportEverything();
            DocumentsToChangeList.ItemsSource = import.DocumentsToModify;
            progressring.IsActive = false;
        }

        private void WatermarkTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var fb = new System.Windows.Forms.FolderBrowserDialog();
            if (fb.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                (sender as TextBox).Text = fb.SelectedPath;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
