using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.UI.Backstage.NewCompany
{
    public class NewCompanyViewModel : Interface.IBackstageContentViewModel
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
            throw new NotImplementedException();
        }

        public void ReceiveData(object data)
        {
            throw new NotImplementedException();
        }

        public Biller.Data.Models.CompanyInformation CompanyInformation { get; private set; }

        public Biller.Data.Models.CompanySettings CompanySettings { get; private set; }
    }
}
