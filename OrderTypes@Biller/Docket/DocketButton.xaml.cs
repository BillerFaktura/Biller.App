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

namespace OrderTypes_Biller.Docket
{
    /// <summary>
    /// Interaktionslogik für DocketButtob.xaml
    /// </summary>
    public partial class DocketButton : Fluent.Button
    {
        public DocketButton()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Biller.UI.DocumentView.Contextual.DocumentEditViewModel)
            {
                await (DataContext as Biller.UI.DocumentView.Contextual.DocumentEditViewModel).ReceiveInternalDocumentCreation(this, "Docket");
                (DataContext as Biller.UI.DocumentView.Contextual.DocumentEditViewModel).DocumentEditRibbonTabItem.ShowDocumentControls();
            }
        }
    }
}
