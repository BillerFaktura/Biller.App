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

namespace Biller.UI.OrderView.Contextual.EditTabs.Articles
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //var viewmodel = (DataContext as OrderEditViewModel);
        }

        /// <summary>
        /// Remove an <see cref="OrderedArticle"/> from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            if (button.DataContext is Data.Articles.OrderedArticle)
            {
                ((DataContext as OrderEditViewModel).Document as Data.Orders.Order).OrderedArticles.Remove(button.DataContext as Data.Articles.OrderedArticle);
            }
        }
    }
}
