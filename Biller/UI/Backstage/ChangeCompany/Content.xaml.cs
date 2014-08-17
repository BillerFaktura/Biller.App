using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Biller.UI.Backstage.ChangeCompany
{
    /// <summary>
    /// Interaktionslogik für Content.xaml
    /// </summary>
    public partial class Content : UserControl
    {
        public Content()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Biller.Core.Models.CompanyInformation company = (sender as Button).DataContext as Biller.Core.Models.CompanyInformation;
            ChangeCompanyViewModel vm = this.DataContext as ChangeCompanyViewModel;
            //vm.ParentViewModel.ParentViewModel.Database.SaveOrUpdateSettings(vm.ParentViewModel.ParentViewModel.SettingsTabViewModel.KeyValueStore);
            await vm.ParentViewModel.ParentViewModel.Database.ChangeCompany(company);
            await vm.ParentViewModel.ParentViewModel.LoadData(true);
            vm.ParentViewModel.ParentViewModel.RibbonFactory.CloseBackstage();
        }
    }
}
