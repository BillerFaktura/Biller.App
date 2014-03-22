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

        /// <summary>
        /// Awaitable function that shows all controls to create a new <see cref="Article"/>.\n
        /// Generates a new <see cref="ArticleEditViewModel"/>, tries to get the next <see cref="ArticleID"/> and calls <see cref="AddTabContentViewModel"/> to add the viewmodel to the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public async Task ReceiveNewArticleCommand(object sender)
        {
            var tempid = await ParentViewModel.Database.GetNextArticleID();
            var orderEditControl = new ArticleView.Contextual.ArticleEditViewModel(this, tempid);
            ParentViewModel.AddTabContentViewModel(orderEditControl);
            orderEditControl.RibbonTabItem.IsSelected = true;
        }

        /// <summary>
        /// Awaitable function that either shows all controls to modify an existing <see cref="Article"/> or loads the selected <see cref="Article"/> and uses <see cref="ReceiveData"/> of the viewmodel referenced <see cref="ReceiveRequestArticleCommand"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public async Task ReceiveEditArticleCommand(object sender)
        {
            if (SelectedArticle != null)
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
        }

        /// <summary>
        /// Decides wheter to objects are equal or not
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Closes a specified <see cref="ArticleEditViewModel"/>.
        /// </summary>
        /// <param name="articleEditViewModel"></param>
        public void ReceiveCloseArticleControl(Contextual.ArticleEditViewModel articleEditViewModel)
        {
            //We need to change the visibility during a bug in the RibbonControl which shows the contextual TabHeader after removing a visible item
            articleEditViewModel.RibbonTabItem.Visibility = System.Windows.Visibility.Collapsed;

            ParentViewModel.RibbonFactory.RemoveTabItem(articleEditViewModel.RibbonTabItem);
            RibbonTabItem.IsSelected = true;
            ViewModelRequestingArticle = null;
        }

        /// <summary>
        /// Call this methode if you want to choose an <see cref="Article"/> of the whole database. The UI changes to the default articleoverview. After an double-click on an item the methode <see cref="ITabContentViewModel.ReceiveData()"/> gets called.
        /// </summary>
        /// <param name="source"></param>
        public void ReceiveRequestArticleCommand(Biller.UI.Interface.ITabContentViewModel source)
        {
            ViewModelRequestingArticle = source;
            ParentViewModel.SelectedContent = TabContent;
        }

        /// <summary>
        /// The viewmodel requesting an <see cref="Article"/>
        /// </summary>
        private Biller.UI.Interface.ITabContentViewModel ViewModelRequestingArticle { get; set; }

        /// <summary>
        /// Awaitable function loading all viewmodel specific data.
        /// </summary>
        /// <returns></returns>
        public async Task LoadData()
        {
            AllArticles = new ObservableCollection<Data.Articles.PreviewArticle>(await ParentViewModel.Database.AllArticles());
            DisplayedArticles = AllArticles;
        }

        /// <summary>
        /// Awaitable function to apply a filter on all articles
        /// </summary>
        /// <returns></returns>
        public async Task ApplyFilter()
        {
            await Task.Run(() => applyFilter());
        }

        private void applyFilter()
        {
            DisplayedArticles = new ObservableCollection<Data.Articles.PreviewArticle>(AllArticles);
        }

        /// <summary>
        /// Saves or update an <see cref="Article"/> to the current <see cref="IDatabase"/> and the UI.\n
        /// Awaitable
        /// </summary>
        /// <param name="source">The <see cref="Article"/> you want to save or update.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Receives data. This methode does not process any data.
        /// </summary>
        /// <param name="data"></param>
        public void ReceiveData(object data)
        {
            
        }
    }
}