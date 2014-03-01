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

namespace Biller.UI.OrderView.Contextual.Controls
{
    /// <summary>
    /// Interaktionslogik für OrderFolder.xaml
    /// </summary>
    public partial class OrderFolder : UserControl
    {
        public OrderFolder()
        {
            InitializeComponent();
        }


        /// <summary>
        /// This methode gets called when the user clicks on one of the templated controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemtemplate_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
