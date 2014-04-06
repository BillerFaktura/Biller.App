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

namespace OrderTypes_Biller.Invoice
{
    /// <summary>
    /// Interaktionslogik für Button.xaml
    /// </summary>
    public partial class Button : Fluent.Button
    {
        public Button()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Biller.UI.DocumentView.Contextual.DocumentEditViewModel)
            {
                await (DataContext as Biller.UI.DocumentView.Contextual.DocumentEditViewModel).ReceiveInternalDocumentCreation(this, "Invoice");
                (DataContext as Biller.UI.DocumentView.Contextual.DocumentEditViewModel).DocumentEditRibbonTabItem.ShowDocumentControls();
            }
        }
    }
}
