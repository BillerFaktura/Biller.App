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

namespace Biller.UI.DocumentView.Contextual.EditTabs.Articles
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
                try
                {
                    dynamic document = ((DataContext as DocumentEditViewModel).Document);
                    document.OrderedArticles.Remove(button.DataContext as Data.Articles.OrderedArticle);
                }
                catch(Exception ex)
                {

                }
            }
        }

        private async void WatermarkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewmodel = (DataContext as DocumentEditViewModel);
            var exists = await viewmodel.ParentViewModel.ParentViewModel.Database.ArticleExists((sender as TextBox).Text);
            if (exists == true)
            {
                viewmodel.PreviewArticle = await viewmodel.ParentViewModel.ParentViewModel.Database.GetArticle((sender as TextBox).Text);
            }
            else
            {
                viewmodel.PreviewArticle = null;
            }
        }

        private void WatermarkTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var viewmodel = (DataContext as DocumentEditViewModel);
            if (e.Key == Key.Enter && viewmodel.PreviewArticle != null)
            {
                try
                {
                    var factory = viewmodel.ParentViewModel.GetFactory(viewmodel.Document.DocumentType);
                    factory.ReceiveData(viewmodel.PreviewArticle, viewmodel.Document);

                    viewmodel.PreviewArticle = null;
                    (sender as TextBox).Text = "";
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
