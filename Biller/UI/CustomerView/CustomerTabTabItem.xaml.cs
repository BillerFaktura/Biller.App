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

namespace Biller.UI.CustomerView
{
    /// <summary>
    /// Interaktionslogik für CustomerTabTabItem.xaml
    /// </summary>
    public partial class CustomerTabTabItem : Fluent.RibbonTabItem, Biller.UI.Interface.IRibbonTabItem
    {
        public CustomerTabTabItem(CustomerTabViewModel parentViewModel)
        {
            InitializeComponent();
            ParentViewModel = parentViewModel;
            _parentViewModel=parentViewModel;
        }

        private CustomerTabViewModel _parentViewModel { get; set; }

        public UI.Interface.ITabContentViewModel ParentViewModel { get; private set; }

        private async void buttonNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            await _parentViewModel.ReceiveNewCustomerCommand();
        }

        private async void buttonnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            await _parentViewModel.ReceiveEditCustomerCommand(this);
        }
    }
}
