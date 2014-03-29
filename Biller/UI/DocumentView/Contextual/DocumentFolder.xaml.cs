using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Biller.UI.DocumentView.Contextual
{
    /// <summary>
    /// Interaktionslogik für OrderFolder.xaml
    /// </summary>
    public partial class DocumentFolder : UserControl
    {
        public DocumentFolder(DocumentEditViewModel parentViewModel)
        {
            InitializeComponent();
            ParentViewModel = parentViewModel;
        }

        DocumentEditViewModel ParentViewModel;

        /// <summary>
        /// This methode gets called when the user clicks on one of the templated controls. It opens the selected document in a new tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void itemtemplate_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var doc = (sender as StackPanel).DataContext as Biller.Data.Document.PreviewDocument;
            await ParentViewModel.ParentViewModel.ReceiveEditOrderCommand(ParentViewModel, doc);
        }

        /// <summary>
        /// Opens the document overview to select a document that is related to the new one
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Office2013Button_Click(object sender, RoutedEventArgs e)
        {
            ParentViewModel.ParentViewModel.ReceiveRequestDocumentCommand(ParentViewModel);
        }
    }
}
