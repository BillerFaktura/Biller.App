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

namespace Biller.UI.OrderView.Contextual.EditTabs.Receipent
{
    /// <summary>
    /// Interaktionslogik für Content.xaml
    /// </summary>
    public partial class Content : UserControl
    {
        public Content()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Request customer view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Office2013Button_Click(object sender, RoutedEventArgs e)
        {
            var viewmodel = (DataContext as OrderEditViewModel);
            viewmodel.ParentViewModel.ParentViewModel.CustomerTabViewModel.ReceiveRequestCustomerCommand(viewmodel);
        }

        private async void WatermarkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewmodel = (DataContext as OrderEditViewModel);
            var exists = await viewmodel.ParentViewModel.ParentViewModel.Database.CustomerExists((sender as TextBox).Text);
            if (exists == true)
            {
                viewmodel.PreviewCustomer = await viewmodel.ParentViewModel.ParentViewModel.Database.GetCustomer((sender as TextBox).Text);
            }
            else
            {
                viewmodel.PreviewCustomer = null;
            }
        }

        private void WatermarkTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var viewmodel = (DataContext as OrderEditViewModel);
            if (e.Key == Key.Enter && viewmodel.PreviewCustomer != null)
            {
                (viewmodel.Document as Data.Orders.Order).Customer = viewmodel.PreviewCustomer;
                viewmodel.PreviewCustomer = null;
                (sender as TextBox).Text = "";
            }
        }
    }
}
