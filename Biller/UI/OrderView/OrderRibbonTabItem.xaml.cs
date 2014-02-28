using Fluent;

namespace Biller.UI.OrderView
{
    /// <summary>
    /// Interaktionslogik für OrderRibbonTabItem.xaml
    /// </summary>
    public partial class OrderRibbonTabItem : RibbonTabItem, Biller.UI.Interface.IRibbonTabItem
    {
        public OrderRibbonTabItem(OrderView.OrderTabViewModel ViewModel)
        {
            InitializeComponent();
            _ParentViewModel = ViewModel;
            DataContext = _ParentViewModel;
        }

        private OrderView.OrderTabViewModel _ParentViewModel;

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
            await _ParentViewModel.ReceiveEditOrderCommand(this);
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
                    factory.GetNewExportClass().SaveDocument(loadingDocument, saveFileDialog.FileName);
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

                factory.GetNewExportClass().PrintDocument(loadingDocument);
            }
        }

        private void cb_month_filter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void cb_year_filter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void buttonOrderSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void buttonOrderFastPick_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}