﻿using Fluent;
using System.Linq;
using System.Windows.Controls;

namespace Biller.UI.DocumentView
{
    /// <summary>
    /// Interaktionslogik für OrderRibbonTabItem.xaml
    /// </summary>
    public partial class DocumentRibbonTabItem : RibbonTabItem, Biller.UI.Interface.IRibbonTabItem
    {
        public DocumentRibbonTabItem(DocumentView.DocumentTabViewModel ViewModel)
        {
            InitializeComponent();
            _ParentViewModel = ViewModel;
            DataContext = _ParentViewModel;
        }

        private DocumentView.DocumentTabViewModel _ParentViewModel;

        public UI.Interface.ITabContentViewModel ParentViewModel
        {
            get { return _ParentViewModel; }
        }

        private async void buttonNewOrder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await _ParentViewModel.ReceiveNewDocumentCommand(this);
        }

        private async void buttonEditOrder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await _ParentViewModel.ReceiveEditDocumentCommand(this);
        }

        private async void buttonOrderPDF_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var factory = _ParentViewModel.GetFactory(_ParentViewModel.SelectedDocument.DocumentType);
            if (factory != null)
            {
                var loadingDocument = factory.GetNewDocument();
                loadingDocument.DocumentID = _ParentViewModel.SelectedDocument.DocumentID;
                loadingDocument = await _ParentViewModel.ParentViewModel.Database.GetDocument(loadingDocument);

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "PDF Dokument|*.pdf";
                saveFileDialog.FileName = loadingDocument.LocalizedDocumentType + " " + loadingDocument.ID;
                
                if (saveFileDialog.ShowDialog() == true)
                    _ParentViewModel.ParentViewModel.SettingsTabViewModel.GetPreferedExportClass(loadingDocument).SaveDocument(loadingDocument, saveFileDialog.FileName);
            }
        }

        private async void buttonPrintOrder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var factory = _ParentViewModel.GetFactory(_ParentViewModel.SelectedDocument.DocumentType);
            if (factory != null)
            {
                var loadingDocument = factory.GetNewDocument();
                loadingDocument.DocumentID = _ParentViewModel.SelectedDocument.DocumentID;
                loadingDocument = await _ParentViewModel.ParentViewModel.Database.GetDocument(loadingDocument);

                _ParentViewModel.ParentViewModel.SettingsTabViewModel.GetPreferedExportClass(loadingDocument).PrintDocument(loadingDocument);
            }
        }

        private void buttonOrderSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void buttonOrderFastPick_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private async void OrderPDFSelection_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var factory = _ParentViewModel.GetFactory(_ParentViewModel.SelectedDocument.DocumentType);
            if (factory != null)
            {
                var loadingDocument = factory.GetNewDocument();
                loadingDocument.DocumentID = _ParentViewModel.SelectedDocument.DocumentID;
                loadingDocument = await _ParentViewModel.ParentViewModel.Database.GetDocument(loadingDocument);

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "PDF Dokument|*.pdf";
                saveFileDialog.FileName = loadingDocument.LocalizedDocumentType + " " + loadingDocument.ID;
                
                if (saveFileDialog.ShowDialog() == true)
                {
                    var export = ((sender as TextBlock).DataContext as Biller.Core.Interfaces.IExport);
                    export.SaveDocument(loadingDocument, saveFileDialog.FileName);
                }
            }
        }

        private async void OrderPrintSelection_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var factory = _ParentViewModel.GetFactory(_ParentViewModel.SelectedDocument.DocumentType);
            if (factory != null)
            {
                var loadingDocument = factory.GetNewDocument();
                loadingDocument.DocumentID = _ParentViewModel.SelectedDocument.DocumentID;
                loadingDocument = await _ParentViewModel.ParentViewModel.Database.GetDocument(loadingDocument);

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "PDF Dokument|*.pdf";
                saveFileDialog.FileName = loadingDocument.LocalizedDocumentType + " " + loadingDocument.ID;

                if (saveFileDialog.ShowDialog() == true)
                {
                    var export = ((sender as TextBlock).DataContext as Biller.Core.Interfaces.IExport);
                    export.PrintDocument(loadingDocument);
                }
            }
        }
    }
}