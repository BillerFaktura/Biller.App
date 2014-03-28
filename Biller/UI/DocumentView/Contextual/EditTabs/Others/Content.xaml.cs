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

namespace Biller.UI.DocumentView.Contextual.EditTabs.Others
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

        private void Office2013Button_Click(object sender, RoutedEventArgs e)
        {
            var viewmodel = (DataContext as DocumentEditViewModel);
            //(viewmodel.Document as Data.Orders.Order).AdditionalValuedPositions.Add(new Data.Models.MoneyDescriptionModel() { Description = "Test 1", MathematicalOperation = Data.Utils.MathematicalOperation.Add, Value = new Data.Utils.Money(12) });
        }
    }
}
