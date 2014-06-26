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

namespace Biller.UI.SettingsView.SettingsList.ShipmentSettings
{
    /// <summary>
    /// Interaktionslogik für ShipmentTabContent.xaml
    /// </summary>
    public partial class ShipmentTabContent : UserControl
    {
        public ShipmentTabContent()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListView).SelectedItem != null)
            {
                var selected = (sender as ListView).SelectedItem as Core.Utils.Shipment;
                var temp = new Core.Utils.Shipment() { Name = selected.Name, DefaultPrice = selected.DefaultPrice };
                (DataContext as SettingsTabViewModel).SelectedShipment = temp;
            }
        }

        private void Office2013Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsTabViewModel).SelectedShipment = new Core.Utils.Shipment();
        }

        /// <summary>
        /// Save changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Office2013Button_Click_1(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsTabViewModel).TabContent.Focus(); //For MVVM //
            (DataContext as SettingsTabViewModel).SaveOrUpdateShipment((DataContext as SettingsTabViewModel).SelectedShipment);
        }
    }
}
