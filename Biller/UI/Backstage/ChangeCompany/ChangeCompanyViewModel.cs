using Biller.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.UI.Backstage.ChangeCompany
{
    public class ChangeCompanyViewModel : Data.Utils.PropertyChangedHelper, Interface.IBackstageContentViewModel
    {
        public ChangeCompanyViewModel(BackstageViewModel parent)
        {
            ParentViewModel = parent;
            BackstageTabItem = new ContentTabItem() { DataContext = this };
            AllCompanies = new ObservableCollection<CompanyInformation>();
        }

        public Fluent.BackstageTabItem BackstageTabItem { get; private set; }

        public BackstageViewModel ParentViewModel { get; private set; }

        public ObservableCollection<CompanyInformation> AllCompanies { get { return GetValue(() => AllCompanies); } set { SetValue(value); } }

        public async Task LoadData()
        {
            AllCompanies = new ObservableCollection<CompanyInformation>(await ParentViewModel.ParentViewModel.Database.GetCompanyList());
            //Console.WriteLine(AllCompanies.ToString());
        }

        public void ReceiveData(object data)
        {
            // Do nothing
        }


    }
}
