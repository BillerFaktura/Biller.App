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

namespace Biller.UI.SettingsView.SettingsList.UnitSettings
{
    /// <summary>
    /// Interaktionslogik für UnitTabContent.xaml
    /// </summary>
    public partial class UnitTabContent : UserControl
    {
        public UnitTabContent()
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
            (DataContext as SettingsTabViewModel).SelectedUnit = new Core.Utils.Unit();
        }

        /// <summary>
        /// Save changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Office2013Button_Click_1(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsTabViewModel).TabContent.Focus(); //For MVVM //
            (DataContext as SettingsTabViewModel).SaveOrUpdateArticleUnit((DataContext as SettingsTabViewModel).SelectedUnit);
        }

        /// <summary>
        /// Selected item changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListView).SelectedItem != null)
            {
                var selected = (sender as ListView).SelectedItem as Core.Utils.Unit;
                var temp = new Core.Utils.Unit() { Name = selected.Name, DecimalDigits = selected.DecimalDigits, DecimalSeperator = selected.DecimalSeperator, ShortName = selected.ShortName, ThousandSeperator = selected.ThousandSeperator };
                (DataContext as SettingsTabViewModel).SelectedUnit = temp;
            }
        }
    }
}
