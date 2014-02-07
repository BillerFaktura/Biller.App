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

namespace Biller.UI.SettingsView.SettingsList.PaymentSettings
{
    /// <summary>
    /// Interaktionslogik für PaymentTabContent.xaml
    /// </summary>
    public partial class PaymentTabContent : UserControl
    {
        public PaymentTabContent()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Reset fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Office2013Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsTabViewModel).SelectedPaymentMethode = new Data.Utils.PaymentMethode();
        }

        /// <summary>
        /// Save changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Office2013Button_Click_1(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsTabViewModel).TabContent.Focus(); //For MVVM //
            (DataContext as SettingsTabViewModel).SaveOrUpdatePaymentMethode((DataContext as SettingsTabViewModel).SelectedPaymentMethode);
        }

        /// <summary>
        /// Selected Item changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListView).SelectedItem != null)
            {
                var selected = (sender as ListView).SelectedItem as Data.Utils.PaymentMethode;
                var temp = new Data.Utils.PaymentMethode() { Name = selected.Name, Text = selected.Text, Discount = selected.Discount};
                (DataContext as SettingsTabViewModel).SelectedPaymentMethode = temp;
            }
        }
    }
}
