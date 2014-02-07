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

namespace Biller.UI.SettingsView
{
    /// <summary>
    /// Interaktionslogik für SettingsTabRibbonTabItem.xaml
    /// </summary>
    public partial class SettingsTabRibbonTabItem : Fluent.RibbonTabItem, Interface.IRibbonTabItem
    {
        public SettingsTabRibbonTabItem(SettingsTabViewModel parentViewModel)
        {
            InitializeComponent();
            ParentViewModel = parentViewModel;
        }

        public UI.Interface.ITabContentViewModel ParentViewModel { get; private set; }
    }
}
