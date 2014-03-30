using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.UI.Backstage.NewCompany
{
    public class NewCompanyViewModel : Data.Utils.PropertyChangedHelper, Interface.IBackstageContentViewModel
    {
        public NewCompanyViewModel(BackstageViewModel parent)
        {
            ParentViewModel = parent;
            BackstageTabItem = new ContentTabItem();
            BackstageTabItem.DataContext = this;

            CompanyInformation = new Data.Models.CompanyInformation();
            CompanySettings = new Data.Models.CompanySettings();
        }

        public Fluent.BackstageTabItem BackstageTabItem { get; private set; }

        public BackstageViewModel ParentViewModel { get; private set; }

        public Task LoadData()
        {
            // Do nothing
            return null;
        }

        public void ReceiveData(object data)
        {
            // Do nothing
        }

        public void ResetCompanyInformation()
        {
            CompanyInformation = new Data.Models.CompanyInformation();
            CompanySettings = new Data.Models.CompanySettings();
        }

        public Biller.Data.Models.CompanyInformation CompanyInformation { get { return GetValue(() => CompanyInformation); } private set { SetValue(value); } }

        public Biller.Data.Models.CompanySettings CompanySettings { get { return GetValue(() => CompanySettings); } private set { SetValue(value); } }
    }
}
