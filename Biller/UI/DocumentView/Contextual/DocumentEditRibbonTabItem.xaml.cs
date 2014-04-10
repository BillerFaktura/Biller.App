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
    /// Interaktionslogik für DocumentEditRibbonTabItem.xaml
    /// </summary>
    public partial class DocumentEditRibbonTabItem : Fluent.RibbonTabItem, Biller.UI.Interface.IRibbonTabItem
    {
        private DocumentEditViewModel _ViewModel;

        public DocumentEditRibbonTabItem(DocumentEditViewModel ViewModel)
        {
            InitializeComponent();
            _ViewModel = ViewModel;
            Group = ViewModel.ContextualTabGroup;
        }

        public UI.Interface.ITabContentViewModel ParentViewModel
        {
            get { return _ViewModel; }
        }

        private async void buttonCloseControl_Click(object sender, RoutedEventArgs e)
        {
            await _ViewModel.ReceiveCloseCommand();
        }

        public void ShowDocumentControls()
        {
            GroupOrderFolder.Visibility = System.Windows.Visibility.Collapsed;
            //GroupOrder1.Visibility = System.Windows.Visibility.Visible;
            GroupOrder2.Visibility = System.Windows.Visibility.Visible;
            GroupOrder3.Visibility = System.Windows.Visibility.Visible;
            GroupOrder4.Visibility = System.Windows.Visibility.Visible;
        }

        private void buttonOrderAddArticle_Click(object sender, RoutedEventArgs e)
        {
            _ViewModel.ParentViewModel.ParentViewModel.ArticleTabViewModel.ReceiveRequestArticleCommand(_ViewModel);
        }

        private async void buttonQuickSaveOrder_Click(object sender, RoutedEventArgs e)
        {
            _ViewModel.DocumentEditRibbonTabItem.Focus(); // MVVM
            await _ViewModel.ParentViewModel.SaveOrUpdateDocument(_ViewModel.Document);
            CheckLinkedDocuments();
            _ViewModel.ParentViewModel.ParentViewModel.SettingsTabViewModel.SaveOrUpdateDocumentFolder(_ViewModel.LinkedDocuments);
        }

        public void AddDocumentButton(Fluent.Button button)
        {
            GroupOrderFolder.Items.Add(button);
        }

        private void buttonPDFOrder_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "PDF Dokument|*.pdf";
            saveFileDialog.FileName = _ViewModel.Document.LocalizedDocumentType + " " + _ViewModel.Document.ID;
            if (saveFileDialog.ShowDialog() == true)
                _ViewModel.ExportClass.SaveDocument(_ViewModel.Document, saveFileDialog.FileName);
        }

        private void buttonPrintOrder_Click(object sender, RoutedEventArgs e)
        {
            _ViewModel.ExportClass.PrintDocument(_ViewModel.Document);
        }

        private void CheckLinkedDocuments()
        {
            dynamic document = new Biller.Data.Document.PreviewDocument(_ViewModel.Document.DocumentType) { DocumentID = _ViewModel.Document.DocumentID };
            document.LocalizedDocumentType = _ViewModel.Document.LocalizedDocumentType;

            if (!_ViewModel.LinkedDocuments.Documents.Contains(document))
                _ViewModel.LinkedDocuments.Documents.Add(document);
        }

        private async void buttonOrderSave_Click(object sender, RoutedEventArgs e)
        {
            _ViewModel.DocumentEditRibbonTabItem.Focus(); // MVVM
            await _ViewModel.ParentViewModel.SaveOrUpdateDocument(_ViewModel.Document);
            CheckLinkedDocuments();
            _ViewModel.ParentViewModel.ParentViewModel.SettingsTabViewModel.SaveOrUpdateDocumentFolder(_ViewModel.LinkedDocuments);
            await _ViewModel.ReceiveCloseCommand();
        }
    }
}
