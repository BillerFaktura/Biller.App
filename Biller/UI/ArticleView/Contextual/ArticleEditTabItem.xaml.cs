using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows;

namespace Biller.UI.ArticleView.Contextual
{
    /// <summary>
    /// Interaktionslogik für ArticleEditTabItem.xaml
    /// </summary>
    public partial class ArticleEditTabItem : Fluent.RibbonTabItem, Biller.UI.Interface.IRibbonTabItem
    {
        public ArticleEditTabItem(ArticleEditViewModel parentViewModel)
        {
            InitializeComponent();
            _ParentViewModel = parentViewModel;
            DataContext = parentViewModel;
            Group = parentViewModel.ContextualTabGroup;
        }

        private ArticleEditViewModel _ParentViewModel;

        public UI.Interface.ITabContentViewModel ParentViewModel
        {
            get { return _ParentViewModel; }
        }

        /// <summary>
        /// Handles actions from the "Close" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCloseArticle_Click(object sender, RoutedEventArgs e)
        {
            _ParentViewModel.ReceiveCloseCommand();
        }

        /// <summary>
        /// Handles actions from the "Save" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonArticleQuickSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfAllPropertiesSet())
            {
                await _ParentViewModel.ReceiveSaveCommand();
            }
        }

        /// <summary>
        /// Handles actions from the "Save & Exit" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonArticleSaveAndExit_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfAllPropertiesSet())
            {
                await _ParentViewModel.ReceiveSaveCommand();
                _ParentViewModel.ReceiveCloseCommand();
            }
        }

        /// <summary>
        /// Checks if all settings required for an <see cref="Article"/> are set.
        /// </summary>
        /// <returns></returns>
        private bool CheckIfAllPropertiesSet()
        {
            string missingproperties = "";
            bool result = true;
            if (_ParentViewModel.Article.ArticleUnit == null || string.IsNullOrEmpty(_ParentViewModel.Article.ArticleUnit.Name))
            {
                result = false;
                missingproperties += FindResource("saleunit").ToString() + "\n";
            }

            if (_ParentViewModel.Article.TaxClass == null || string.IsNullOrEmpty(_ParentViewModel.Article.TaxClass.Name))
            {
                result = false;
                missingproperties += FindResource("taxclass").ToString() + "\n";
            }

            if (string.IsNullOrEmpty(_ParentViewModel.Article.ArticleID))
            {
                result = false;
                missingproperties += FindResource("articlenumber").ToString() + "\n";
            }

            if (string.IsNullOrEmpty(_ParentViewModel.Article.ArticleDescription))
            {
                result = false;
                missingproperties += FindResource("articledescription").ToString() + "\n";
            }

            if (!result)
            {
                TaskDialog td = new TaskDialog();
                td.StandardButtons = TaskDialogStandardButtons.Ok;
                td.Caption = FindResource("missingproperties").ToString();
                td.InstructionText = FindResource("setmissingproperties").ToString();
                td.Icon = TaskDialogStandardIcon.Error;
                td.DetailsCollapsedLabel = FindResource("showmissingpropertieslist").ToString();
                td.DetailsExpandedLabel = FindResource("hidemissingpropertieslist").ToString();
                td.DetailsExpandedText = missingproperties;
                TaskDialogResult tdr = td.Show();
            }

            return result;
        }

        /// <summary>
        /// Logic for creating an <see cref="Article"/> from a template.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreateFromTemplate_Click(object sender, RoutedEventArgs e)
        {
            _ParentViewModel.ParentViewModel.ReceiveRequestArticleCommand(_ParentViewModel);
        }
    }
}