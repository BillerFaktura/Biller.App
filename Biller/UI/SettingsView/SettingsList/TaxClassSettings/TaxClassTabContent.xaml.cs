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

namespace Biller.UI.SettingsView.SettingsList.TaxClassSettings
{
    /// <summary>
    /// Interaktionslogik für TaxClassTabContent.xaml
    /// </summary>
    public partial class TaxClassTabContent : UserControl
    {
        public TaxClassTabContent()
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
            (DataContext as SettingsTabViewModel).SelectedTaxClass = new Core.Utils.TaxClass();
        }

        /// <summary>
        /// Save changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Office2013Button_Click_1(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsTabViewModel).TabContent.Focus(); //For MVVM //
            (DataContext as SettingsTabViewModel).SaveOrUpdateTaxClass((DataContext as SettingsTabViewModel).SelectedTaxClass);
        }
    }
}
