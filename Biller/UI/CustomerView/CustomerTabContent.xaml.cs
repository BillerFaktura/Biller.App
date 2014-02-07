using System.Windows.Controls;

namespace Biller.UI.CustomerView
{
    /// <summary>
    /// Interaktionslogik für CustomerTabContent.xaml
    /// </summary>
    public partial class CustomerTabContent : UserControl
    {
        public CustomerTabContent(CustomerTabViewModel parentViewModel)
        {
            InitializeComponent();
            ParentViewModel = parentViewModel;
        }

        private CustomerTabViewModel ParentViewModel;

        private async void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ParentViewModel.ReceiveEditCustomerCommand(this);
        }
    }
}