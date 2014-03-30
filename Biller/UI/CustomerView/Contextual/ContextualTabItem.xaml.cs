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

namespace Biller.UI.CustomerView.Contextual
{
    /// <summary>
    /// Interaktionslogik für ContextualTabItem.xaml
    /// </summary>
    public partial class ContextualTabItem : Fluent.RibbonTabItem, Biller.UI.Interface.IRibbonTabItem
    {
        public ContextualTabItem(CustomerEditViewModel parentViewModel)
        {
            InitializeComponent();
            ParentViewModel = parentViewModel;
            Group = parentViewModel.ContextualTabGroup;
        }

        public UI.Interface.ITabContentViewModel ParentViewModel { get; private set; }

        private async void buttonAbort_Click(object sender, RoutedEventArgs e)
        {
            await (ParentViewModel as CustomerEditViewModel).ReceiveCloseCommand();
        }

        private async void buttonSaveAndExit_Click(object sender, RoutedEventArgs e)
        {
            await (ParentViewModel as CustomerEditViewModel).ReceiveSaveCommand();
            await (ParentViewModel as CustomerEditViewModel).ReceiveCloseCommand();
        }

        private async void buttonQuickSave_Click(object sender, RoutedEventArgs e)
        {
            await (ParentViewModel as CustomerEditViewModel).ReceiveSaveCommand();
        }
    }
}
