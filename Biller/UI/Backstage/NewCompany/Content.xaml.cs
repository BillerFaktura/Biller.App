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

namespace Biller.UI.Backstage.NewCompany
{
    /// <summary>
    /// UI Control for the basic data of a new company
    /// </summary>
    public partial class Content : UserControl
    {
        public Content()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Office2013Button_Click(object sender, RoutedEventArgs e)
        {
            EditCompanyViewModel vm = (DataContext as EditCompanyViewModel);
            
            if (vm.EditMode)
            {
                vm.BackstageTabItem.Focus(); //For MVVM //
                vm.CompanySettings.MainAddress.CompanyName = vm.CompanyInformation.CompanyName;
                await vm.ParentViewModel.ParentViewModel.Database.SaveOrUpdateStorageableItem(vm.CompanySettings);
            }
            else
            {
                vm.BackstageTabItem.Focus(); //For MVVM //
                vm.CompanyInformation.GenerateNewID();
                vm.ParentViewModel.ParentViewModel.RibbonFactory.CloseBackstage();
                vm.ParentViewModel.ParentViewModel.Database.AddCompany(vm.CompanyInformation);
                vm.CompanySettings.MainAddress.CompanyName = vm.CompanyInformation.CompanyName;
                await vm.ParentViewModel.ParentViewModel.Database.ChangeCompany(vm.CompanyInformation);
                await vm.ParentViewModel.ParentViewModel.LoadData();
                await vm.ParentViewModel.ParentViewModel.Database.SaveOrUpdateStorageableItem(vm.CompanySettings);
                vm.ResetCompanyInformation();
            }
        }
    }
}
