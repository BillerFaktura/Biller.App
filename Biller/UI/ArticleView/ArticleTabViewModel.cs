using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Biller.UI.ArticleView
{
    /// <summary>
    /// ViewModel for the Articleview. This class manages all behaviour regarding articles
    /// </summary>
    public class ArticleTabViewModel : Data.Utils.PropertyChangedHelper, Biller.UI.Interface.ITabContentViewModel
    {
        public ArticleTabViewModel(ViewModel.MainWindowViewModel parentViewModel)
        {
            this.ParentViewModel = parentViewModel;
            ArticleRibbonTabItem = new ArticleRibbonTabItem(this) { DataContext = this };
            ArticleTabContent = new ArticleTabContent(this) { DataContext = this };
            ContextualTabGroup = new Fluent.RibbonContextualTabGroup() { Header = "Artikel bearbeiten", Background = Brushes.Orange, BorderBrush = Brushes.Orange, Visibility = System.Windows.Visibility.Visible };
            parentViewModel.RibbonFactory.AddContextualGroup(ContextualTabGroup);
        }

        /// <summary>
        /// Holds a reference to <see cref="ArticleRibbonTabItem"/>
        /// </summary>
        public ArticleRibbonTabItem ArticleRibbonTabItem { get; private set; }

        public ArticleTabContent ArticleTabContent { get; private set; }

        public ObservableCollection<Data.Articles.PreviewArticle> AllArticles { get { return GetValue(() => AllArticles); } set { SetValue(value); } }

        public ObservableCollection<Data.Articles.PreviewArticle> DisplayedArticles { get { return GetValue(() => DisplayedArticles); } set { SetValue(value); } }

        public ObservableCollection<object> AllCategories { get { return GetValue(() => AllCategories); } set { SetValue(value); } }

        public object SelectedCategory { get; set; }

        public String SearchString { get; set; }

        public ViewModel.MainWindowViewModel ParentViewModel { get; private set; }

        public Fluent.RibbonContextualTabGroup ContextualTabGroup { get; private set; }

        public Fluent.RibbonTabItem RibbonTabItem
        {
            get
            {
                return ArticleRibbonTabItem;
            }
        }

        public System.Windows.UIElement TabContent
        {
            get
            {
                return ArticleTabContent;
            }
        }

        public async Task ReceiveNewArticleCommand(object sender)
        {
            var tempid = await ParentViewModel.Database.GetNextArticleID();
            var orderEditControl = new ArticleView.Contextual.ArticleEditViewModel(this, tempid);
            ParentViewModel.AddTabContentViewModel(orderEditControl);
            orderEditControl.RibbonTabItem.IsSelected = true;
        }

        public async Task ReceiveEditArticleCommand(object sender)
        {
            if (ViewModelRequestingArticle == null)
            {
                var temp = await ParentViewModel.Database.GetArticle(SelectedArticle.ArticleID);
                var orderEditControl = new ArticleView.Contextual.ArticleEditViewModel(this, temp);
                ParentViewModel.AddTabContentViewModel(orderEditControl);
                orderEditControl.RibbonTabItem.IsSelected = true;
            }
            else
            {
                var temp = await ParentViewModel.Database.GetArticle(SelectedArticle.ArticleID);
                ViewModelRequestingArticle.ReceiveData(temp);
                ParentViewModel.SelectedContent = ViewModelRequestingArticle.TabContent;
                ViewModelRequestingArticle = null;
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public void ReceiveCloseArticleControl(Contextual.ArticleEditViewModel articleEditViewModel)
        {
            //We need to change the visibility during a bug in the RibbonControl which shows the contextual TabHeader after removing a visible item
            articleEditViewModel.RibbonTabItem.Visibility = System.Windows.Visibility.Collapsed;

            ParentViewModel.RibbonFactory.RemoveTabItem(articleEditViewModel.RibbonTabItem);
            RibbonTabItem.IsSelected = true;
            ViewModelRequestingArticle = null;
        }

        public void ReceiveRequestArticleCommand(Biller.UI.Interface.ITabContentViewModel source)
        {
            ViewModelRequestingArticle = source;
            ParentViewModel.SelectedContent = TabContent;
        }

        private Biller.UI.Interface.ITabContentViewModel ViewModelRequestingArticle { get; set; }

        public async System.Threading.Tasks.Task LoadData()
        {
            AllArticles = new ObservableCollection<Data.Articles.PreviewArticle>(await ParentViewModel.Database.AllArticles());
            DisplayedArticles = AllArticles;
        }

        public async Task ApplyFilter()
        {
            await Task.Run(() => applyFilter());
        }

        private void applyFilter()
        {
            DisplayedArticles = new ObservableCollection<Data.Articles.PreviewArticle>(AllArticles);
        }

        public async Task SaveOrUpdateArticle(Data.Articles.Article source)
        {
            var tempPreview = Data.Articles.PreviewArticle.FromArticle(source);
            if (AllArticles.Contains(tempPreview))
            {
                var index = AllArticles.IndexOf(tempPreview);
                AllArticles.RemoveAt(index); AllArticles.Insert(index, tempPreview);
            }
            else
            {
                AllArticles.Add(tempPreview);
            }
            await ApplyFilter();
            bool result = await ParentViewModel.Database.SaveOrUpdateArticle(source);
        }

        public Data.Articles.PreviewArticle SelectedArticle { get { return GetValue(() => SelectedArticle); } set { SetValue(value); } }

        public void ReceiveData(object data)
        {
            
        }
    }
}