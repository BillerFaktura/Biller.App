using Biller.Data.Articles;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace Biller.UI.ArticleView.Contextual
{
    public class ArticleEditViewModel : Data.Utils.PropertyChangedHelper, Biller.UI.Interface.ITabContentViewModel
    {
        /// <summary>
        /// Constructor if you want to create a new article.
        /// </summary>
        /// <param name="parentViewModel"></param>
        public ArticleEditViewModel(ArticleView.ArticleTabViewModel parentViewModel, int ID = -1)
        {
            ParentViewModel = parentViewModel;
            EditContentTabs = new ObservableCollection<UIElement>();
            ContextualTabGroup = parentViewModel.ContextualTabGroup;
            EditContentTabs.Add(new Contextual.Controls.ArticleEditControlTabItem());
            Article = new Article();
            if (ID != -1)
                Article.ArticleID = ID.ToString();
            EditMode = false;
            TabItem = new ArticleEditTabItem(this);
            Content = new ArticleEditTabContent(this);
        }

        /// <summary>
        /// Constructor to edit data of an existing article.
        /// </summary>
        /// <param name="parentViewModel"></param>
        /// <param name="article"></param>
        public ArticleEditViewModel(ArticleView.ArticleTabViewModel parentViewModel, Article article)
        {
            ParentViewModel = parentViewModel;
            EditContentTabs = new ObservableCollection<UIElement>();
            ContextualTabGroup = parentViewModel.ContextualTabGroup;
            EditContentTabs.Add(new Contextual.Controls.ArticleEditControlTabItem());
            Article = article;
            EditMode = true;
            TabItem = new ArticleEditTabItem(this);
            Content = new ArticleEditTabContent(this);
        }

        public Fluent.RibbonContextualTabGroup ContextualTabGroup { get; private set; }

        public ArticleView.ArticleTabViewModel ParentViewModel { get; set; }

        private ArticleEditTabItem TabItem { get; set; }

        private ArticleEditTabContent Content { get; set; }

        public Fluent.RibbonTabItem RibbonTabItem
        {
            get { return TabItem; }
        }

        public System.Windows.UIElement TabContent
        {
            get { return Content; }
        }

        public ObservableCollection<UIElement> EditContentTabs { get; private set; }

        public string TabItemName
        {
            get
            {
                return "Artikel bearbeiten";
            }
        }

        public string TabItemDescription
        {
            get
            {
                return "Bearbeiten oder erstellen Sie einen Artikel und speichern ihn in der Stammdatenbank.";
            }
        }

        public Data.Articles.Article Article
        {
            get { return GetValue(() => Article); }
            set { SetValue(value); }
        }

        public void ReceiveCloseCommand()
        {
            TabItem.Focus(); // For MVVM //
            ParentViewModel.ParentViewModel.Database.UpdateTemporaryUsedArticleID(Article.ArticleID, "");
            ParentViewModel.ReceiveCloseArticleControl(this);
        }

        public async Task ReceiveSaveCommand()
        {
            TabItem.Focus(); // For MVVM //
            await ParentViewModel.SaveOrUpdateArticle(Article);
        }

        public async System.Threading.Tasks.Task LoadData()
        {

        }

        public ObservableCollection<Data.Utils.Unit> ArticleUnits
        {
            get { return ParentViewModel.ParentViewModel.SettingsTabViewModel.ArticleUnits; }
        }

        public ObservableCollection<Data.Utils.TaxClass> TaxClasses
        {
            get { return ParentViewModel.ParentViewModel.SettingsTabViewModel.TaxClasses; }
        }

        public bool EditMode { get; private set; }

        public void ReceiveData(object data)
        {
            if (data is Article)
            {
                var id = this.Article.ArticleID;
                this.Article = (data as Article);
                this.Article.ArticleID = id;
            }
        }
    }
}