using System.Windows.Controls;
using System.Windows.Media;

namespace Biller.UI.ArticleView.Contextual.Controls
{
    /// <summary>
    /// This control allows to edit data from the object <see cref="Article"/>.The values are changing with data binding. \n
    /// Your viewmodel needs a property "Article" of the type <see cref="Article"/>.
    /// </summary>
    public partial class ArticleEditControl : UserControl
    {
        private string previousID = "";

        /// <summary>
        /// Creates a new instance of the control.
        /// </summary>
        public ArticleEditControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Checks if the entered <see cref="ArticleID"/> already exists or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if ((sender as TextBox).IsFocused == true)
                {
                    bool result = true;
                    try { result = await (DataContext as ArticleEditViewModel).ParentViewModel.ParentViewModel.Database.ArticleExists((sender as TextBox).Text); }
                    catch { }
                    if (result && (DataContext as ArticleEditViewModel).EditMode == false)
                    {
                        (sender as TextBox).BorderBrush = Brushes.Red;
                        (sender as TextBox).BorderThickness = new System.Windows.Thickness(2);
                    }
                    else
                    {
                        var converter = new System.Windows.Media.BrushConverter();
                        var brush = (Brush)converter.ConvertFromString("#FFABADB3");
                        (sender as TextBox).BorderBrush = brush;
                        (sender as TextBox).BorderThickness = new System.Windows.Thickness(1);
                    }
                    result = await (DataContext as ArticleEditViewModel).ParentViewModel.ParentViewModel.Database.UpdateTemporaryUsedArticleID(previousID, (sender as TextBox).Text);
                    previousID = (sender as TextBox).Text;
                }
            }
            catch { }
        }
    }
}