using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Biller.UI.DocumentView
{
    /// <summary>
    /// Interaktionslogik für OrderTabContent.xaml
    /// </summary>
    public partial class DocumentTabContent : UserControl
    {
        public DocumentTabContent(DocumentTabViewModel parentViewModel)
        {
            ParentViewModel = parentViewModel;
            DataContext = parentViewModel;
            InitializeComponent();
        }

        private DocumentTabViewModel ParentViewModel;

        /// <summary>
        /// Methode to add a dynamic column to the list. This espacially usefull if your addin adds additional data and you want to display it to the user.\n
        /// <example> The following shows how to use this methode
        /// <code>
        /// <pre>
        /// AddNewColumn("Orderstate", "State", 0.8);
        /// </pre>
        /// </code>
        /// The modified order object can look like this:\n
        /// <code>
        /// <pre>
        /// dynamic sampleOrder = new Data.Orders.PreviewOrder();
        /// sampleOrder.State = "Printed";
        /// </pre>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="columnheader">The ColumnHeader as string</param>
        /// <param name="binding">The Binding string as you would use it in your XAML-file</param>
        /// <param name="starWidth">The width with Star-units</param>
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
            widthBinding.ElementName =  gridname;
            BindingOperations.SetBinding(newColumn, GridViewColumn.WidthProperty, widthBinding);

            GridView.Columns.Add(newColumn);
        }

        private async void ListView1_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ParentViewModel.ReceiveEditOrderCommand(this);
        }
    }
}