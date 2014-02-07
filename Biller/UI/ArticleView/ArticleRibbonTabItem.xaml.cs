namespace Biller.UI.ArticleView
{
    /// <summary>
    /// Interaktionslogik für ArticleRibbonTabItem.xaml
    /// </summary>
    public partial class ArticleRibbonTabItem : Fluent.RibbonTabItem, Biller.UI.Interface.IRibbonTabItem
    {
        public ArticleRibbonTabItem(ArticleTabViewModel ViewModel)
        {
            InitializeComponent();
            _ParentViewModel = ViewModel;
        }

        private ArticleTabViewModel _ParentViewModel;

        UI.Interface.ITabContentViewModel Interface.IRibbonTabItem.ParentViewModel
        {
            get { return _ParentViewModel; }
        }

        private async void buttonNewArticle_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await _ParentViewModel.ReceiveNewArticleCommand(this);
        }

        private async void buttonEditArticle_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await _ParentViewModel.ReceiveEditArticleCommand(this);
        }
    }
}