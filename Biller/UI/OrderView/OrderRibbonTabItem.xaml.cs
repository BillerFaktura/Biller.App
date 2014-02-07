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

        private void buttonEditOrder_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void buttonOrderPDF_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void buttonPrintOrder_Click(object sender, System.Windows.RoutedEventArgs e)
        {

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