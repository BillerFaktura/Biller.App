using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Biller.UI.ArticleView
{
    /// <summary>
    /// Interaktionslogik für OrderTabContent.xaml
    /// </summary>
    public partial class ArticleTabContent : UserControl
    {
        public ArticleTabContent(ArticleTabViewModel ViewModel)
        {
            InitializeComponent();
            ParentViewModel = ViewModel;
            DataContext = ViewModel;
        }

        private ArticleTabViewModel ParentViewModel;

        public void AddNewColumn(string columnheader, string binding, double starWidth)
        {
            var newColumn = new GridViewColumn();
            newColumn.Header = columnheader;
            newColumn.DisplayMemberBinding = new Binding(binding);

            ColumnDefinition c1 = new ColumnDefinition();
            c1.Width = new GridLength(starWidth, GridUnitType.Star);
            GridColumnWidths.ColumnDefinitions.Add(c1);

            var widthGrid = new Grid();
            string gridname = "Width" + columnheader;
            widthGrid.Name = gridname;
            Grid.SetColumn(widthGrid, GridColumnWidths.ColumnDefinitions.IndexOf(c1));
            GridColumnWidths.Children.Add(widthGrid);
            RegisterName(gridname, widthGrid);

            Binding widthBinding = new Binding("ActualWidth");
            widthBinding.ElementName = gridname;
            BindingOperations.SetBinding(newColumn, GridViewColumn.WidthProperty, widthBinding);

            GridView.Columns.Add(newColumn);
        }

        private async void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ParentViewModel.ReceiveEditArticleCommand(this);
        }
    }
}