using System.Windows.Controls;

namespace Biller.UI.ArticleView.Contextual
{
    /// <summary>
    /// Interaktionslogik für ArticleEditTabContent.xaml
    /// </summary>
    public partial class ArticleEditTabContent : UserControl
    {
        public ArticleEditTabContent(ArticleEditViewModel parentViewModel)
        {
            InitializeComponent();
            DataContext = parentViewModel;
        }
    }
}